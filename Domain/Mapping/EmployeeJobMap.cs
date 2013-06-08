using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using Domain.Model;

namespace Domain.Mapping
{
    public class EmployeejobMap : ClassMap<Employeejob>
    {
        public EmployeejobMap()
        {
            Table("employee_job");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
            References(x => x.Designation).Column("designation_id");
            References(x => x.Department).Column("department_id");
            References(x => x.Employmentstatus).Column("employment_status_id");
            References(x => x.Jobcategory).Column("job_category_id");
            Map(x => x.Joindate).Column("join_date").Not.Nullable();
            Map(x => x.Confirmdate).Column("confirm_date");
        }
    }
}
