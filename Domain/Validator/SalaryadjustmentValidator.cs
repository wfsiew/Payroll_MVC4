using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using Domain.Model;

namespace Domain.Validator
{
    public class SalaryadjustmentValidator : AbstractValidator<Salaryadjustment>
    {
        public SalaryadjustmentValidator()
        {
            RuleFor(o => o.Staffid).NotEmpty().OverridePropertyName("staff_id")
                .WithName("Staff ID").WithMessage("{PropertyName} is required");
            RuleFor(o => o.Inc).NotNull().OverridePropertyName("inc")
                .WithName("Increment").WithMessage("{PropertyName} is required");
            RuleFor(o => o.Month).NotNull().OverridePropertyName("month")
                .WithName("Month").WithMessage("{PropertyName} is required");
            RuleFor(o => o.Year).NotNull().OverridePropertyName("year")
                .WithName("Year").WithMessage("{PropertyName} is required");

            RuleFor(o => o.Inc).GreaterThan(0).OverridePropertyName("inc")
                .WithMessage("Increment is invalid");
            RuleFor(o => o.Month).GreaterThan(0).OverridePropertyName("month")
                .WithMessage("Month is invalid");
            RuleFor(o => o.Month).LessThanOrEqualTo(12).OverridePropertyName("month")
                .WithMessage("Month is invalid");
            RuleFor(o => o.Year).GreaterThan(0).OverridePropertyName("year")
                .WithMessage("Year is invalid");
        }
    }
}
