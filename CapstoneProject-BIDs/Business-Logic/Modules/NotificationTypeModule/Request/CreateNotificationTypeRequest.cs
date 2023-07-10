using FluentValidation;
using System;

namespace Business_Logic.Modules.NotificationTypeModule.Request
{
    public class CreateNotificationTypeRequest
    {
        public string Name { get; set; }
    }
    public class CreateNotificationTypeRequestValidator : AbstractValidator<CreateNotificationTypeRequest>
    {
        public CreateNotificationTypeRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull();
        }
    }
}
