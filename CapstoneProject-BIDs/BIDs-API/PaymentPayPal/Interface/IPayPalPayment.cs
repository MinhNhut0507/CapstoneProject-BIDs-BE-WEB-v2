namespace BIDs_API.PaymentPayPal.Interface
{
    public interface IPayPalPayment
    {
        public Task<string> PaymentPaypalComplete(Guid SesionId, Guid UserID, string urlSuccess, string urlFail);
        public Task<string> CheckAndUpdateOrder(string orderId);
        public Task<string> PaymentPaypalJoining(Guid SesionId, Guid UserID, string urlSuccess, string urlFail);
        public Task<string> PaymentStaffReturnDeposit(Guid sessionId, Guid userId, Guid StaffId, string urlSuccess, string urlFail);
    }
}
