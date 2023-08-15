using Common.Helper;
using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Business_Logic.Modules.SessionModule.Request
{
    public class CreateBeginSessionRequest
    {
        public string SessionName { get; set; }
        public Guid ItemId { get; set; }
        public Guid SessionRuleId { get; set; }

    }

    public class CreateBeginSessionRequestValidator : AbstractValidator<CreateBeginSessionRequest>
    {
        public CreateBeginSessionRequestValidator()
        {
            RuleFor(x => x.SessionName).NotEmpty().NotNull();
            RuleFor(x => x.ItemId).NotEmpty().NotNull();
            RuleFor(x => x.SessionRuleId).NotEmpty().NotNull();
        }
    }
}
