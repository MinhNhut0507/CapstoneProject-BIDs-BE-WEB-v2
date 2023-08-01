using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Business_Logic.Modules.SessionModule.Request
{
    public class UpdateSessionStatusRequest
    {
        public Guid SessionID { get; set; }
    }
    public class UpdateSessionStatusRequestValidator : AbstractValidator<UpdateSessionStatusRequest>
    {
        public UpdateSessionStatusRequestValidator()
        {
            RuleFor(x => x.SessionID).NotEmpty().NotNull();
        }
    }
}
