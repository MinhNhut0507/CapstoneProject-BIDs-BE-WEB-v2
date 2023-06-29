using FluentValidation;
using System;

namespace Business_Logic.Modules.BanHistoryModule.Request
{
    public class CreateBanHistoryRequest
    {
        public Guid UserId { get; set; }
        public string Reason { get; set; }
    }
    public class CreateBanHistoryRequestValidator : AbstractValidator<CreateBanHistoryRequest>
    {
        public CreateBanHistoryRequestValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().NotNull();
            RuleFor(x => x.Reason).NotEmpty().NotNull();
        }
    }
}
