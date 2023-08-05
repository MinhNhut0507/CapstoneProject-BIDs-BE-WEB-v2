using FluentValidation;
using System;

namespace Business_Logic.Modules.UserPaymentInformationModule.Request
{
    public class UpdateUserPaymentInformationRequest
    {
        public Guid Id { get; set; }
        public string PayPalAccount { get; set; }
        public bool Status { get; set; }
    }
    public class UpdateUserPaymentInformationRequestValidator : AbstractValidator<UpdateUserPaymentInformationRequest>
    {
        public UpdateUserPaymentInformationRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
            RuleFor(x => x.PayPalAccount).NotEmpty().NotNull();
            RuleFor(x => x.Status);
        }
    }
}
