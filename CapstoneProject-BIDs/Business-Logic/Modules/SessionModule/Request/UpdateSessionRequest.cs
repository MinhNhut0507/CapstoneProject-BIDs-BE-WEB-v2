using Common.Helper;
using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Business_Logic.Modules.SessionModule.Request
{
    public class UpdateSessionRequest
    {
        public Guid? SessionID { get; set; }
        public Guid SessionRuleId { get; set; }
        public string SessionName { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
    }
    public class UpdateSessionRequestValidator : AbstractValidator<UpdateSessionRequest>
    {
        public UpdateSessionRequestValidator()
        {
            RuleFor(x => x.SessionID).NotEmpty().NotNull();
            RuleFor(x => x.SessionRuleId).NotEmpty().NotNull();
            RuleFor(x => x.SessionName).NotEmpty().NotNull();
            RuleFor(x => x.BeginTime).NotEmpty().NotNull();
            RuleFor(x => x.EndTime).NotEmpty().NotNull();
        }
    }
}
