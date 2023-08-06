using FluentValidation;
using System;

namespace Business_Logic.Modules.SessionRuleModule.Request
{
    public class CreateSessionRuleRequest
    {
        public string Name { get; set; }
        public TimeDTO FreeTime { get; set; }
        public TimeDTO DelayTime { get; set; }
        public TimeDTO DelayFreeTime { get; set; }
    }
    public class CreateSessionRuleRequestValidator : AbstractValidator<CreateSessionRuleRequest>
    {
        public CreateSessionRuleRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.FreeTime);
            RuleFor(x => x.DelayTime);
            RuleFor(x => x.DelayFreeTime);
        }
    }

    public class TimeDTO
    {
        public int hour { get; set; }
        public int minute { get; set; }
        public int second { get; set; }
    }
}
