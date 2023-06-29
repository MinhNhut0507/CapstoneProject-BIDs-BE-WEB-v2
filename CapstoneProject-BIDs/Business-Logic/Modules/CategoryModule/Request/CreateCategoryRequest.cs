using FluentValidation;
using System;

namespace Business_Logic.Modules.CategoryModule.Request
{
    public class CreateCategoryRequest
    {
        public string CategoryName { get; set; }
    }
    public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
    {
        public CreateCategoryRequestValidator()
        {
            RuleFor(x => x.CategoryName).NotEmpty().NotNull();
        }
    }
}
