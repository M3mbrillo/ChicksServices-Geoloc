using Chicks.Core.Repository.EFCore;
using Chicks.Database.Sql.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chicks.Database.Sql
{
    public class ChicksRepositoryProviderEF : ProviderEF<ChicksDbContext>
    {
        public ChicksRepositoryProviderEF(ChicksDbContext dbContext) 
            : base(dbContext)
        {
        }

        public RepositoryEFBase<RequestGeocoder> RequestGeocoder => this.Provider<RequestGeocoder>();
    }
}
