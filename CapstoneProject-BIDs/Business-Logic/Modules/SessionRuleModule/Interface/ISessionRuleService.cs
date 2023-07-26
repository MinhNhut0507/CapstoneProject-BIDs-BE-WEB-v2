using Business_Logic.Modules.SessionRuleModule.Request;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.SessionRuleModule.Interface
{
    public interface ISessionRuleService
    {
        public Task<SessionRule> AddNewSessionRule(CreateSessionRuleRequest SessionRuleCreate);

        public Task<SessionRule> UpdateSessionRule(UpdateSessionRuleRequest SessionRuleUpdate);

        public Task<SessionRule> DeleteSessionRule(Guid SessionRuleDeleteID);

        public Task<ICollection<SessionRule>> GetAll();

        public Task<SessionRule> GetSessionRuleByID(Guid id);

        public Task<SessionRule> GetSessionRuleByName(string Name);

    }
}
