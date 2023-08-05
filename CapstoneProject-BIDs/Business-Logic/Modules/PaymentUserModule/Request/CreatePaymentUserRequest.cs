using FluentValidation;
using System;

namespace Business_Logic.Modules.PaymentUserModule.Request
{
    public class CreatePaymentUserRequest
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid SessionId { get; set; }
        public Guid PayPalTransactionId { get; set; }
        public string PaymentDetail { get; set; }
        public double Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool Status { get; set; }
    }
    public class CreatePaymentUserRequestValidator : AbstractValidator<CreatePaymentUserRequest>
    {
        public CreatePaymentUserRequestValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().NotNull();
            RuleFor(x => x.SessionId).NotEmpty().NotNull();
            RuleFor(x => x.PayPalTransactionId).NotEmpty().NotNull();
            RuleFor(x => x.PaymentDetail).NotEmpty().NotNull();
            RuleFor(x => x.Amount).NotEmpty().NotNull();
            RuleFor(x => x.PaymentDate).NotEmpty().NotNull();
            RuleFor(x => x.Status);
        }
    }
}
