using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NHibernate.Validator.Constraints;

using Domain.Validator;
using Domain.Helper;
using FluentValidation.Results;

namespace Domain.Model
{
    public class Employeequalification
    {
        public Employeequalification()
        {
            Validator = new EmployeequalificationValidator();
        }

        public virtual Guid Id { get; set; }
        [NotNullNotEmpty]
        public virtual int Level { get; set; }
        [NotNullNotEmpty]
        public virtual string Institute { get; set; }
        public virtual string Major { get; set; }
        [NotNullNotEmpty]
        public virtual int Year { get; set; }
        public virtual double Gpa { get; set; }
        [NotNullNotEmpty]
        public virtual DateTime Startdate { get; set; }
        [NotNullNotEmpty]
        public virtual DateTime Enddate { get; set; }

        public virtual EmployeequalificationValidator Validator { get; set; }

        public virtual Dictionary<string, object> IsValid()
        {
            ValidationResult r = Validator.Validate(this);

            return ValidationHelper.GetErrors(r);
        }
    }
}
