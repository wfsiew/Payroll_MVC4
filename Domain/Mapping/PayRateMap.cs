using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using Domain.Model;

namespace Domain.Mapping
{
    public class PayrateMap : ClassMap<Payrate>
    {
        public PayrateMap()
        {
            Table("pay_rate");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.GuidComb().Column("Id");
            Map(x => x.Staffid).Column("staff_id").Not.Nullable();
            Map(x => x.Month).Column("month").Not.Nullable();
            Map(x => x.Year).Column("year").Not.Nullable();
            Map(x => x.Hourlypayrate).Column("hourly_pay_rate").Not.Nullable();
        }
    }
}
