using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Employee
    {
        public virtual Guid Id { get; protected set; }
        public virtual string StaffId { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string MiddleName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string NewIC { get; set; }
        public virtual string OldIC { get; set; }
        public virtual string PassportNo { get; set; }
        public virtual char Gender { get; set; }
        public virtual char MaritalStatus { get; set; }
        public virtual string Nationality { get; set; }
        public virtual DateTime Dob { get; set; }
        public virtual string PlaceOfBirth { get; set; }
        public virtual string Race { get; set; }
        public virtual string Religion { get; set; }
        public virtual bool IsBumi { get; set; }
        public virtual User User { get; set; }
    }
}
