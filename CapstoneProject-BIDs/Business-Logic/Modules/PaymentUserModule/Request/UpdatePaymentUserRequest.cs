using FluentValidation;
using System;

namespace Business_Logic.Modules.PaymentUserModule.Request
{
    public class UpdatePaymentUserRequest
    {
        public Guid Id { get; set; }
        public string DetailPaymentUser { get; set; }
    }
    public class UpdatePaymentUserRequestValidator : AbstractValidator<UpdatePaymentUserRequest>
    {
        public UpdatePaymentUserRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
            RuleFor(x => x.DetailPaymentUser).NotEmpty().NotNull();
        }
    }
}
