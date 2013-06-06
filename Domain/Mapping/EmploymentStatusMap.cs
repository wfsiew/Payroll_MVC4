using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using Domain.Model;

namespace Domain.Mapping
{
    public class EmploymentStatusMap : ClassMap<EmploymentStatus>
    {
        public EmploymentStatusMap()
        {
            Table("employment_status");
            Id(o => o.Id);
            Map(o => o.Name).Column("name").Unique().Access.Property().Generated.Never().Not.Nullable();
        }
    }
}
