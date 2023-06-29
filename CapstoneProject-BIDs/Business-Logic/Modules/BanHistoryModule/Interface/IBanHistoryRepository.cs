using Common.Utils.Repository.Interface;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.BanHistoryModule.Interface
{
    public interface IBanHistoryRepository : IRepository<BanHistory>
    {
        public Task<ICollection<BanHistory>> GetBanHistorysBy(
               Expression<Func<BanHistory, bool>> filter = null,
               Func<IQueryable<BanHistory>, ICollection<BanHistory>> options = null,
               string includeProperties = null
           );
    }
}
