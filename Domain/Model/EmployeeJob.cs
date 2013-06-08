using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NHibernate.Validator.Constraints;

namespace Domain.Model
{
    public class Employeejob
    {
        public virtual Guid Id { get; set; }
        public virtual Designation Designation { get; set; }
        public virtual Department Department { get; set; }
        public virtual Employmentstatus Employmentstatus { get; set; }
        public virtual Jobcategory Jobcategory { get; set; }
        [NotNullNotEmpty]
        public virtual DateTime Joindate { get; set; }
        public virtual DateTime? Confirmdate { get; set; }
    }
}
