using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NHibernate.Validator.Constraints;

namespace Domain.Model
{
    public class Employeecontact
    {
        public virtual Guid Id { get; set; }
        [NotNullNotEmpty]
        public virtual string Address1 { get; set; }
        public virtual string Address2 { get; set; }
        public virtual string Address3 { get; set; }
        [NotNullNotEmpty]
        public virtual string City { get; set; }
        [NotNullNotEmpty]
        public virtual string State { get; set; }
        [NotNullNotEmpty]
        public virtual string Postcode { get; set; }
        [NotNullNotEmpty]
        public virtual string Country { get; set; }
        public virtual string Homephone { get; set; }
        public virtual string Mobilephone { get; set; }
        [NotNullNotEmpty]
        public virtual string Workemail { get; set; }
        public virtual string Otheremail { get; set; }
    }
}
