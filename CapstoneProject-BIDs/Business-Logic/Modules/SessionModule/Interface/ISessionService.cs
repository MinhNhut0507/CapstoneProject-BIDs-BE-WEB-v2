using Business_Logic.Modules.SessionModule.Request;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.SessionModule.Interface
{
    public interface ISessionService
    {
        public Task<Session> AddNewSession(CreateSessionRequest SessionCreate);

        public Task<Session> UpdateSession(UpdateSessionRequest SessionUpdate);

        public Task UpdatePriceSession(Guid id, double price);

        public Task<Session> UpdateSessionStatusInStage(UpdateSessionStatusRequest SessionUpdate);

        public Task<Session> UpdateSessionStatusNotStart(UpdateSessionStatusRequest SessionUpdate);
        public Task<Session> UpdateSessionStatusHaventTranfer(UpdateSessionStatusRequest SessionUpdate);
        public Task<Session> UpdateSessionStatusComplete(UpdateSessionStatusRequest SessionUpdate);

        public Task<Session> DeleteSession(Guid? SessionDeleteID);

        public Task<ICollection<Session>> GetAll();

        public Task<ICollection<Session>> GetSessionByID(Guid? id);

        public Task<ICollection<Session>> GetSessionByName(string Name);

        public Task<ICollection<Session>> GetSessionsIsNotStart();

        public Task<ICollection<Session>> GetSessionsIsInStage();

        public Task<ICollection<Session>> GetSessionsIsComplete();

        public Task<ICollection<Session>> GetSessionsIsHaventPay();

        public Task<ICollection<Session>> GetSessionsIsOutOfDate();

        public Task<ICollection<Session>> GetSessionsIsNotStartByUser(Guid id);

        public Task<ICollection<Session>> GetSessionsIsInStageByUser(Guid id);

        public Task<ICollection<Session>> GetSessionsIsCompleteByUser(Guid id);

        public Task<ICollection<Session>> GetSessionsIsHaventPayByUser(Guid id);

        public Task<ICollection<Session>> GetSessionsIsOutOfDateByUser(Guid id);
    }
}
