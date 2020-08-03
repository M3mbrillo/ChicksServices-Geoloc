using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Chicks.Database.Sql
{
    public class ChicksDbContext : DbContext
    {
        DbSet<Models.RequestGeocoder> GeocoderRequests { get; set; }

        public ChicksDbContext([NotNull] DbContextOptions options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Models.RequestGeocoder).Assembly);
        }
    }
}
