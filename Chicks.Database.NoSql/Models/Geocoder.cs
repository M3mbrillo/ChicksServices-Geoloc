using Chicks.Core.Repository.BaseModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chicks.Database.NoSql.Models
{
    public class Geocoder : IId
    {
        public int Id { get; set; }
        public int RequerestId { get; set; }

        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }        
    }
}
