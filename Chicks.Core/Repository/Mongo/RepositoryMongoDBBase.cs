using Chicks.Core.Repository.BaseModel;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Chicks.Core.Repository.Mongo
{

    // TODO Refactor and implenet IPK to find by pk
    public class RepositoryMongoDBBase<TDocument> : IRepositoryBase<TDocument>
        where TDocument : class, IObjectId //IPK
    {
        private IMongoDatabase MongoDatabase { get; }
        public string CollectionName { get; }
        private IMongoCollection<TDocument> Documents => this.MongoDatabase.GetCollection<TDocument>(this.CollectionName);

        public RepositoryMongoDBBase(IMongoDatabase mongoDatabase, string collectionName)
        {
            CollectionName = collectionName; //table
            MongoDatabase = mongoDatabase;
        }

        public IEnumerable<TDocument> Get()
            => this.Documents.Find(x => true).ToEnumerable();

        public IEnumerable<TDocument> Get(Expression<Func<TDocument, bool>> filter, FindOptions options = null) {
            return this.Documents.Find(filter, options).ToList();
        }

        public async Task<TDocument> SaveAsync(TDocument entity)
        {
            if (entity.IsNew())
                await this.Documents.InsertOneAsync(entity);
            else
                await this.Documents.ReplaceOneAsync(x => x._id == entity._id, entity);

            return entity;
        }


        TDocument IRepositoryBase<TDocument>.Save(TDocument entity)
        {
            throw new NotImplementedException();
        }

        IEnumerable<TDocument> IRepositoryBase<TDocument>.Save(IEnumerable<TDocument> entitys)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<TDocument>> IRepositoryBase<TDocument>.SaveAsync(IEnumerable<TDocument> entitys)
        {
            throw new NotImplementedException();
        }

        void IRepositoryBase<TDocument>.Delete(TDocument entityToDelete)
        {
            throw new NotImplementedException();
        }

        void IRepositoryBase<TDocument>.Delete(IEnumerable<TDocument> entitysToDelete)
        {
            throw new NotImplementedException();
        }

        bool IRepositoryBase<TDocument>.Exist(TDocument entity)
        {
            throw new NotImplementedException();
        }

    }
}
