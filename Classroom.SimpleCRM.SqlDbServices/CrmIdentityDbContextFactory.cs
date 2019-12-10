
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;

namespace Classroom.SimpleCRM.SqlDbServices
{
    public class CrmIdentityDbContextFactory : IDesignTimeDbContextFactory<CrmIdentityDbContext>
    {
        public CrmIdentityDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<CrmIdentityDbContext>();
            builder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=SimpleCrmV2;Trusted_Connection=True;MultipleActiveResultSets=true",
                optionsbuilder => optionsbuilder.MigrationsAssembly(typeof(CrmIdentityDbContext).GetTypeInfo().Assembly.GetName().Name));
            return new CrmIdentityDbContext(builder.Options);
        }
    }
}