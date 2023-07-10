using FluentValidation;
using System;

namespace Business_Logic.Modules.NotificationTypeModule.Request
{
    public class UpdateNotificationTypeRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class UpdateNotificationTypeRequestValidator : AbstractValidator<UpdateNotificationTypeRequest>
    {
        public UpdateNotificationTypeRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
            RuleFor(x => x.Name).NotEmpty().NotNull();
        }
    }
}
