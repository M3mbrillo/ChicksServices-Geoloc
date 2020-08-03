using Nominatim.API.Geocoders;
using Nominatim.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chicks.Geocoder.Application
{
    class GeocoderApi
    {
        public async Task<Models.GeocodeResponse> GetGeocoderAsync(Models.QueueItemGeocodeRequest command)
        {
            var client = new ForwardGeocoder();

            var queryString = !command.Request.Country.IsNullOrEmpty() ? command.Request.Country + ", " : "";
            queryString += !command.Request.State.IsNullOrEmpty() ? command.Request.State + ", " : "";
            queryString += !command.Request.City.IsNullOrEmpty() ? command.Request.City + ", " : "";
            queryString += !command.Request.Street.IsNullOrEmpty() ? command.Request.Street + " " : "";
            queryString += !command.Request.Number.IsNullOrEmpty() ? command.Request.Number: "";

            var result = await client.Geocode(new ForwardGeocodeRequest() { 
                queryString = queryString,
                BreakdownAddressElements = true,
                ShowExtraTags = true,
                ShowAlternativeNames = true,
                ShowGeoJSON = true
            });

            if (result.Length < 1)
                return null;

            var r = result.FirstOrDefault();

            return new Models.GeocodeResponse()
            {
                Id = command.Id,
                Latitude = r.Latitude,
                Longitude = r.Longitude
            };
        }
    }

    public static class StrignExtension
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
    }
}
