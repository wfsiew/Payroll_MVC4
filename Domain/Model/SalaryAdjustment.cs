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
    public class Salaryadjustment
    {
        public Salaryadjustment()
        {
            Validator = new SalaryadjustmentValidator();
        }

        public virtual Guid Id { get; set; }
        [NotNullNotEmpty]
        public virtual string Staffid { get; set; }
        [NotNullNotEmpty]
        public virtual double Inc { get; set; }
        [NotNullNotEmpty]
        public virtual int Month { get; set; }
        [NotNullNotEmpty]
        public virtual int Year { get; set; }

        public virtual SalaryadjustmentValidator Validator { get; set; }

        public virtual Dictionary<string, object> IsValid()
        {
            ValidationResult r = Validator.Validate(this);

            return ValidationHelper.GetErrors(r);
        }
    }
}
