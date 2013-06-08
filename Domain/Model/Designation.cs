using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NHibernate.Validator.Constraints;

namespace Domain.Model
{
    public class Designation
    {
        public Designation()
        {
            Employeejob = new List<Employeejob>();
        }

        public virtual int Id { get; set; }
        [NotNullNotEmpty]
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual string Note { get; set; }
        public virtual IList<Employeejob> Employeejob { get; set; }
    }
}
