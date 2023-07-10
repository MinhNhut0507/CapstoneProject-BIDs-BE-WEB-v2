using FluentValidation;
using System;

namespace Business_Logic.Modules.UserNotificationDetailModule.Request
{
    public class CreateUserNotificationDetailRequest
    {
        public Guid NotificationId { get; set; }
        public int TypeId { get; set; }
        public Guid UserId { get; set; }
        public string Messages { get; set; }
    }
    public class CreateUserNotificationDetailRequestValidator : AbstractValidator<CreateUserNotificationDetailRequest>
    {
        public CreateUserNotificationDetailRequestValidator()
        {
            RuleFor(x => x.NotificationId).NotEmpty().NotNull();
            RuleFor(x => x.TypeId).NotEmpty().NotNull();
            RuleFor(x => x.UserId).NotEmpty().NotNull();
            RuleFor(x => x.Messages).NotEmpty().NotNull();
        }
    }
}
