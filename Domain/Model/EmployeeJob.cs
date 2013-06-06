using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class EmployeeJob
    {
        public virtual Guid Id { get; protected set; }
        public virtual Designation Designation { get; set; }
        public virtual Department Department { get; set; }
        public virtual EmploymentStatus EmploymentStatus { get; set; }
        public virtual JobCategory JobCategory { get; set; }
        public virtual DateTime JoinDate { get; set; }
        public virtual DateTime ConfirmDate { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
