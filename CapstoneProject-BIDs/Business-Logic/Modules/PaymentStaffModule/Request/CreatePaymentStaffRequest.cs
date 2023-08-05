using FluentValidation;
using System;

namespace Business_Logic.Modules.PaymentStaffModule.Request
{
    public class CreatePaymentStaffRequest
    {
        public Guid StaffId { get; set; }
        public Guid SessionId { get; set; }
        public Guid UserPaymentInformationId { get; set; }
        public Guid PayPalTransactionId { get; set; }
        public string PaymentDetail { get; set; }
        public double Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool Status { get; set; }
    }
    public class CreatePaymentStaffRequestValidator : AbstractValidator<CreatePaymentStaffRequest>
    {
        public CreatePaymentStaffRequestValidator()
        {
            RuleFor(x => x.StaffId).NotEmpty().NotNull();
            RuleFor(x => x.SessionId).NotEmpty().NotNull();
            RuleFor(x => x.UserPaymentInformationId).NotEmpty().NotNull();
            RuleFor(x => x.PayPalTransactionId).NotEmpty().NotNull();
            RuleFor(x => x.PaymentDetail).NotEmpty().NotNull();
            RuleFor(x => x.Amount).NotEmpty().NotNull();
            RuleFor(x => x.PaymentDate).NotEmpty().NotNull();
            RuleFor(x => x.Status);
        }
    }
}
