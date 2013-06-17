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
    public class DepartmentValidator : AbstractValidator<Department>
    {
        public DepartmentValidator()
        {
            RuleFor(o => o.Name).NotEmpty().OverridePropertyName("name")
                .WithName("Name").WithMessage("{PropertyName} is required");
            RuleFor(o => o.Name).Must(UniqueName).OverridePropertyName("name")
                .WithName("Department").WithMessage("{PropertyName} {PropertyValue} already exist");
        }

        public ISession Session { get; set; }
        public int Id { get; set; }

        private bool UniqueName(string field)
        {
            ICriteria cr = Session.CreateCriteria<Department>();
            cr.Add(Restrictions.Eq("Name", field));
            cr.SetFirstResult(0);
            cr.SetMaxResults(1);
            Department o = cr.List<Department>().FirstOrDefault();
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
