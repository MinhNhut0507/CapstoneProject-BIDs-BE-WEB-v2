﻿using Business_Logic.Modules.CommonModule.Data;
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
        public Task<Users> SendEmailWinnerAuction(Session session);
        public Task<Users> SendEmailCompleteAuction(Session session);
        public Task SendEmailFailAuction(Session session);
        public Task<ICollection<Session>> GetSessionInStageByAuctioneer(Guid id);
        public Task<ICollection<Session>> GetSessionCompleteByAuctioneer(Guid id);
        public Task<ICollection<Session>> GetSessionHaventTranferByAuctioneer(Guid id);
        public Task<ICollection<Session>> GetSessionFailByAuctioneer(Guid id);
        public Task<ICollection<Session>> GetSessionErrorItemByAuctioneer(Guid id);
        public Task<ICollection<Session>> GetSessionReceivedByAuctioneer(Guid id);
        public Task<ICollection<Session>> GetSessionNeedToPayByUser(Guid id);
        public Task<ICollection<Session>> GetSessionCompleteByWinner(Guid id);
        public Task<ICollection<Session>> GetSessionReceivedByWinner(Guid id);
        public Task<ICollection<Session>> GetSessionErrorItemByWinner(Guid id);
        public Task<Users> GetUserWinning(Guid id);
        public Task<UserNotiResponse> UserNotification(int ExDay, int TypeId, string message, Guid UserId);
        public Task<StaffNotiResponse> StaffNotification(int ExDay, int TypeId, string message, Guid StaffId);
        public Task<bool> CheckSessionJoining(Guid id);
        public Task<Users> GetUserWinningByJoining(Guid id);
        public Task<bool> CheckSessionIncrease(Guid id);
        public Task<Item> ReAuction(UpdateItemRequest updateItemRequest);
        public Task<BookingItem> ReAuctionItem(UpdateItemRequest reAuctionRequest);
        public Task<UTCCode> ConfirmEmail(string email);
        public Task<bool> CheckUTCCode(string codeInput, string codeCheck);
        public Task<double> Exchange();
        public Task<ICollection<Users>> GetUserJoinSession(Guid sessionId);
        public Task<ICollection<Session>> GetSessionFailHadJoin();
        public Task<ReportTotalSessionPayment> ReportSessionTotal();
        public Task<ReportPaymentUser> ReportPaymentUser(Guid UserId, DateTime Start, DateTime End);
        public Task<ReportPaymentUser> ReportPaymentUserToTal(Guid UserId);
        public Task<ReportPaymentUser> ReportPaymentToTal();
        public Task<ReportUser> ReportUser(DateTime startDate, DateTime endDate);
        public Task<ReportSessionTotal> ReportCategoryDetail(Guid CategoryId, DateTime startDate, DateTime endDate);
        public Task<ReportSessionTotal> ReportSessionTotalByDate(DateTime Start, DateTime End);
    }
}
