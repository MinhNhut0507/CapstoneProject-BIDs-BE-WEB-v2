using FluentValidation;
using System;

namespace Business_Logic.Modules.UserModule.Request
{
    public class UpdateUserRequest
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }
        public string Avatar { get; set; }

    }
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().NotNull();
            RuleFor(x => x.UserName).NotEmpty().NotNull();
            RuleFor(x => x.Password).NotEmpty().NotNull();
            RuleFor(x => x.Address).NotEmpty().NotNull();
            RuleFor(x => x.Phone).NotEmpty().NotNull();
            RuleFor(x => x.Avatar).NotEmpty().NotNull();
        }
    }
}
