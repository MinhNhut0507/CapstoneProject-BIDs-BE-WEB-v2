using FluentValidation;
using System;

namespace Business_Logic.Modules.ItemDescriptionModule.Request
{
    public class CreateItemDescriptionRequest
    {
        public Guid ItemId { get; set; }
        public Guid DescriptionId { get; set; }
        public string Detail { get; set; }
    }
    public class CreateItemDescriptionRequestValidator : AbstractValidator<CreateItemDescriptionRequest>
    {
        public CreateItemDescriptionRequestValidator()
        {
            RuleFor(x => x.ItemId).NotEmpty().NotNull();
            RuleFor(x => x.DescriptionId).NotEmpty().NotNull();
            RuleFor(x => x.Detail).NotEmpty().NotNull();
        }
    }
}
