using FluentValidation;
using System;

namespace Business_Logic.Modules.PaymentUserModule.Request
{
    public class UpdatePaymentUserStatusRequest
    {
        public string TransactionId { get; set; }
        public string Status { get; set; }
    }
    public class UpdatePaymentUserStatusRequestValidator : AbstractValidator<UpdatePaymentUserStatusRequest>
    {
        public UpdatePaymentUserStatusRequestValidator()
        {
            RuleFor(x => x.TransactionId).NotEmpty().NotNull();
            RuleFor(x => x.Status).NotEmpty().NotNull();
        }
    }
}
