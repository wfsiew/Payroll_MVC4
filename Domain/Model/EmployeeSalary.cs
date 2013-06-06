using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class EmployeeSalary
    {
        public virtual Guid Id { get; protected set; }
        public virtual double Salary { get; set; }
        public virtual double Allowance { get; set; }
        public virtual double Epf { get; set; }
        public virtual double Socso { get; set; }
        public virtual double IncomeTax { get; set; }
        public virtual string BankName { get; set; }
        public virtual string BankAccNo { get; set; }
        public virtual string BankAccType { get; set; }
        public virtual string BankAddress { get; set; }
        public virtual string EpfNo { get; set; }
        public virtual string SocsoNo { get; set; }
        public virtual string IncomeTaxNo { get; set; }
        public virtual int PayType { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
