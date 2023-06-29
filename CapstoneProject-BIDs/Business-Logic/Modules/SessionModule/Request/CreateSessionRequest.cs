using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Business_Logic.Modules.SessionModule.Request
{
    public class CreateSessionRequest
    {
        public string SessionName { get; set; }
        public int FeeId { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime AuctionTime { get; set; }
        public DateTime EndTime { get; set; }

    }

        public class CreateSessionRequestValidator : AbstractValidator<CreateSessionRequest>
    {
        public CreateSessionRequestValidator()
        {
            RuleFor(x => x.SessionName).NotEmpty().NotNull();
            RuleFor(x => x.FeeId).NotEmpty().NotNull();
            RuleFor(x => x.BeginTime).NotEmpty().NotNull();
            RuleFor(x => x.AuctionTime).NotEmpty().NotNull();
            RuleFor(x => x.EndTime).NotEmpty().NotNull();
        }
    }
}
