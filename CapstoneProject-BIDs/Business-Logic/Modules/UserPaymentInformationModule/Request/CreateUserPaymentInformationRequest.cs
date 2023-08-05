using FluentValidation;
using System;

namespace Business_Logic.Modules.UserPaymentInformationModule.Request
{
    public class CreateUserPaymentInformationRequest
    {
        public Guid UserId { get; set; }
        public string PayPalAccount { get; set; }
    }
    public class CreateUserPaymentInformationRequestValidator : AbstractValidator<CreateUserPaymentInformationRequest>
    {
        public CreateUserPaymentInformationRequestValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().NotNull();
            RuleFor(x => x.PayPalAccount).NotEmpty().NotNull();
        }
    }
}
