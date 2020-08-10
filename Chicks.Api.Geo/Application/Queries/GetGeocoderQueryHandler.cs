using Chicks.Core.Repository.Mongo;
using Chicks.Database.NoSql;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chicks.Api.Geo.Application.Queries
{

    public class GetGeocoderQuery : IRequest<GeocoderRequestResult> 
    {
        public int Id { get; set; }
    }

    public class GetGeocoderQueryHandler : IRequestHandler<GetGeocoderQuery, GeocoderRequestResult>
    {
        public GetGeocoderQueryHandler(ChicksRepositoryProviderMongo mongo)
        {
            Mongo = mongo;
        }

        ChicksRepositoryProviderMongo Mongo { get; }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<GeocoderRequestResult> Handle(GetGeocoderQuery request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return this.GetGeocoder(request.Id);
        }

        GeocoderRequestResult GetGeocoder(int id) {

            var r = this.Mongo.GeocoderResult.Get(x => x.RequerestId == id).SingleOrDefault();

            if (r == null)
                return null;

            return new GeocoderRequestResult()
            {
                Id = r.RequerestId,
                Latitude = r.Latitud,
                Longitude = r.Longitud,
                Estado = r.Estado,

                UrlGoogleMap = $"http://maps.google.com/maps?q={r.Latitud},{r.Longitud}"
            };
        }

    }
}
