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
    public class Employeesalary
    {
        public Employeesalary()
        {
            Validator = new EmployeesalaryValidator();
        }

        public virtual Guid Id { get; set; }
        [NotNullNotEmpty]
        public virtual double Salary { get; set; }
        public virtual double Allowance { get; set; }
        public virtual double Epf { get; set; }
        public virtual double Socso { get; set; }
        public virtual double Incometax { get; set; }
        [NotNullNotEmpty]
        public virtual string Bankname { get; set; }
        [NotNullNotEmpty]
        public virtual string Bankaccno { get; set; }
        [NotNullNotEmpty]
        public virtual string Bankacctype { get; set; }
        [NotNullNotEmpty]
        public virtual string Bankaddress { get; set; }
        [NotNullNotEmpty]
        public virtual string Epfno { get; set; }
        public virtual string Socsono { get; set; }
        public virtual string Incometaxno { get; set; }
        public virtual int? Paytype { get; set; }

        public virtual EmployeesalaryValidator Validator { get; set; }

        public virtual Dictionary<string, object> IsValid()
        {
            ValidationResult r = Validator.Validate(this);

            return ValidationHelper.GetErrors(r);
        }
    }
}
