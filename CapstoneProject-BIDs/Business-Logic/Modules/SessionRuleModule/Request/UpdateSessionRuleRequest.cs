using FluentValidation;
using System;

namespace Business_Logic.Modules.SessionRuleModule.Request
{
    public class UpdateSessionRuleRequest
    {
        public Guid SessionRuleId { get; set; }
        public string Name { get; set; }
        public TimeDTO FreeTime { get; set; }
        public TimeDTO DelayTime { get; set; }
        public TimeDTO DelayFreeTime { get; set; }
        public bool Status { get; set; }
    }
    public class UpdateSessionRuleRequestValidator : AbstractValidator<UpdateSessionRuleRequest>
    {
        public UpdateSessionRuleRequestValidator()
        {
            RuleFor(x => x.SessionRuleId).NotEmpty().NotNull();
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.FreeTime);
            RuleFor(x => x.DelayTime);
            RuleFor(x => x.DelayFreeTime);
            RuleFor(x => x.Status);
        }
    }
}
