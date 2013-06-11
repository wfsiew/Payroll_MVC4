using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using Domain.Model;

namespace Domain.Validator
{
    public class EmployeejobValidator : AbstractValidator<Employeejob>
    {
        public EmployeejobValidator()
        {
            RuleFor(o => o.Designation).NotNull().OverridePropertyName("designation_id")
                .WithMessage("Designation is required");
            RuleFor(o => o.Department).NotNull().OverridePropertyName("department_id")
                .WithMessage("Department is required");
            RuleFor(o => o.Employmentstatus).NotNull().OverridePropertyName("employment_status_id")
                .WithMessage("Employment Status is required");
            RuleFor(o => o.Jobcategory).NotNull().OverridePropertyName("job_category_id")
                .WithMessage("Job Category is required");
            RuleFor(o => o.Joindate).NotEmpty().OverridePropertyName("join_date")
                .WithMessage("Join Date is required");
        }
    }
}
