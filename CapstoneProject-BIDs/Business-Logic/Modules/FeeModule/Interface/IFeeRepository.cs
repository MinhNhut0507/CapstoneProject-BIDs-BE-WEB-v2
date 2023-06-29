using Common.Utils.Repository.Interface;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.FeeModule.Interface
{
    public interface IFeeRepository : IRepository<Fee>
    {
        public Task<ICollection<Fee>> GetFeesBy(
               Expression<Func<Fee, bool>> filter = null,
               Func<IQueryable<Fee>, ICollection<Fee>> options = null,
               string includeProperties = null
           );
    }
}
