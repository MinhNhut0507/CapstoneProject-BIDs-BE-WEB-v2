using Common.Utils.Repository.Interface;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.SessionDetailModule.Interface
{
    public interface ISessionDetailRepository : IRepository<SessionDetail>
    {
        public Task<ICollection<SessionDetail>> GetSessionDetailsBy(
               Expression<Func<SessionDetail, bool>> filter = null,
               Func<IQueryable<SessionDetail>, ICollection<SessionDetail>> options = null,
               string includeProperties = null
           );
    }
}
