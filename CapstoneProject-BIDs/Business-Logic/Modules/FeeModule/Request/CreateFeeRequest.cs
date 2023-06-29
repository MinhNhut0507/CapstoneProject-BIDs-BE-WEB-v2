using FluentValidation;
using System;

namespace Business_Logic.Modules.FeeModule.Request
{
    public class CreateFeeRequest
    {
        public string Name { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public int ParticipationFee { get; set; }
        public int DepositFee { get; set; }
        public double Surcharge { get; set; }
    }
    public class CreateFeeRequestValidator : AbstractValidator<CreateFeeRequest>
    {
        public CreateFeeRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.Min).NotEmpty().NotNull();
            RuleFor(x => x.Max).NotEmpty().NotNull();
            RuleFor(x => x.ParticipationFee).NotEmpty().NotNull();
            RuleFor(x => x.DepositFee).NotEmpty().NotNull();
            RuleFor(x => x.Surcharge).NotEmpty().NotNull();
            //RuleFor(x => x.Status).NotEmpty().NotNull();
        }
    }
}
