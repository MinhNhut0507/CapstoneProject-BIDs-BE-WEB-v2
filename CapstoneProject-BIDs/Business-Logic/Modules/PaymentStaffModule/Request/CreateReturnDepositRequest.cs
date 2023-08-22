﻿using FluentValidation;
using System;

namespace Business_Logic.Modules.PaymentStaffModule.Request
{
    public class CreateReturnDepositRequest
    {
        public Guid StaffId { get; set; }
        public Guid SessionId { get; set; }
        public string PayPalRecieveAccount { get; set; }
        public string PayPalTransactionId { get; set; }
        public string PaymentDetail { get; set; }
        public double Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Status { get; set; }
    }
    public class CreateReturnDepositRequestValidator : AbstractValidator<CreateReturnDepositRequest>
    {
        public CreateReturnDepositRequestValidator()
        {
            RuleFor(x => x.StaffId).NotEmpty().NotNull();
            RuleFor(x => x.SessionId).NotEmpty().NotNull();
            RuleFor(x => x.PayPalRecieveAccount).NotEmpty().NotNull();
            RuleFor(x => x.PayPalTransactionId).NotEmpty().NotNull();
            RuleFor(x => x.PaymentDetail).NotEmpty().NotNull();
            RuleFor(x => x.Amount).NotEmpty().NotNull();
            RuleFor(x => x.PaymentDate).NotEmpty().NotNull();
            RuleFor(x => x.Status).NotEmpty().NotNull();
        }
    }
}
