using FluentValidation;
using System;

namespace Business_Logic.Modules.ItemDescriptionModule.Request
{
    public class UpdateItemDescriptionRequest
    {
        public Guid Id { get; set; }
        public string Detail { get; set; }
    }
    public class UpdateItemDescriptionRequestValidator : AbstractValidator<UpdateItemDescriptionRequest>
    {
        public UpdateItemDescriptionRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
            RuleFor(x => x.Detail).NotEmpty().NotNull();
        }
    }
}
