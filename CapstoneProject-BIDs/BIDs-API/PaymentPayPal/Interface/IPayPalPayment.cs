namespace BIDs_API.PaymentPayPal.Interface
{
    public interface IPayPalPayment
    {
        public Task<string> PaymentPaypalComplete(Guid SesionId, Guid UserID, string urlSuccess, string urlFail);
        public Task<string> CheckAndUpdateOrderComplete(Guid userId);
        public Task<string> PaymentPaypalJoining(Guid SesionId, Guid UserID, string urlSuccess, string urlFail);
        public Task<string> PaymentStaffReturnDeposit(Guid sessionId);
        public Task<string> PaymentStaffToWinner(Guid sessionId, Guid userId, Guid staffId, string urlSuccess, string urlFail);
        public Task<string> PaymentStaffToUserSuccessSession(Guid sessionId);
    }
}
