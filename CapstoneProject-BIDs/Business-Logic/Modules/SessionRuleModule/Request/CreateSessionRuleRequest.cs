using FluentValidation;
using System;

namespace Business_Logic.Modules.SessionRuleModule.Request
{
    public class CreateSessionRuleRequest
    {
        public string Name { get; set; }
        public int IncreaseTime { get; set; }
        public SetTime FreeTime { get; set; }
        public SetTime DelayTime { get; set; }
        public SetTime DelayFreeTime { get; set; }
    }
    public class CreateSessionRuleRequestValidator : AbstractValidator<CreateSessionRuleRequest>
    {
        public CreateSessionRuleRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.IncreaseTime).NotEmpty().NotNull();
            RuleFor(x => x.FreeTime).NotEmpty().NotNull();
            RuleFor(x => x.DelayTime).NotEmpty().NotNull();
            RuleFor(x => x.DelayFreeTime).NotEmpty().NotNull();
        }
    }

    public class SetTime
    {
        public int days { get; set; }
        public int hours { get; set; }
        public int minutes { get; set; }
        public int seconds { get; set; }
    }
}
