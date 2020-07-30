using Chicks.Core.Repository.BaseModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chicks.Core.Repositorys
{
    public interface IRepositoryBase<TEntity> : IRepositoryBase
       where TEntity : class, IId
    {
        public IEnumerable<TEntity> Get();

        public Task<TEntity> SaveAsync(TEntity entity);

        public TEntity Save(TEntity entity);

        public Task<IEnumerable<TEntity>> SaveAsync(IEnumerable<TEntity> entitys);

        public IEnumerable<TEntity> Save(IEnumerable<TEntity> entitys);

        public void Delete(TEntity entityToDelete);

        public void Delete(IEnumerable<TEntity> entitysToDelete);

        public bool Exist(TEntity entity);


        #region hide non-generic accesor of RepositoryBase, C# 8 allow implements on interface
        bool IRepositoryBase.Exist(object entity) => this.Exist((entity as TEntity).Id);
        bool IRepositoryBase.Exist(int id) => this.Exist(id);

        async Task<object> IRepositoryBase.SaveAsync(object entity) => await this.SaveAsync(entity as TEntity);

        object IRepositoryBase.Save(object entity) => this.Save(entity as TEntity);

        async Task<IEnumerable<object>> IRepositoryBase.SaveAsync(IEnumerable<object> entitys) => await this.SaveAsync(entitys as IEnumerable<TEntity>);

        IEnumerable<object> IRepositoryBase.Save(IEnumerable<object> entitys) => this.Save(entitys as IEnumerable<TEntity>);

        void IRepositoryBase.Delete(object entityToDelete) => this.Delete(entityToDelete as TEntity);

        void IRepositoryBase.Delete(IEnumerable<object> entitysToDelete) => this.Delete(entitysToDelete as IEnumerable<TEntity>);
        #endregion
    }




    /// <summary>
    ///     The non-generic objetive of non-generic RepositoryBase is could be cast frm RepositoryBase'object to a RepositoryBase
    ///     Like thad IEnumrable'T and IEnumerable
    ///     https://stackoverflow.com/questions/21203004/generic-interface-inheriting-non-generic-one-c-sharp
    /// </summary>
    public interface IRepositoryBase
    {
        public Task<object> SaveAsync(object entity);

        public object Save(object entity);

        public Task<IEnumerable<object>> SaveAsync(IEnumerable<object> entitys);

        public IEnumerable<object> Save(IEnumerable<object> entitys);

        public void Delete(object entityToDelete);

        public void Delete(IEnumerable<object> entitysToDelete);

        public bool Exist(int Id);

        public bool Exist(object entity);
    }
}
