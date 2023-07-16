using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.CommonModule.Interface
{
    public interface ICommon
    {
        public Task SendEmailBeginAuction(Session session);
        public Task SendEmailWinnerAuction(Session session);
        public Task SendEmailOutOfDateAuction(Session session);
        public Task<ICollection<Session>> GetSessionInStageByUser(Guid id);
    }
}
