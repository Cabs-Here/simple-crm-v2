﻿    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Classroom.SimpleCRM.SqlDbServices;
    using Microsoft.AspNetCore.SpaServices.AngularCli;
    using Classroom.SimpleCRM.WebApi.Auth;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.IdentityModel.Tokens;
    using System.Text;
    using System;
    using NSwag.Generation.Processors.Security;
    using System.Collections.Generic;
    using NSwag;
    using NSwag.AspNetCore;
    using Newtonsoft.Json.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Classroom.SimpleCRM.WebApi.Filters;

namespace Classroom.SimpleCRM.WebApi
{
    public class Startup
    {
        private const string SecretKey = "sdkdhsHOQPdjspQNSHsjsSDQWJqzkpdnf"; //<-- NEW: make up a random key here
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCaching();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var googleOptions = Configuration.GetSection(nameof(GoogleAuthSettings));
            services.Configure<GoogleAuthSettings>(options =>
            {
                options.ClientId = googleOptions[nameof(GoogleAuthSettings.ClientId)];
                options.ClientSecret = googleOptions[nameof(GoogleAuthSettings.ClientSecret)];
            });

            var microsoftOptions = Configuration.GetSection(nameof(MicrosoftAuthSettings));
            services.Configure<MicrosoftAuthSettings>(options =>
            {
                options.ClientId = microsoftOptions[nameof(MicrosoftAuthSettings.ClientId)];
                options.ClientSecret = microsoftOptions[nameof(MicrosoftAuthSettings.ClientSecret)];
            });

            services.AddSpaStaticFiles(config =>
            {
                config.RootPath = Configuration["SpaRoot"];
            });

            var jwtOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });
            var tokenValidationPrms = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtOptions[nameof(JwtIssuerOptions.Issuer)],
                ValidateAudience = true,
                ValidAudience = jwtOptions[nameof(JwtIssuerOptions.Audience)],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,
                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationPrms;
                configureOptions.SaveToken = true; // allows token access in controller
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim(
                    Constants.JwtClaimIdentifiers.Rol,
                    Constants.JwtClaims.ApiAccess));
            });

            var identityBuilder = services.AddIdentityCore<CrmIdentityUser>(o =>
            { // add any custom password rules here
            });
            identityBuilder = new IdentityBuilder(
                identityBuilder.UserType,
                typeof(IdentityRole<Guid>),
                identityBuilder.Services);
            identityBuilder.AddEntityFrameworkStores<CrmIdentityDbContext>();
            identityBuilder.AddRoleValidator<RoleValidator<IdentityRole>>();
            identityBuilder.AddRoleManager<RoleManager<IdentityRole<Guid>>>();
            identityBuilder.AddSignInManager<SignInManager<CrmIdentityUser>>();
            identityBuilder.AddDefaultTokenProviders();

            services.AddOpenApiDocument(options =>
            {
                options.DocumentName = "v1";
                options.Title = "Simple CRM";
                options.Version = "1.0";
                options.DocumentProcessors.Add(new SecurityDefinitionAppender("JWT token",
                    new List<string>(), //no scope names to add
                    new OpenApiSecurityScheme
                    {
                        In = OpenApiSecurityApiKeyLocation.Header,
                        Name = "Authorization",
                        Type = OpenApiSecuritySchemeType.ApiKey,
                        Description = "Type into the textbox: 'Bearer {your_JWT_token}'. You can get a JWT from endpoints: '/auth/register' or '/auth/login'"
                    }
                ));
                options.OperationProcessors.Add(
                    new OperationSecurityScopeProcessor("JWT token"));
            });

            services.AddMvc(o => {
                o.Filters.Add(typeof(GlobalExceptionFilter));
            })
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                    .AddJsonOptions(options =>
                    {
                        var settings = options.SerializerSettings;
                        settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        settings.Converters = new JsonConverter[]
                        {
                        new IsoDateTimeConverter(),
                        new StringEnumConverter(true)
                        };
                    });
            services.AddDbContext<CrmIdentityDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<CrmDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddSingleton<IJwtFactory, JwtFactory>();
            services.AddScoped<ICustomerData, SqlCustomerData>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper, UrlHelper>(factory =>
            {
                var context = factory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(context);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseResponseCaching();
            app.UseSpaStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseOpenApi();
            app.UseSwaggerUi3(settings =>
            {
                var microsoftOptions = Configuration.GetSection(nameof(MicrosoftAuthSettings));
                settings.OAuth2Client = new OAuth2ClientSettings
                {
                    ClientId = microsoftOptions[nameof(MicrosoftAuthSettings.ClientId)],
                    ClientSecret = microsoftOptions[nameof(MicrosoftAuthSettings.ClientSecret)],
                    AppName = "Roba",
                    Realm = "Nexul Academy"
                };
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "identity",
                    template: "Identity/{controller=Account}/{action=Register}/{id?}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseWhen(
                context => !context.Request.Path.StartsWithSegments("/api"),
                appBuilder => appBuilder.UseSpa(spa =>
                {
                    spa.Options.SourcePath = "../simple-crm-cli";
                    if (env.IsDevelopment())
                    {
                        spa.UseAngularCliServer(npmScript: "start");
                    }
                }));
        }
    }
}