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
    public class OvertimerateValidator : AbstractValidator<Overtimerate>
    {
        public OvertimerateValidator()
        {
            RuleFor(o => o.Duration).NotEmpty().OverridePropertyName("duration")
                .WithName("Duration").WithMessage("{PropertyName} is required");
            RuleFor(o => o.Year).NotEmpty().OverridePropertyName("year")
                .WithName("Year").WithMessage("{PropertyName} is required");
            RuleFor(o => o.Payrate).NotEmpty().OverridePropertyName("pay_rate")
                .WithName("Pay Rate").WithMessage("{PropertyName} is required");

            RuleFor(o => o.Duration).GreaterThanOrEqualTo(0).OverridePropertyName("duration")
                .WithMessage("Duration is invalid");
            RuleFor(o => o.Year).GreaterThan(0).OverridePropertyName("year")
                .WithMessage("Year is invalid");
            RuleFor(o => o.Payrate).GreaterThan(0).OverridePropertyName("pay_rate")
                .WithMessage("Pay Rate is invalid");

            RuleFor(o => o.Year).Must(UniqueYear).OverridePropertyName("year")
                .WithName("Overtime rate for year").WithMessage("{PropertyName} {PropertyValue} already exist");
        }

        public ISession Session { get; set; }
        public int Id { get; set; }

        private bool UniqueYear(int? field)
        {
            ICriteria cr = Session.CreateCriteria<Overtimerate>();
            cr.Add(Restrictions.Eq("Year", field.GetValueOrDefault()));
            cr.SetFirstResult(0);
            cr.SetMaxResults(1);
            Overtimerate o = cr.List<Overtimerate>().FirstOrDefault();
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
