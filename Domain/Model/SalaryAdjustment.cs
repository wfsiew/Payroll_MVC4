using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NHibernate.Validator.Constraints;

namespace Domain.Model
{
    public class Salaryadjustment
    {
        public virtual Guid Id { get; set; }
        [NotNullNotEmpty]
        public virtual string Staffid { get; set; }
        [NotNullNotEmpty]
        public virtual double Inc { get; set; }
        [NotNullNotEmpty]
        public virtual int Month { get; set; }
        [NotNullNotEmpty]
        public virtual int Year { get; set; }
    }
}
