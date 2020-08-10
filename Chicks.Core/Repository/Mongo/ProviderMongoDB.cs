using Chicks.Core.Repository.BaseModel;
using Chicks.Core.Repository.Providers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chicks.Core.Repository.Mongo
{
    public class ProviderMongoDB : IRepositoryProvider
    {
        private Dictionary<Type, object> ActivesRepos { get; set; }
        //private MongoClient Client { get; }
        private IMongoDatabase MongoDatabase { get; }

        public ProviderMongoDB(IMongoDatabase mongoDatabase)
        {

            this.ActivesRepos = new Dictionary<Type, object>();
            this.MongoDatabase = mongoDatabase;
        }

        private string GetDefaultCollectionName(Type type)
        {
            return ((BsonCollectionAttribute)type.GetCustomAttributes(
                typeof(BsonCollectionAttribute),
                true).FirstOrDefault())?.CollectionName;
        }


        public RepositoryMongoDBBase<TDocument> Provider<TDocument>()
            where TDocument : class, IObjectId
        {
            var type = typeof(TDocument);

            var collectionName = this.GetDefaultCollectionName(type);
            return this.Provider<TDocument>(collectionName);
        }


        public RepositoryMongoDBBase<TDocument> Provider<TDocument>(string collectionName)
            where TDocument : class, IObjectId
        {
            var type = typeof(TDocument);


            if (!ActivesRepos.ContainsKey(type))
                ActivesRepos.Add(type, new RepositoryMongoDBBase<TDocument>(this.MongoDatabase, collectionName));

            return (RepositoryMongoDBBase<TDocument>)ActivesRepos[type];
        }

        IRepositoryBase IRepositoryProvider.Provider<TDocument>()
        {
            //return this.Provider<TDocument>();
            throw new NotImplementedException();
        }

        tRepo IRepositoryProvider.Provider<tRepo, tEntity>()
        {
            throw new NotImplementedException("RepositoryMongoProvider dont support custom repo yet");
        }
    }
}
