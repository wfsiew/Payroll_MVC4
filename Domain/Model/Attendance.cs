using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Attendance
    {
        public virtual Guid Id { get; protected set; }
        public virtual string StaffId { get; set; }
        public virtual DateTime WorkDate { get; set; }
        public virtual DateTime TimeIn { get; set; }
        public virtual DateTime TimeOut { get; set; }
    }
}
