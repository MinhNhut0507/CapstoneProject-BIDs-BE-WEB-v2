using FluentValidation;
using System;

namespace Business_Logic.Modules.CategoryModule.Request
{
    public class UpdateCategoryRequest
    {
        public Guid CategoryId { get; set; }

        public string CategoryName { get; set; }

        public bool Status { get; set; }
    }
    public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
    {
        public UpdateCategoryRequestValidator()
        {
            RuleFor(x => x.CategoryId).NotEmpty().NotNull();
            RuleFor(x => x.CategoryName).NotEmpty().NotNull();
            RuleFor(x => x.Status).NotEmpty().NotNull();
        }
    }
}
