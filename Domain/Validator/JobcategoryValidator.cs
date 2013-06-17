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
    public class JobcategoryValidator : AbstractValidator<Jobcategory>
    {
        public JobcategoryValidator()
        {
            RuleFor(o => o.Name).NotEmpty().OverridePropertyName("name")
                .WithName("Name").WithMessage("{PropertyName} is required");
            RuleFor(o => o.Name).Must(UniqueName).OverridePropertyName("name")
                .WithName("Category").WithMessage("{PropertyName} {PropertyValue} already exist");
        }

        public ISession Session { get; set; }
        public int Id { get; set; }

        private bool UniqueName(string field)
        {
            ICriteria cr = Session.CreateCriteria<Jobcategory>();
            cr.Add(Restrictions.Eq("Name", field));
            cr.SetFirstResult(0);
            cr.SetMaxResults(1);
            Jobcategory o = cr.List<Jobcategory>().FirstOrDefault();
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
