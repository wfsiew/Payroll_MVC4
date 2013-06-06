using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using Domain.Model;

namespace Domain.Mapping
{
    public class EmployeeContactMap : ClassMap<EmployeeContact>
    {
        public EmployeeContactMap()
        {
            Table("employee_contact");
            Id(o => o.Id).GeneratedBy.Foreign("Employee");
            Map(o => o.Address1).Column("address_1").Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.Address2).Column("address_2").Access.Property().Generated.Never();
            Map(o => o.Address3).Column("address_3").Access.Property().Generated.Never();
            Map(o => o.City).Column("city").Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.State).Column("state").Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.PostCode).Column("postcode").Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.Country).Column("country").Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.HomePhone).Column("home_phone").Access.Property().Generated.Never();
            Map(o => o.MobilePhone).Column("mobile_phone").Access.Property().Generated.Never();
            Map(o => o.WorkEmail).Column("work_email").Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.OtherEmail).Column("other_email").Access.Property().Generated.Never();
            HasOne(o => o.Employee)
                .Constrained().ForeignKey()
                .LazyLoad();
        }
    }
}
