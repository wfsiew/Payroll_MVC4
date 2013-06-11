using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using NHibernate;
using NHibernate.Criterion;
using Domain.Model;

namespace Domain.Validator
{
    public class EmployeeValidator : AbstractValidator<Employee>
    {
        public EmployeeValidator()
        {
            RuleFor(o => o.Staffid).NotEmpty().OverridePropertyName("staff_id")
                .WithMessage("Employee ID is required");
            RuleFor(o => o.Firstname).NotEmpty().OverridePropertyName("first_name")
                .WithMessage("First Name is required");
            RuleFor(o => o.Lastname).NotEmpty().OverridePropertyName("last_name")
                .WithMessage("Last Name is required");
            RuleFor(o => o.Newic).NotEmpty().OverridePropertyName("new_ic")
                .WithMessage("New IC No. is required");
            RuleFor(o => o.Gender).NotEmpty().OverridePropertyName("gender")
                .WithMessage("Gender is required");
            RuleFor(o => o.Maritalstatus).NotEmpty().OverridePropertyName("marital_status")
                .WithMessage("Marital Status is required");
            RuleFor(o => o.Nationality).NotEmpty().OverridePropertyName("nationality")
                .WithMessage("Nationality is required");
            RuleFor(o => o.Dob).NotEmpty().OverridePropertyName("dob")
                .WithMessage("Date of Birth is required");
            RuleFor(o => o.Placeofbirth).NotEmpty().OverridePropertyName("place_of_birth")
                .WithMessage("Place of Birth is required");
            RuleFor(o => o.Race).NotEmpty().OverridePropertyName("race")
                .WithMessage("Race is required");

            RuleFor(o => o.Staffid).Must(UniqueStaffid).OverridePropertyName("staff_id")
                .WithMessage("Employee ID {PropertyValue} already exist");
        }

        public ISession Session { get; set; }
        public Guid Id { get; set; }

        private bool UniqueStaffid(string field)
        {
            ICriteria cr = Session.CreateCriteria<Employee>();
            cr.Add(Restrictions.Eq("Staffid", field));
            cr.SetFirstResult(0);
            cr.SetMaxResults(1);
            Employee o = cr.List<Employee>().FirstOrDefault();
            bool a = true;

            if (o != null)
            {
                if (o.Id != Id)
                    a = false;
            }

            return a;
        }
    }
}
