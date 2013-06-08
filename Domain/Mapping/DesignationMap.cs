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
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Title).Column("title").Not.Nullable();
            Map(x => x.Description).Column("description");
            Map(x => x.Note).Column("note");
            HasMany(x => x.Employeejob).KeyColumn("designation_id");
        }
    }
}
