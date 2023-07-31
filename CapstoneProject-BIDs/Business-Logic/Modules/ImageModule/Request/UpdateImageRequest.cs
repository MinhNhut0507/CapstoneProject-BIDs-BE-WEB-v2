using FluentValidation;
using System;

namespace Business_Logic.Modules.ImageModule.Request
{
    public class UpdateImageRequest
    {
        public Guid Id { get; set; }
        public string DetailImage { get; set; }
    }
    public class UpdateImageRequestValidator : AbstractValidator<UpdateImageRequest>
    {
        public UpdateImageRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
            RuleFor(x => x.DetailImage).NotEmpty().NotNull();
        }
    }
}
