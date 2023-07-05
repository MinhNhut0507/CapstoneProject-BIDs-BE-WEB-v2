using Common.Utils.Repository.Interface;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.SessionRuleModule.Interface
{
    public interface ISessionRuleRepository : IRepository<SessionRule>
    {
        public Task<ICollection<SessionRule>> GetSessionRulesBy(
               Expression<Func<SessionRule, bool>> filter = null,
               Func<IQueryable<SessionRule>, ICollection<SessionRule>> options = null,
               string includeProperties = null
           );
    }
}
