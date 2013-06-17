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
    public class Designation
    {
        public Designation()
        {
            Employeejob = new List<Employeejob>();
            Validator = new DesignationValidator();
        }

        public virtual int Id { get; set; }
        [NotNullNotEmpty]
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual string Note { get; set; }
        public virtual IList<Employeejob> Employeejob { get; set; }

        public virtual DesignationValidator Validator { get; set; }

        public virtual Dictionary<string, object> IsValid(ISession se)
        {
            Validator.Id = Id;
            Validator.Session = se;

            ValidationResult r = Validator.Validate(this);

            return ValidationHelper.GetErrors(r);
        }
    }
}
