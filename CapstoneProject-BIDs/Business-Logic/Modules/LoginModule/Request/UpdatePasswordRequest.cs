using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.LoginModule.Request
{
    public class UpdatePasswordRequest
    {
        public Guid Id { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
    public class UpdatePasswordRequestValidator : AbstractValidator<UpdatePasswordRequest>
    {
        public UpdatePasswordRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
            RuleFor(x => x.OldPassword).NotEmpty().NotNull();
            RuleFor(x => x.NewPassword).NotEmpty().NotNull();
        }
    }
}
