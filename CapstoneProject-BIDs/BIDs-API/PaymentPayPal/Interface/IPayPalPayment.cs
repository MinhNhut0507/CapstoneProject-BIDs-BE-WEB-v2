namespace BIDs_API.PaymentPayPal.Interface
{
    public interface IPayPalPayment
    {
        public Task<string> PaymentPaypal(Guid SesionId, Guid payerId);
    }
}
