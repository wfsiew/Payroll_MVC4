using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using Domain.Model;

namespace Domain.Validator
{
    public class EmployeecontactValidator : AbstractValidator<Employeecontact>
    {
        public EmployeecontactValidator()
        {
            RuleFor(o => o.Address1).NotEmpty().OverridePropertyName("address_1")
                .WithMessage("Address 1 is required");
            RuleFor(o => o.City).NotEmpty().OverridePropertyName("city")
                .WithMessage("City is required");
            RuleFor(o => o.State).NotEmpty().OverridePropertyName("state")
                .WithMessage("State is required");
            RuleFor(o => o.Postcode).NotEmpty().OverridePropertyName("postcode")
                .WithMessage("Postal Code is required");
            RuleFor(o => o.Country).NotEmpty().OverridePropertyName("country")
                .WithMessage("Country is required");
            RuleFor(o => o.Workemail).NotEmpty().OverridePropertyName("work_email")
                .WithMessage("Work Email is required");
        }
    }
}
