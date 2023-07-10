using FluentValidation;
using System;

namespace Business_Logic.Modules.StaffNotificationDetailModule.Request
{
    public class CreateStaffNotificationDetailRequest
    {
        public Guid NotificationId { get; set; }
        public int TypeId { get; set; }
        public Guid StaffId { get; set; }
        public string Messages { get; set; }
    }
    public class CreateStaffNotificationDetailRequestValidator : AbstractValidator<CreateStaffNotificationDetailRequest>
    {
        public CreateStaffNotificationDetailRequestValidator()
        {
            RuleFor(x => x.NotificationId).NotEmpty().NotNull();
            RuleFor(x => x.TypeId).NotEmpty().NotNull();
            RuleFor(x => x.StaffId).NotEmpty().NotNull();
            RuleFor(x => x.Messages).NotEmpty().NotNull();
        }
    }
}
