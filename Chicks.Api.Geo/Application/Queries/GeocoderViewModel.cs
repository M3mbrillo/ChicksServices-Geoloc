using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chicks.Api.Geo.Application.Queries
{
    public class GeocoderRequestResult
    {
        public int Id { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
