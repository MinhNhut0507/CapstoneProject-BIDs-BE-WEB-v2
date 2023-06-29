using FluentValidation;
using System;

namespace Business_Logic.Modules.DescriptionModule.Request
{
    public class CreateDescriptionRequest
    {
        public Guid CategoryId { get; set; }
        public string Detail { get; set; }
    }
    public class CreateDescriptionRequestValidator : AbstractValidator<CreateDescriptionRequest>
    {
        public CreateDescriptionRequestValidator()
        {
            RuleFor(x => x.CategoryId).NotEmpty().NotNull();
            RuleFor(x => x.Detail).NotEmpty().NotNull();
        }
    }
}
