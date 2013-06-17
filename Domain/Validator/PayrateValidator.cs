using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using Domain.Model;

namespace Domain.Validator
{
    public class PayrateValidator : AbstractValidator<Payrate>
    {
        public PayrateValidator()
        {
            RuleFor(o => o.Staffid).NotEmpty().OverridePropertyName("staff_id")
                .WithName("Staff ID").WithMessage("{PropertyName} is required");
            RuleFor(o => o.Month).NotEmpty().OverridePropertyName("month")
                .WithName("Month").WithMessage("{PropertyName} is required");
            RuleFor(o => o.Year).NotEmpty().OverridePropertyName("year")
                .WithName("Year").WithMessage("{PropertyName} is required");
            RuleFor(o => o.Hourlypayrate).NotEmpty().OverridePropertyName("hourly_pay_rate")
                .WithName("Hourly pay rate").WithMessage("{PropertyName} is required");

            RuleFor(o => o.Month).GreaterThan(0).OverridePropertyName("month")
                .WithMessage("Month is invalid");
            RuleFor(o => o.Month).LessThanOrEqualTo(12).OverridePropertyName("month")
                .WithMessage("Month is invalid");
            RuleFor(o => o.Year).GreaterThan(0).OverridePropertyName("year")
                .WithMessage("Year is invalid");
            RuleFor(o => o.Hourlypayrate).GreaterThan(0).OverridePropertyName("hourly_pay_rate")
                .WithMessage("Hourly pay rate is invalid");
        }
    }
}
