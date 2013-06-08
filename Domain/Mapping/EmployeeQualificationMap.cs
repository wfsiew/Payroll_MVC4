using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using Domain.Model;

namespace Domain.Mapping
{
    public class EmployeequalificationMap : ClassMap<Employeequalification>
    {
        public EmployeequalificationMap()
        {
            Table("employee_qualification");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
            Map(x => x.Level).Column("level").Not.Nullable();
            Map(x => x.Institute).Column("institute").Not.Nullable();
            Map(x => x.Major).Column("major");
            Map(x => x.Year).Column("year").Not.Nullable();
            Map(x => x.Gpa).Column("gpa");
            Map(x => x.Startdate).Column("start_date").Not.Nullable();
            Map(x => x.Enddate).Column("end_date").Not.Nullable();
        }
    }
}
