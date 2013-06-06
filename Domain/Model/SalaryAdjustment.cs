using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class SalaryAdjustment
    {
        public virtual Guid Id { get; protected set; }
        public virtual string StaffId { get; set; }
        public virtual double Inc { get; set; }
        public virtual int Month { get; set; }
        public virtual int Year { get; set; }
    }
}
