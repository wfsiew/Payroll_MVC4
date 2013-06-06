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
            Id(o => o.Id);
            Map(o => o.Role).Column("role").Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.Username).Column("username").Unique().Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.Status).Column("status").Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.Password).Column("password").Access.Property().Generated.Never().Not.Nullable();
        }
    }
}
