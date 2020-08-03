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
        private readonly ChicksRepositoryProvider provider;

        public CreateGeocoderRequestHandler(ChicksRepositoryProvider provider)
        {
            this.provider = provider;
        }

        public async Task<CreatedGeocoderRequestDto> Handle(CreateGeocoderRequestCommand request, CancellationToken cancellationToken)
        {
            var result = await this.provider.RequestGeocoder.SaveAsync(new RequestGeocoder { 
                AtDateTime = DateTime.Now,
                City = request.City,
                Country = request.Country,
                State = request.State,
                Street = request.Street,
                Number = request.Number,
                ZipCode = request.ZipCode
            });

            return new CreatedGeocoderRequestDto() { Id = result.Id };
        }
    }

    public class CreatedGeocoderRequestDto {
        public int Id { get; set; }
    }
}
