using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Business_Logic.Modules.SessionModule.Request
{
    public class UpdateSessionRequest
    {
        public Guid? SessionID { get; set; }
        [Required]
        public int FeeId { get; set; }
        public string SessionName { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime AuctionTime { get; set; }
        public DateTime EndTime { get; set; }
        public double? FinailPrice { get; set; }
        public int Status { get; set; }
    }
    public class UpdateSessionRequestValidator : AbstractValidator<UpdateSessionRequest>
    {
        public UpdateSessionRequestValidator()
        {
            RuleFor(x => x.SessionID).NotEmpty().NotNull();
            RuleFor(x => x.FeeId).NotEmpty().NotNull();
            RuleFor(x => x.SessionName).NotEmpty().NotNull();
            RuleFor(x => x.BeginTime).NotEmpty().NotNull();
            RuleFor(x => x.AuctionTime).NotEmpty().NotNull();
            RuleFor(x => x.EndTime).NotEmpty().NotNull();
            RuleFor(x => x.FinailPrice).NotEmpty().NotNull();
            RuleFor(x => x.Status).NotEmpty().NotNull();
        }
    }
}
