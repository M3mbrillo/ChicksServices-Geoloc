using Chicks.Core.Repository.Mongo;
using Chicks.Database.NoSql.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chicks.Database.NoSql
{
    public class ChicksRepositoryProviderMongo : ProviderMongoDB
    {
        public ChicksRepositoryProviderMongo(IMongoDatabase mongoDatabase) 
            : base(mongoDatabase)
        {
        }

        public RepositoryMongoDBBase<GeocoderResult> GeocoderResult => this.Provider<GeocoderResult>();
    }
}
