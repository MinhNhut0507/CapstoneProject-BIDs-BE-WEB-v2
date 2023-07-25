using Business_Logic.Modules.SessionRuleModule.Request;
using Business_Logic.Modules.SessionRuleModule.Response;
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
        public Task<ReturnSessionRule> AddNewSessionRule(CreateSessionRuleRequest SessionRuleCreate);

        public Task<ReturnSessionRule> UpdateSessionRule(UpdateSessionRuleRequest SessionRuleUpdate);

        public Task<ReturnSessionRule> DeleteSessionRule(Guid SessionRuleDeleteID);

        public Task<ICollection<SessionRule>> GetAll();

        public Task<ReturnSessionRule> GetSessionRuleByID(Guid id);

        public Task<ReturnSessionRule> GetSessionRuleByName(string Name);

    }
}
