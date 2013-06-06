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
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.StaffId).Column("staff_id").Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.WorkDate).Column("work_date").Access.Property().Generated.Never().CustomSqlType("date").CustomType<DateTime>();
            Map(o => o.TimeIn).Column("time_in").Access.Property().Generated.Never();
            Map(o => o.TimeOut).Column("time_out").Access.Property().Generated.Never();
        }
    }
}
