using Business_Logic.Modules.SessionDetailModule.Request;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.SessionDetailModule.Interface
{
    public interface ISessionDetailService
    {
        public Task<SessionDetail> AddNewSessionDetail(CreateSessionDetailRequest SessionDetailCreate);

        public Task<SessionDetail> UpdateSessionDetail(UpdateSessionDetailRequest SessionDetailUpdate);

        public Task<SessionDetail> DeleteSessionDetail(Guid? SessionDetailDeleteID);

        public Task<ICollection<SessionDetail>> GetAll();

        public Task<SessionDetail> GetSessionDetailByID(Guid? id);

        public Task<ICollection<SessionDetail>> GetSessionDetailIsActive();

        public Task<ICollection<SessionDetail>> GetSessionDetailsIsInActive();

        public Task<ICollection<SessionDetail>> GetSessionDetailByUser(Guid? id);

        public Task<ICollection<SessionDetail>> GetSessionDetailBySession(Guid? id);

    }
}
