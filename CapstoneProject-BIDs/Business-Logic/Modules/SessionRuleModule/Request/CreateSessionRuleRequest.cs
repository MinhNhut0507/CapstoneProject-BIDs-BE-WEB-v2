using FluentValidation;
using System;

namespace Business_Logic.Modules.SessionRuleModule.Request
{
    public class CreateSessionRuleRequest
    {
        public string Name { get; set; }
        public TimeSpan FreeTime { get; set; }
        public TimeSpan DelayTime { get; set; }
        public TimeSpan DelayFreeTime { get; set; }
    }
    public class CreateSessionRuleRequestValidator : AbstractValidator<CreateSessionRuleRequest>
    {
        public CreateSessionRuleRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.FreeTime).NotEmpty().NotNull();
            RuleFor(x => x.DelayTime).NotEmpty().NotNull();
            RuleFor(x => x.DelayFreeTime).NotEmpty().NotNull();
        }
    }
}
