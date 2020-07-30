using Chicks.Core.Repository.BaseModel;
using Chicks.Core.Repositorys;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chicks.Core.Repository.Providers
{
    public interface IRepositoryProvider
    {
        IRepositoryBase Provider<TEntity>()
            where TEntity : class, IId;

        TRepo Provider<TRepo, TEntity>()
            where TRepo : class, IRepositoryBase
            where TEntity : class, IId;
    }
}
