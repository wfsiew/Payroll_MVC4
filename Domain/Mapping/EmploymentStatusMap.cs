using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using Domain.Model;

namespace Domain.Mapping
{
    public class EmploymentstatusMap : ClassMap<Employmentstatus>
    {
        public EmploymentstatusMap()
        {
            Table("employment_status");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Name).Column("name").Not.Nullable();
            HasMany(x => x.Employeejob).KeyColumn("employment_status_id");
        }
    }
}
