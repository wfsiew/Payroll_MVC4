using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using Domain.Model;

namespace Domain.Mapping
{
    public class DesignationMap : ClassMap<Designation>
    {
        public DesignationMap()
        {
            Table("designation");
            Id(o => o.Id);
            Map(o => o.Title).Column("title").Unique().Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.Desc).Column("description").Access.Property().Generated.Never();
            Map(o => o.Note).Column("note").Access.Property().Generated.Never();
        }
    }
}
