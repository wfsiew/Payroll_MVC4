using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NHibernate.Validator.Constraints;

namespace Domain.Model
{
    public class Employeesalary
    {
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
    }
}
