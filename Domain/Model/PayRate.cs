using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NHibernate.Validator.Constraints;

using Domain.Validator;
using FluentValidation.Results;

namespace Domain.Model
{
    public class Payrate
    {
        public Payrate()
        {
            Validator = new PayrateValidator();
        }

        public virtual Guid Id { get; set; }
        [NotNullNotEmpty]
        public virtual string Staffid { get; set; }
        [NotNullNotEmpty]
        public virtual int Month { get; set; }
        [NotNullNotEmpty]
        public virtual int Year { get; set; }
        [NotNullNotEmpty]
        public virtual double Hourlypayrate { get; set; }

        public virtual PayrateValidator Validator { get; set; }

        public virtual Dictionary<string, object> IsValid()
        {
            ValidationResult r = Validator.Validate(this);

            return ValidationHelper.GetErrors(r);
        }
    }
}
