using FluentValidation;
using System;

namespace Business_Logic.Modules.BookingItemModule.Request
{
    public class CreateBookingItemRequest
    {
        public Guid ItemId { get; set; }
    }
    public class CreateBookingItemRequestValidator : AbstractValidator<CreateBookingItemRequest>
    {
        public CreateBookingItemRequestValidator()
        {
            RuleFor(x => x.ItemId).NotEmpty().NotNull();
        }
    }
}
