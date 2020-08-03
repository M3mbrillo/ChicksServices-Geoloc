using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.IO;


namespace Chicks.Database.Sql.Migrations
{
    public class DesignTimeFactory : IDesignTimeDbContextFactory<ChicksDbContext>
    {
        public ChicksDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json")
                                    .Build();

            var builder = new DbContextOptionsBuilder<ChicksDbContext>();

            var connectionString = configuration.GetConnectionString("ChicksSqlServer");

            builder.UseSqlServer(connectionString);

            return new ChicksDbContext(builder.Options);
        }
    }
}
