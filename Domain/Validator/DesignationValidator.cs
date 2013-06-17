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
    public class DesignationValidator : AbstractValidator<Designation>
    {
        public DesignationValidator()
        {
            RuleFor(o => o.Title).NotEmpty().OverridePropertyName("title")
                .WithName("Job Title").WithMessage("{PropertyName} is required");
            RuleFor(o => o.Title).Must(UniqueTitle).OverridePropertyName("title")
                .WithName("Job Title").WithMessage("{PropertyName} {PropertyValue} already exist");
        }

        public ISession Session { get; set; }
        public int Id { get; set; }

        private bool UniqueTitle(string field)
        {
            ICriteria cr = Session.CreateCriteria<Designation>();
            cr.Add(Restrictions.Eq("Title", field));
            cr.SetFirstResult(0);
            cr.SetMaxResults(1);
            Designation o = cr.List<Designation>().FirstOrDefault();
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
