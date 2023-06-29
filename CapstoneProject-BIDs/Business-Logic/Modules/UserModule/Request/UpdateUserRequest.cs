using FluentValidation;
using System;

namespace Business_Logic.Modules.UserModule.Request
{
    public class UpdateUserRequest
    {
        public Guid UserId { get; set; }
        //public string AccountName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }
        public string Avatar { get; set; }

        //public DateTime DateOfBirth { get; set; }

        //public string Cccdnumber { get; set; }

        //public byte[] CccdfrontImage { get; set; }

        //public byte[] CccdbackImage { get; set; }

        //public DateTime UpdateDate { get; set; }

        //public DateTime CreateDate { get; set; }

        //public string Notification { get; set; }

        //public bool Status { get; set; }
    }
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().NotNull();
            //RuleFor(x => x.AccountName).NotEmpty().NotNull();
            RuleFor(x => x.UserName).NotEmpty().NotNull();
            RuleFor(x => x.Email).NotEmpty().NotNull();
            RuleFor(x => x.Password).NotEmpty().NotNull();
            RuleFor(x => x.Address).NotEmpty().NotNull();
            RuleFor(x => x.Phone).NotEmpty().NotNull();
            RuleFor(x => x.Avatar).NotEmpty().NotNull();
            //RuleFor(x => x.DateOfBirth).NotEmpty().NotNull();
            //RuleFor(x => x.Cccdnumber).NotEmpty().NotNull();
            //RuleFor(x => x.CccdfrontImage).NotEmpty().NotNull();
            //RuleFor(x => x.CccdbackImage).NotEmpty().NotNull();
            //RuleFor(x => x.UpdateDate).NotEmpty().NotNull();
            //RuleFor(x => x.CreateDate).NotEmpty().NotNull();
            //RuleFor(x => x.Notification).NotEmpty().NotNull();
            //RuleFor(x => x.Status).NotEmpty().NotNull();
        }
    }
}
