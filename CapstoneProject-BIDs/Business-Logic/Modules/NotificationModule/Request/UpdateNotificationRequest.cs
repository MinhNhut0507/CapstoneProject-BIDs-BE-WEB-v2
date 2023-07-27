using FluentValidation;
using System;

namespace Business_Logic.Modules.NotificationModule.Request
{
    public class UpdateNotificationRequest
    {
        public Guid Id { get; set; }
        public DateTime ExpireDate { get; set; }
        public bool? Status { get; set; }
    }
    public class UpdateNotificationRequestValidator : AbstractValidator<UpdateNotificationRequest>
    {
        public UpdateNotificationRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
            RuleFor(x => x.ExpireDate).NotEmpty().NotNull();
            RuleFor(x => x.Status);
        }
    }
}
