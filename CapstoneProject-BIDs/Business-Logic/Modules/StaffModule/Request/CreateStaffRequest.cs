using FluentValidation;
using System;

namespace Business_Logic.Modules.StaffModule.Request
{
    public class CreateStaffRequest
    {
        public string AccountName { get; set; }
        public string StaffName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int RoleId { get; set; }
    }
    public class CreateStaffRequestValidator : AbstractValidator<CreateStaffRequest>
    {
        public CreateStaffRequestValidator()
        {
            RuleFor(x => x.RoleId).NotEmpty().NotNull();
            RuleFor(x => x.AccountName).NotEmpty().NotNull();
            RuleFor(x => x.StaffName).NotEmpty().NotNull();
            RuleFor(x => x.Email).NotEmpty().NotNull();
            RuleFor(x => x.Address).NotEmpty().NotNull();
            RuleFor(x => x.Phone).NotEmpty().NotNull();
            RuleFor(x => x.DateOfBirth).NotEmpty().NotNull();
            RuleFor(x => x.Password).NotEmpty().NotNull();
            //RuleFor(x => x.UpdateDate).NotEmpty().NotNull();
            //RuleFor(x => x.CreateDate).NotEmpty().NotNull();
            //RuleFor(x => x.Notification).NotEmpty().NotNull();
            //RuleFor(x => x.Status).NotEmpty().NotNull();
        }
    }
}
