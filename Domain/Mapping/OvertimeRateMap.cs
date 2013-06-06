using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using Domain.Model;

namespace Domain.Mapping
{
    public class OvertimeRateMap : ClassMap<OvertimeRate>
    {
        public OvertimeRateMap()
        {
            Table("overtime_rate");
            Id(o => o.Id);
            Map(o => o.Duration).Column("duration").Access.Property().Generated.Never();
            Map(o => o.Year).Column("year").Unique().Access.Property().Generated.Never();
            Map(o => o.PayRate).Column("pay_rate").Access.Property().Generated.Never();
        }
    }
}
