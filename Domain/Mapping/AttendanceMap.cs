using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using Domain.Model;

namespace Domain.Mapping
{
    public class AttendanceMap : ClassMap<Attendance>
    {
        public AttendanceMap()
        {
            Table("attendance");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.GuidComb().Column("Id");
            Map(x => x.Staffid).Column("staff_id").Not.Nullable();
            Map(x => x.Workdate).Column("work_date");
            Map(x => x.Timein).Column("time_in");
            Map(x => x.Timeout).Column("time_out");
            HasMany(x => x.Employee).KeyColumn("staff_id").PropertyRef("Staffid");
        }
    }
}
