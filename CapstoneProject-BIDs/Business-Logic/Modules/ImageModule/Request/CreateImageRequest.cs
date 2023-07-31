using FluentValidation;
using System;

namespace Business_Logic.Modules.ImageModule.Request
{
    public class CreateImageRequest
    {
        public Guid ItemId { get; set; }
        public string DetailImage { get; set; }
    }
    public class CreateImageRequestValidator : AbstractValidator<CreateImageRequest>
    {
        public CreateImageRequestValidator()
        {
            RuleFor(x => x.ItemId).NotEmpty().NotNull();
            RuleFor(x => x.DetailImage).NotEmpty().NotNull();
        }
    }
}
