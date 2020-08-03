using System;
using System.Collections.Generic;
using System.Text;

namespace Chicks.Geocoder.Application.Models
{
    class QueueItemGeocodeRequest
    {
        public int Id { get; set; }

        public GeocodeRequest Request { get; set; }
    }

    class GeocodeRequest {
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string ZipCode { get; set; }
    }
}
