using Chicks.Core.Repository.BaseModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chicks.Database.NoSql.Models
{
    [Core.Repository.Mongo.BsonCollection("GeocoderResult")]
    public class GeocoderResult : IObjectId
    {
        [BsonId]        
        public ObjectId _id { get; set; }
        public int RequerestId { get; set; }

        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }

        public string Estado { get; set; }
    }
}
