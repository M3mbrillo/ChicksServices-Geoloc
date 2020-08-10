using Chicks.Database.NoSql;
using Chicks.Database.Sql;
using Chicks.Database.Sql.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chicks.Api.Geo.Application.Commands
{
    public class CreateGeocoderRequestHandler
        : IRequestHandler<CreateGeocoderRequestCommand, CreatedGeocoderRequestDto>
    {
        private readonly ChicksRepositoryProviderEF providerSql;
        private readonly ChicksRepositoryProviderMongo providerMongo;

        public CreateGeocoderRequestHandler(
            ChicksRepositoryProviderEF providerSql,
            ChicksRepositoryProviderMongo providerMongo)
        {
            this.providerSql = providerSql;
            this.providerMongo = providerMongo;
        }

        public async Task<CreatedGeocoderRequestDto> Handle(CreateGeocoderRequestCommand request, CancellationToken cancellationToken)
        {
            //TODO move to Aggregate layer of a DDD 
            // and encapsule the save in a sql and mongodb

            var result = await this.providerSql.RequestGeocoder.SaveAsync(new RequestGeocoder { 
                AtDateTime = DateTime.Now,
                City = request.City,
                Country = request.Country,
                State = request.State,
                Street = request.Street,
                Number = request.Number,
                ZipCode = request.ZipCode
            });

            await this.providerMongo.GeocoderResult.SaveAsync(new Database.NoSql.Models.GeocoderResult
            {
                RequerestId = result.Id,
                Estado = "PROCESANDO"
            });

            return new CreatedGeocoderRequestDto() { Id = result.Id };
        }
    }

    public class CreatedGeocoderRequestDto {
        public int Id { get; set; }
    }
}
