using FluentValidation;
using System;

namespace Business_Logic.Modules.BanHistoryModule.Request
{
    public class UpdateBanHistoryRequest
    {
        public Guid BanHistoryId { get; set; }

        public string Reason { get; set; }

        public bool Status { get; set; }
    }
    public class UpdateBanHistoryRequestValidator : AbstractValidator<UpdateBanHistoryRequest>
    {
        public UpdateBanHistoryRequestValidator()
        {
            RuleFor(x => x.BanHistoryId).NotEmpty().NotNull();
            RuleFor(x => x.Reason).NotEmpty().NotNull();
            RuleFor(x => x.Status).NotEmpty().NotNull();
        }
    }
}
