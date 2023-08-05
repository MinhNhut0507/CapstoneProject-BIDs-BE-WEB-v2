using FluentValidation;
using System;

namespace Business_Logic.Modules.PaymentStaffModule.Request
{
    public class UpdatePaymentStaffRequest
    {
        public Guid Id { get; set; }
        public string DetailPaymentStaff { get; set; }
    }
    public class UpdatePaymentStaffRequestValidator : AbstractValidator<UpdatePaymentStaffRequest>
    {
        public UpdatePaymentStaffRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
            RuleFor(x => x.DetailPaymentStaff).NotEmpty().NotNull();
        }
    }
}
