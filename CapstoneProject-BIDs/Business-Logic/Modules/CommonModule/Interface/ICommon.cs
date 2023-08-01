using Business_Logic.Modules.CommonModule.Response;
using Business_Logic.Modules.ItemModule.Request;
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
        public Task SendEmailFailAuction(Session session);
        public Task<ICollection<Session>> GetSessionInStageByAuctioneer(Guid id);
        public Task<ICollection<Session>> GetSessionCompleteByAuctioneer(Guid id);
        public Task<ICollection<Session>> GetSessionHaventTranferByAuctioneer(Guid id);
        public Task<Users> GetUserWinning(Guid id);
        public Task<UserNotiResponse> UserNotification(int ExDay, int TypeId, string message, Guid UserId);
        public Task<StaffNotiResponse> StaffNotification(int ExDay, int TypeId, string message, Guid StaffId);
        public Task<bool> CheckSessionJoining(Guid id);
        public Task<Users> GetUserWinningByJoining(Guid id);
        public Task<bool> CheckSessionIncrease(Guid id);
        public Task<BookingItem> ReAuction(UpdateItemRequest updateItemRequest, Guid id);

    }
}
