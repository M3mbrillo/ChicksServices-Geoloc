using Chicks.Core.Repository.BaseModel;
using Chicks.Core.Repositorys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Chicks.Core.Repository.EFCore
{
    public class RepositoryEFBase<TEntity> : IRepositoryBase<TEntity>
        where TEntity : class, IId
    {
        protected DbContext context;
        protected DbSet<TEntity> dbSet;

        public RepositoryEFBase() { }
        public RepositoryEFBase(DbContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }

        private IQueryable<TEntity> GetQ(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
                query = query.Where(filter);


            foreach (string includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return orderBy != null
                    ? orderBy(query)
                    : query;
        }

        public virtual IEnumerable<TEntity> Get() => this.Get(null, null, "");

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        => GetQ(filter, orderBy, includeProperties).ToList();

        public virtual TEntity GetSingle(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        => GetQ(filter, orderBy, includeProperties).SingleOrDefault();


        public async virtual Task<IEnumerable<TEntity>> GetAsync(
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           string includeProperties = "")
        => await GetQ(filter, orderBy, includeProperties).ToListAsync();


        public bool Exist(int Id)
        {
            return dbSet.Any(x => x.Id == Id);
        }

        public bool Exist(TEntity entity)
        {
            if (entity == null || entity.Id == 0)
                return false;

            return Exist(entity.Id);
        }


        public virtual IEnumerable<TEntity> GetAll() => Get();

        public virtual TEntity Get(int id)
            => dbSet.SingleOrDefault(x => x.Id == id);

        public virtual TEntity Get(int? id)
            => id.HasValue ? dbSet.SingleOrDefault(x => x.Id == id.Value) : null;


        private void SaveAttach(TEntity entity)
        {
            if (entity.IsNew())
            {
                dbSet.Add(entity);
                context.Entry(entity).State = EntityState.Added;
            }
            else
            {
                dbSet.Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
            }
        }

        public virtual async Task<TEntity> SaveAsync(TEntity entity)
        {
            SaveAttach(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public virtual TEntity Save(TEntity entity)
        {
            SaveAttach(entity);
            context.SaveChanges();
            return entity;
        }


        public virtual async Task<IEnumerable<TEntity>> SaveAsync(IEnumerable<TEntity> entitys)
        {
            foreach (TEntity entity in entitys)
                SaveAttach(entity);

            await context.SaveChangesAsync();
            return entitys;
        }

        public virtual IEnumerable<TEntity> Save(IEnumerable<TEntity> entitys)
        {
            foreach (TEntity entity in entitys)
                SaveAttach(entity);

            context.SaveChanges();
            return entitys;
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }

            dbSet.Remove(entityToDelete);
            context.SaveChanges();
        }

        public virtual void Delete(IEnumerable<TEntity> entitysToDelete)
        {
            entitysToDelete.Where(etd => context.Entry(etd).State == EntityState.Detached)
                            .ToList().ForEach(x => dbSet.Attach(x));

            dbSet.RemoveRange(entitysToDelete);

            context.SaveChanges();
        }
    }
}
}
