using FluentValidation;
using System;

namespace Business_Logic.Modules.NotificationModule.Request
{
    public class CreateNotificationRequest
    {
        public DateTime ExpireDate { get; set; }
        public bool? Status { get; set; }
    }
    public class CreateNotificationRequestValidator : AbstractValidator<CreateNotificationRequest>
    {
        public CreateNotificationRequestValidator()
        {
            RuleFor(x => x.ExpireDate).NotEmpty().NotNull();
            RuleFor(x => x.Status).NotEmpty().NotNull();
        }
    }
}
