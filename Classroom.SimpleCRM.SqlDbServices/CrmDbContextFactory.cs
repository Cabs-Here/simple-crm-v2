using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;

namespace Classroom.SimpleCRM.SqlDbServices
{
    public class CrmDbContextFactory : IDesignTimeDbContextFactory<CrmDbContext>
    {
        public CrmDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<CrmDbContext>();
            builder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=SimpleCrmV2;Trusted_Connection=True;MultipleActiveResultSets=true",
                optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(CrmDbContext).GetTypeInfo().Assembly.GetName().Name));
            return new CrmDbContext(builder.Options);
        }
    }
}