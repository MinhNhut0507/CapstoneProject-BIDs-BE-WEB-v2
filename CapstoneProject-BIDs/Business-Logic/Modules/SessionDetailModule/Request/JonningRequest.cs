using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Business_Logic.Modules.SessionDetailModule.Request
{
    public class JonningRequest
    {
        public Guid UserId { get; set; }
        public Guid SessionId { get; set; }

    }

        public class JonningRequestValidator : AbstractValidator<JonningRequest>
    {
        public JonningRequestValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().NotNull();
            RuleFor(x => x.SessionId).NotEmpty().NotNull();
        }
    }
}
