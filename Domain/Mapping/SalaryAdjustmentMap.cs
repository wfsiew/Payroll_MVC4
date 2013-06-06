using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using Domain.Model;

namespace Domain.Mapping
{
    public class SalaryAdjustmentMap : ClassMap<SalaryAdjustment>
    {
        public SalaryAdjustmentMap()
        {
            Table("salary_adjustment");
            Id(o => o.Id);
            Map(o => o.StaffId).Column("staff_id").Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.Inc).Column("inc").Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.Month).Column("month").Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.Year).Column("year").Access.Property().Generated.Never().Not.Nullable();
        }
    }
}
