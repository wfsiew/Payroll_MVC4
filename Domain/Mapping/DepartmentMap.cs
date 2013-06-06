using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using Domain.Model;

namespace Domain.Mapping
{
    public class DepartmentMap : ClassMap<Department>
    {
        public DepartmentMap()
        {
            Table("department");
            Id(o => o.Id);
            Map(o => o.Name).Column("name").Unique().Access.Property().Generated.Never().Not.Nullable();
        }
    }
}
