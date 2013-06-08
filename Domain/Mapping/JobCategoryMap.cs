using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using Domain.Model;

namespace Domain.Mapping
{
    public class Jobcategorymap : ClassMap<Jobcategory>
    {
        public Jobcategorymap()
        {
            Table("job_category");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Name).Column("name").Not.Nullable();
            HasMany(x => x.Employeejob).KeyColumn("job_category_id");
        }
    }
}
