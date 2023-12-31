﻿using FluentValidation;
using System;

namespace Business_Logic.Modules.FeeModule.Request
{
    public class UpdateFeeRequest
    {
        public int FeeId { get; set; }
        public string Name { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public double ParticipationFee { get; set; }
        public double DepositFee { get; set; }
        public double Surcharge { get; set; }
        public bool Status { get; set; }
    }
    public class UpdateFeeRequestValidator : AbstractValidator<UpdateFeeRequest>
    {
        public UpdateFeeRequestValidator()
        {
            RuleFor(x => x.FeeId).NotEmpty().NotNull();
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.Min).NotEmpty().NotNull();
            RuleFor(x => x.Max).NotEmpty().NotNull();
            RuleFor(x => x.ParticipationFee).NotEmpty().NotNull();
            RuleFor(x => x.DepositFee);
            RuleFor(x => x.Surcharge).NotEmpty().NotNull();
            RuleFor(x => x.Status);
        }
    }
}
