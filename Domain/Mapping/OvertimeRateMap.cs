using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using Domain.Model;

namespace Domain.Mapping
{
    public class OvertimerateMap : ClassMap<Overtimerate>
    {
        public OvertimerateMap()
        {
            Table("overtime_rate");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Duration).Column("duration");
            Map(x => x.Year).Column("year");
            Map(x => x.Payrate).Column("pay_rate");
        }
    }
}
