using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using Domain.Model;

namespace Domain.Validator
{
    public class EmployeequalificationValidator : AbstractValidator<Employeequalification>
    {
        public EmployeequalificationValidator()
        {
            RuleFor(o => o.Level).NotEmpty().OverridePropertyName("level")
                .WithMessage("Qualification Level is required");
            RuleFor(o => o.Institute).NotEmpty().OverridePropertyName("institute")
                .WithMessage("Institute name is required");
            RuleFor(o => o.Year).NotEmpty().OverridePropertyName("year")
                .WithMessage("Year obtained is required");
            RuleFor(o => o.Startdate).NotEmpty().OverridePropertyName("start_date")
                .WithMessage("Start Date is required");
            RuleFor(o => o.Enddate).NotEmpty().OverridePropertyName("end_date")
                .WithMessage("End Date is required");
        }
    }
}
