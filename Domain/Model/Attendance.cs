using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NHibernate.Validator.Constraints;

namespace Domain.Model
{
    public class Attendance
    {
        public Attendance()
        {
            Employee = new List<Employee>();
        }

        public virtual Guid Id { get; set; }
        [NotNullNotEmpty]
        public virtual string Staffid { get; set; }
        public virtual DateTime? Workdate { get; set; }
        public virtual DateTime? Timein { get; set; }
        public virtual DateTime? Timeout { get; set; }
        public virtual IList<Employee> Employee { get; set; }
    }
}
