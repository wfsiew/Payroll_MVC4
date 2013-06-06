using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using Domain.Model;

namespace Domain.Mapping
{
    public class EmployeeJobMap : ClassMap<EmployeeJob>
    {
        public EmployeeJobMap()
        {
            Table("employee_job");
            Id(o => o.Id).GeneratedBy.Foreign("Employee");
            References(o => o.Designation).Column("designation_id").Access.Property().Not.Nullable();
            References(o => o.Department).Column("department_id").Access.Property().Not.Nullable();
            References(o => o.EmploymentStatus).Column("employment_status_id").Access.Property().Not.Nullable();
            References(o => o.JobCategory).Column("job_category_id").Access.Property().Not.Nullable();
            Map(o => o.JoinDate).Column("join_date").Access.Property().Not.Nullable().CustomSqlType("date").CustomType<DateTime>();
            Map(o => o.ConfirmDate).Column("confirm_date").Access.Property().CustomSqlType("date").CustomType<DateTime>();
            HasOne(o => o.Employee)
                .Constrained().ForeignKey()
                .LazyLoad();
        }
    }
}
