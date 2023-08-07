using FluentValidation;
using System;

namespace Business_Logic.Modules.PaymentStaffModule.Request
{
    public class UpdatePaymentStaffStatusRequest
    {
        public string TransactionId { get; set; }
        public string Status { get; set; }
    }
    public class UpdatePaymentStaffStatusRequestValidator : AbstractValidator<UpdatePaymentStaffStatusRequest>
    {
        public UpdatePaymentStaffStatusRequestValidator()
        {
            RuleFor(x => x.TransactionId).NotEmpty().NotNull();
            RuleFor(x => x.Status).NotEmpty().NotNull();
        }
    }
}
