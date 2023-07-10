using FluentValidation;
using System;

namespace Business_Logic.Modules.StaffNotificationDetailModule.Request
{
    public class UpdateStaffNotificationDetailRequest
    {
        public Guid NotificationId { get; set; }
        public int TypeId { get; set; }
        public Guid StaffId { get; set; }
        public string Messages { get; set; }
    }
    public class UpdateStaffNotificationDetailRequestValidator : AbstractValidator<UpdateStaffNotificationDetailRequest>
    {
        public UpdateStaffNotificationDetailRequestValidator()
        {
            RuleFor(x => x.NotificationId).NotEmpty().NotNull();
            RuleFor(x => x.TypeId).NotEmpty().NotNull();
            RuleFor(x => x.StaffId).NotEmpty().NotNull();
            RuleFor(x => x.Messages).NotEmpty().NotNull();
        }
    }
}
