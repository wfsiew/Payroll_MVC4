using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class EmployeeContact
    {
        public virtual Guid Id { get; protected set; }
        public virtual string Address1 { get; set; }
        public virtual string Address2 { get; set; }
        public virtual string Address3 { get; set; }
        public virtual string City { get; set; }
        public virtual string State { get; set; }
        public virtual string PostCode { get; set; }
        public virtual string Country { get; set; }
        public virtual string HomePhone { get; set; }
        public virtual string MobilePhone { get; set; }
        public virtual string WorkEmail { get; set; }
        public virtual string OtherEmail { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
