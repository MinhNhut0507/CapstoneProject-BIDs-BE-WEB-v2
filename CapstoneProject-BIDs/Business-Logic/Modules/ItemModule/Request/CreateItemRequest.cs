using FluentValidation;
using System;

namespace Business_Logic.Modules.ItemModule.Request
{
    public class CreateItemRequest
    {
        public Guid UserId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public int Quantity { get; set; }
        public bool Deposit { get; set; }
        public int AuctionTime { get; set; }
        public double FirstPrice { get; set; }
        public double StepPrice { get; set; }
        public int TypeOfSession { get; set; }
    }
    public class CreateItemRequestValidator : AbstractValidator<CreateItemRequest>
    {
        public CreateItemRequestValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().NotNull();
            RuleFor(x => x.ItemName).NotEmpty().NotNull();
            RuleFor(x => x.Description).NotEmpty().NotNull();
            RuleFor(x => x.CategoryId).NotEmpty().NotNull();
            RuleFor(x => x.Quantity).NotEmpty().NotNull();
            RuleFor(x => x.AuctionTime).NotEmpty().NotNull();
            RuleFor(x => x.Deposit).NotNull();
            RuleFor(x => x.FirstPrice).NotEmpty().NotNull();
            RuleFor(x => x.StepPrice).NotEmpty().NotNull();
            RuleFor(x => x.TypeOfSession).NotEmpty().NotNull();
        }
    }
}
