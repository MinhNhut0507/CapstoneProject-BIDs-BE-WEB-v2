using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Business_Logic.Modules.LoginModule.Request
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().NotNull();
            RuleFor(x => x.Password).NotEmpty().NotNull();
        }
    }
}
