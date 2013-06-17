using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NHibernate.Validator.Constraints;

using NHibernate;
using Domain.Validator;
using FluentValidation.Results;

namespace Domain.Model
{
    public class Jobcategory
    {
        public Jobcategory()
        {
			Employeejob = new List<Employeejob>();
            Validator = new JobcategoryValidator();
        }

        public virtual int Id { get; set; }
        [NotNullNotEmpty]
        public virtual string Name { get; set; }
        public virtual IList<Employeejob> Employeejob { get; set; }

        public virtual JobcategoryValidator Validator { get; set; }

        public virtual Dictionary<string, object> IsValid(ISession se)
        {
            Validator.Id = Id;
            Validator.Session = se;

            ValidationResult r = Validator.Validate(this);

            return ValidationHelper.GetErrors(r);
        }
    }
}
