using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NHibernate;
using Domain.Validator;
using FluentValidation.Results;

namespace Domain.Model
{
    public class Overtimerate
    {
        public Overtimerate()
        {
            Validator = new OvertimerateValidator();
        }

        public virtual int Id { get; set; }
        public virtual double Duration { get; set; }
        public virtual int? Year { get; set; }
        public virtual double Payrate { get; set; }

        public virtual OvertimerateValidator Validator { get; set; }

        public virtual Dictionary<string, object> IsValid(ISession se)
        {
            Validator.Id = Id;
            Validator.Session = se;

            ValidationResult r = Validator.Validate(this);

            return ValidationHelper.GetErrors(r);
        }
    }
}
