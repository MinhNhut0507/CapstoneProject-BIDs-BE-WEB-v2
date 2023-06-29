using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Business_Logic.Modules.SessionDetailModule.Request
{
    public class UpdateSessionDetailRequest
    {
        public Guid? SessionDetailId { get; set; }
        public bool Status { get; set; }
    }
    public class UpdateSessionDetailRequestValidator : AbstractValidator<UpdateSessionDetailRequest>
    {
        public UpdateSessionDetailRequestValidator()
        {
            RuleFor(x => x.SessionDetailId).NotEmpty().NotNull();
            RuleFor(x => x.Status).NotEmpty().NotNull();
        }
    }
}
