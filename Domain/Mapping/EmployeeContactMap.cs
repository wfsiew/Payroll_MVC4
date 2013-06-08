using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using Domain.Model;

namespace Domain.Mapping
{
    public class EmployeecontactMap : ClassMap<Employeecontact>
    {
        public EmployeecontactMap()
        {
            Table("employee_contact");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
            Map(x => x.Address1).Column("address_1").Not.Nullable();
            Map(x => x.Address2).Column("address_2");
            Map(x => x.Address3).Column("address_3");
            Map(x => x.City).Column("city").Not.Nullable();
            Map(x => x.State).Column("state").Not.Nullable();
            Map(x => x.Postcode).Column("postcode").Not.Nullable();
            Map(x => x.Country).Column("country").Not.Nullable();
            Map(x => x.Homephone).Column("home_phone");
            Map(x => x.Mobilephone).Column("mobile_phone");
            Map(x => x.Workemail).Column("work_email").Not.Nullable();
            Map(x => x.Otheremail).Column("other_email");
        }
    }
}
