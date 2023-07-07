using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.SendEmailModule.Interface
{
    public interface ISendEmail
    {
        public Task SendEmailBeginAuction(Session session);
        public Task SendEmailWinnerAuction(Session session);
        public Task SendEmailOutOfDateAuction(Session session);
    }
}
