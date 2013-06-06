using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using Domain.Model;

namespace Domain.Mapping
{
    public class PayRateMap : ClassMap<PayRate>
    {
        public PayRateMap()
        {
            Table("pay_rate");
            Id(o => o.Id);
            Map(o => o.StaffId).Column("staff_id").Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.Month).Column("month").Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.Year).Column("year").Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.HourlyPayRate).Column("hourly_pay_rate").Access.Property().Generated.Never().Not.Nullable();
        }
    }
}
