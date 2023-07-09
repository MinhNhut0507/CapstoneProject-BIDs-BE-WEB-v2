using FluentValidation;
using System;

namespace Business_Logic.Modules.AdminModule.Request
{
    public class UpdateAdminRequest
    {
        public Guid AdminId { get; set; }

        public string AdminName { get; set; }


        public string Password { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

    }
    public class UpdateAdminRequestValidator : AbstractValidator<UpdateAdminRequest>
    {
        public UpdateAdminRequestValidator()
        {
            RuleFor(x => x.AdminId).NotEmpty().NotNull();
            RuleFor(x => x.AdminName).NotEmpty().NotNull();
            RuleFor(x => x.Password).NotEmpty().NotNull();
            RuleFor(x => x.Address).NotEmpty().NotNull();
            RuleFor(x => x.Phone).NotEmpty().NotNull();
        }
    }
}
