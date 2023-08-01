using FluentValidation;
using System;

namespace Business_Logic.Modules.NotificationModule.Request
{
    public class CreateNotificationRequest
    {
        public int ExpireDate { get; set; }
    }
    public class CreateNotificationRequestValidator : AbstractValidator<CreateNotificationRequest>
    {
        public CreateNotificationRequestValidator()
        {
            RuleFor(x => x.ExpireDate).NotEmpty().NotNull();
        }
    }
}
