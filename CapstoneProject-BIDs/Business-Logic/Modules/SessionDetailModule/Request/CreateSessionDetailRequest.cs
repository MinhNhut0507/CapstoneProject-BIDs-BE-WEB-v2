using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Business_Logic.Modules.SessionDetailModule.Request
{
    public class CreateSessionDetailRequest
    {
        public Guid UserId { get; set; }
        public Guid SessionId { get; set; }
        public double Price { get; set; }

    }

        public class CreateSessionDetailRequestValidator : AbstractValidator<CreateSessionDetailRequest>
    {
        public CreateSessionDetailRequestValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().NotNull();
            RuleFor(x => x.SessionId).NotEmpty().NotNull();
            RuleFor(x => x.Price).NotEmpty().NotNull();
        }
    }
}
