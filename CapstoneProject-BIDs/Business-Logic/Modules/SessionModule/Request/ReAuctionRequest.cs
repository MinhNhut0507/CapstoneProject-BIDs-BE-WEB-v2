using Common.Helper;
using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Business_Logic.Modules.SessionModule.Request
{
    public class ReAuctionRequest
    {
        public Guid SessionId { get; set; }
        public Guid ItemId { get; set; }
        public int AuctionTime { get; set; }
        public double FinalPrice { get; set; }

    }

    public class ReAuctionRequestValidator : AbstractValidator<ReAuctionRequest>
    {
        public ReAuctionRequestValidator()
        {
            RuleFor(x => x.SessionId).NotEmpty().NotNull();
            RuleFor(x => x.ItemId).NotEmpty().NotNull();
            RuleFor(x => x.AuctionTime).NotEmpty().NotNull();
            RuleFor(x => x.FinalPrice).NotEmpty().NotNull();
        }
    }

}
