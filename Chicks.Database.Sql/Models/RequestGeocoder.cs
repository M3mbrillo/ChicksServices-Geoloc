using Chicks.Core.Repository.BaseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chicks.Database.Sql.Models
{
    public class RequestGeocoder : IId
    {
        public int Id { get; set; }

        public string Street { get; set; }

        public string Number { get; set; }

        public string City { get; set; }

        public string zipCode { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        public DateTime AtDateTime { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }
    }
}

namespace Chicks.Database.Sql.Models.Config
{
    internal class RequestGeocoderConfiguration : IEntityTypeConfiguration<RequestGeocoder>
    {
        public void Configure(EntityTypeBuilder<RequestGeocoder> builderEntity)
        {
            builderEntity.HasKey(x => x.Id);

            builderEntity.Property(x => x.Latitude).HasColumnType("decimal(12, 9)");
            builderEntity.Property(x => x.Longitude).HasColumnType("decimal(12, 9)");
        }
    }
}