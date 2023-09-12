using FluentValidation;
using System;

namespace Business_Logic.Modules.PaymentStaffModule.Request
{
    public class CreatePaymentStaffRequest
    {
        public Guid StaffId { get; set; }
        public Guid SessionId { get; set; }
        public Guid UserId { get; set; }
        public string PayPalRecieveAccount { get; set; }
        public string PayPalTransactionId { get; set; }
        public string PaymentDetail { get; set; }
        public double Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Status { get; set; }
    }
    public class CreatePaymentStaffRequestValidator : AbstractValidator<CreatePaymentStaffRequest>
    {
        public CreatePaymentStaffRequestValidator()
        {
            RuleFor(x => x.StaffId).NotEmpty().NotNull();
            RuleFor(x => x.SessionId).NotEmpty().NotNull();
            RuleFor(x => x.UserId).NotEmpty().NotNull();
            RuleFor(x => x.PayPalRecieveAccount);
            RuleFor(x => x.PayPalTransactionId).NotEmpty().NotNull();
            RuleFor(x => x.PaymentDetail).NotEmpty().NotNull();
            RuleFor(x => x.Amount);
            RuleFor(x => x.PaymentDate).NotEmpty().NotNull();
            RuleFor(x => x.Status).NotEmpty().NotNull();
        }
    }
}
