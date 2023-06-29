using Common.Utils.Repository.Interface;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.SessionModule.Interface
{
    public interface ISessionRepository : IRepository<Session>
    {
        public Task<ICollection<Session>> GetSessionsBy(
               Expression<Func<Session, bool>> filter = null,
               Func<IQueryable<Session>, ICollection<Session>> options = null,
               string includeProperties = null
           );
    }
}
