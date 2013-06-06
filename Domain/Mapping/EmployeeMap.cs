using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using Domain.Model;

namespace Domain.Mapping
{
    public class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            Table("employee");
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.StaffId).Column("staff_id").Unique().Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.FirstName).Column("first_name").Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.MiddleName).Column("middle_name").Access.Property().Generated.Never();
            Map(o => o.LastName).Column("last_name").Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.NewIC).Column("new_ic").Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.OldIC).Column("old_ic").Access.Property().Generated.Never();
            Map(o => o.PassportNo).Column("passport_no").Access.Property().Generated.Never();
            Map(o => o.Gender).Column("gender").Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.MaritalStatus).Column("marital_status").Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.Nationality).Column("nationality").Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.Dob).Column("dob").Access.Property().Generated.Never().Not.Nullable().CustomSqlType("date").CustomType<DateTime>();
            Map(o => o.PlaceOfBirth).Column("place_of_birth").Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.Race).Column("race").Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.Religion).Column("religion").Access.Property().Generated.Never();
            Map(o => o.IsBumi).Column("is_bumi").Access.Property().Generated.Never();
            References(o => o.User).Column("user_id");
        }
    }
}
