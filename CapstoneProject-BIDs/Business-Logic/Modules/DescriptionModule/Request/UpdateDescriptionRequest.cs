using FluentValidation;
using System;

namespace Business_Logic.Modules.DescriptionModule.Request
{
    public class UpdateDescriptionRequest
    {
        public Guid DescriptionId { get; set; }
        public string Detail { get; set; }
        public bool Status { get; set; }
    }
    public class UpdateDescriptionRequestValidator : AbstractValidator<UpdateDescriptionRequest>
    {
        public UpdateDescriptionRequestValidator()
        {
            RuleFor(x => x.DescriptionId).NotEmpty().NotNull();
            RuleFor(x => x.Detail).NotEmpty().NotNull();
            RuleFor(x => x.Status).NotEmpty().NotNull();
        }
    }
}
