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
    public class Employeecontact
    {
        public Employeecontact()
        {
            Validator = new EmployeecontactValidator();
        }

        public virtual Guid Id { get; set; }
        [NotNullNotEmpty]
        public virtual string Address1 { get; set; }
        public virtual string Address2 { get; set; }
        public virtual string Address3 { get; set; }
        [NotNullNotEmpty]
        public virtual string City { get; set; }
        [NotNullNotEmpty]
        public virtual string State { get; set; }
        [NotNullNotEmpty]
        public virtual string Postcode { get; set; }
        [NotNullNotEmpty]
        public virtual string Country { get; set; }
        public virtual string Homephone { get; set; }
        public virtual string Mobilephone { get; set; }
        [NotNullNotEmpty]
        public virtual string Workemail { get; set; }
        public virtual string Otheremail { get; set; }

        public virtual EmployeecontactValidator Validator { get; set; }

        public virtual Dictionary<string, object> IsValid()
        {
            ValidationResult r = Validator.Validate(this);

            return ValidationHelper.GetErrors(r);
        }
    }
}
