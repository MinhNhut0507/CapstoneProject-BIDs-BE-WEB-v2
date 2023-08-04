using FluentValidation;
using System;

namespace Business_Logic.Modules.FeeModule.Request
{
    public class CreateFeeRequest
    {
        public string Name { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public double ParticipationFee { get; set; }
        public double DepositFee { get; set; }
        public double Surcharge { get; set; }
    }
    public class CreateFeeRequestValidator : AbstractValidator<CreateFeeRequest>
    {
        public CreateFeeRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.Min);
            RuleFor(x => x.Max);
            RuleFor(x => x.ParticipationFee);
            RuleFor(x => x.DepositFee);
            RuleFor(x => x.Surcharge);
            //RuleFor(x => x.Status).NotEmpty().NotNull();
        }
    }
}
