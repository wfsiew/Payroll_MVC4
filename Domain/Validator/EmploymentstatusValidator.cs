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
    public class EmploymentstatusValidator : AbstractValidator<Employmentstatus>
    {
        public EmploymentstatusValidator()
        {
            RuleFor(o => o.Name).NotEmpty().OverridePropertyName("name")
                .WithName("Name").WithMessage("{PropertyName} is required");
            RuleFor(o => o.Name).Must(UniqueName).OverridePropertyName("name")
                .WithName("Employment Status").WithMessage("{PropertyName} {PropertyValue} already exist");
        }

        public ISession Session { get; set; }
        public int Id { get; set; }

        private bool UniqueName(string field)
        {
            ICriteria cr = Session.CreateCriteria<Employmentstatus>();
            cr.Add(Restrictions.Eq("Name", field));
            cr.SetFirstResult(0);
            cr.SetMaxResults(1);
            Employmentstatus o = cr.List<Employmentstatus>().FirstOrDefault();
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
