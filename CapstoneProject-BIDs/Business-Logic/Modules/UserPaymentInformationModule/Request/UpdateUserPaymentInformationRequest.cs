using FluentValidation;
using System;

namespace Business_Logic.Modules.UserPaymentInformationModule.Request
{
    public class UpdateUserPaymentInformationRequest
    {
        public Guid UserId { get; set; }
        public string PayPalAccount { get; set; }
    }
    public class UpdateUserPaymentInformationRequestValidator : AbstractValidator<UpdateUserPaymentInformationRequest>
    {
        public UpdateUserPaymentInformationRequestValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().NotNull();
            RuleFor(x => x.PayPalAccount).NotEmpty().NotNull();
        }
    }
}
