using FluentValidation;
using System;

namespace Business_Logic.Modules.ItemModule.Request
{
    public class UpdateItemRequest
    {
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public int Quantity { get; set; }
        public bool Deposit { get; set; }
        public double FirstPrice { get; set; }
        public double StepPrice { get; set; }
    }
    public class UpdateItemRequestValidator : AbstractValidator<UpdateItemRequest>
    {
        public UpdateItemRequestValidator()
        {
            RuleFor(x => x.ItemId).NotEmpty().NotNull();
            RuleFor(x => x.ItemName).NotEmpty().NotNull();
            RuleFor(x => x.Description).NotEmpty().NotNull();
            RuleFor(x => x.CategoryId).NotEmpty().NotNull();
            RuleFor(x => x.Deposit).NotNull();
            RuleFor(x => x.Quantity).NotEmpty().NotNull();
            RuleFor(x => x.FirstPrice).NotEmpty().NotNull();
            RuleFor(x => x.StepPrice).NotEmpty().NotNull();
        }
    }
}
