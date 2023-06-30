using FluentValidation;
using System;

namespace Business_Logic.Modules.BookingItemModule.Request
{
    public class UpdateBookingItemRequest
    {
        public Guid Id { get; set; }
        public int Status { get; set; }
    }
    public class UpdateBookingItemRequestValidator : AbstractValidator<UpdateBookingItemRequest>
    {
        public UpdateBookingItemRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
            RuleFor(x => x.Status).NotEmpty().NotNull();
        }
    }
}
