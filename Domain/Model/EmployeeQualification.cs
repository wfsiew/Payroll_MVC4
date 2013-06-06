using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class EmployeeQualification
    {
        public virtual Guid Id { get; protected set; }
        public virtual int Level { get; set; }
        public virtual string Institute { get; set; }
        public virtual string Major { get; set; }
        public virtual int Year { get; set; }
        public virtual double Gpa { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
