using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class OvertimeRate
    {
        public virtual int Id { get; protected set; }
        public virtual double Duration { get; set; }
        public virtual int Year { get; set; }
        public virtual double PayRate { get; set; }
    }
}
