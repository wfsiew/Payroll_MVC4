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
    public class Employeejob
    {
        public Employeejob()
        {
            Validator = new EmployeejobValidator();
        }

        public virtual Guid Id { get; set; }
        public virtual Designation Designation { get; set; }
        public virtual Department Department { get; set; }
        public virtual Employmentstatus Employmentstatus { get; set; }
        public virtual Jobcategory Jobcategory { get; set; }
        [NotNullNotEmpty]
        public virtual DateTime Joindate { get; set; }
        public virtual DateTime? Confirmdate { get; set; }

        public virtual EmployeejobValidator Validator { get; set; }

        public virtual Dictionary<string, object> IsValid()
        {
            ValidationResult r = Validator.Validate(this);

            return ValidationHelper.GetErrors(r);
        }
    }
}
