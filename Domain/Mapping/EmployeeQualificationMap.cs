using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using Domain.Model;

namespace Domain.Mapping
{
    public class EmployeeQualificationMap : ClassMap<EmployeeQualification>
    {
        public EmployeeQualificationMap()
        {
            Table("employee_qualification");
            Id(o => o.Id).GeneratedBy.Foreign("Employee");
            Map(o => o.Level).Column("level").Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.Institute).Column("institute").Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.Major).Column("major").Access.Property().Generated.Never();
            Map(o => o.Year).Column("year").Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.Gpa).Column("gpa").Access.Property().Generated.Never();
            Map(o => o.StartDate).Column("start_date").Access.Property().Generated.Never().Not.Nullable().CustomSqlType("date").CustomType<DateTime>();
            Map(o => o.EndDate).Column("end_date").Access.Property().Generated.Never().Not.Nullable().CustomSqlType("date").CustomType<DateTime>();
            HasOne(o => o.Employee)
                .Constrained().ForeignKey()
                .LazyLoad();
        }
    }
}
