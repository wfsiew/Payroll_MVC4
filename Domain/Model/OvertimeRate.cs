using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Overtimerate
    {
        public virtual int Id { get; set; }
        public virtual double Duration { get; set; }
        public virtual int? Year { get; set; }
        public virtual double Payrate { get; set; }
    }
}
