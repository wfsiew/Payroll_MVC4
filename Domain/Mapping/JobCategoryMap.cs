using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using Domain.Model;

namespace Domain.Mapping
{
    public class JobCategoryMap : ClassMap<JobCategory>
    {
        public JobCategoryMap()
        {
            Table("job_category");
            Id(o => o.Id);
            Map(o => o.Name).Column("name").Unique().Access.Property().Generated.Never().Not.Nullable();
        }
    }
}
