using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using Domain.Model;

namespace Domain.Mapping
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("user");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.GuidComb().Column("Id");
            Map(x => x.Role).Column("role").Not.Nullable();
            Map(x => x.Username).Column("username").Not.Nullable();
            Map(x => x.Status).Column("status").Not.Nullable();
            Map(x => x.Password).Column("password").Not.Nullable();
            HasOne(x => x.Employee).PropertyRef("User");
        }
    }
}
