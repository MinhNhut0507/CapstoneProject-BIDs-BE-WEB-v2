using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Business_Logic.Modules.SessionModule.Request
{
    public class UpdateSessionStatusRequest
    {
        [Required]
        public Guid? SessionID { get; set; }
        [Required]
        public int Status { get; set; }
    }
    public class UpdateSessionStatusRequestValidator : AbstractValidator<UpdateSessionStatusRequest>
    {
        public UpdateSessionStatusRequestValidator()
        {
            RuleFor(x => x.SessionID).NotEmpty().NotNull();
            RuleFor(x => x.Status).NotEmpty().NotNull();
        }
    }
}
