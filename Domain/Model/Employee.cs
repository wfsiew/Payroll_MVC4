using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NHibernate.Validator.Constraints;

namespace Domain.Model
{
    public class Employee
    {
        public Employee()
        {
            Attendance = new List<Attendance>();
        }

        public virtual Guid Id { get; set; }
        public virtual User User { get; set; }
        [NotNullNotEmpty]
        public virtual string Staffid { get; set; }
        [NotNullNotEmpty]
        public virtual string Firstname { get; set; }
        public virtual string Middlename { get; set; }
        [NotNullNotEmpty]
        public virtual string Lastname { get; set; }
        [NotNullNotEmpty]
        public virtual string Newic { get; set; }
        public virtual string Oldic { get; set; }
        public virtual string Passportno { get; set; }
        [NotNullNotEmpty]
        public virtual string Gender { get; set; }
        [NotNullNotEmpty]
        public virtual string Maritalstatus { get; set; }
        [NotNullNotEmpty]
        public virtual string Nationality { get; set; }
        [NotNullNotEmpty]
        public virtual DateTime Dob { get; set; }
        [NotNullNotEmpty]
        public virtual string Placeofbirth { get; set; }
        [NotNullNotEmpty]
        public virtual string Race { get; set; }
        public virtual string Religion { get; set; }
        public virtual bool? Isbumi { get; set; }
        public virtual Employeecontact Employeecontact { get; set; }
        public virtual Employeejob Employeejob { get; set; }
        public virtual Employeequalification Employeequalification { get; set; }
        public virtual Employeesalary Employeesalary { get; set; }
        public virtual IList<Attendance> Attendance { get; set; }
    }
}
