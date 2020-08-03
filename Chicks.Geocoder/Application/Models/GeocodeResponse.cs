using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace Chicks.Geocoder.Application.Models
{
    class GeocodeResponse
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
