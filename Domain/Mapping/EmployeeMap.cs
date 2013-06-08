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
            LazyLoad();
            Id(x => x.Id).GeneratedBy.GuidComb().Column("Id");
            References(x => x.User).Column("user_id");
            Map(x => x.Staffid).Column("staff_id").Not.Nullable();
            Map(x => x.Firstname).Column("first_name").Not.Nullable();
            Map(x => x.Middlename).Column("middle_name");
            Map(x => x.Lastname).Column("last_name").Not.Nullable();
            Map(x => x.Newic).Column("new_ic").Not.Nullable();
            Map(x => x.Oldic).Column("old_ic");
            Map(x => x.Passportno).Column("passport_no");
            Map(x => x.Gender).Column("gender").Not.Nullable();
            Map(x => x.Maritalstatus).Column("marital_status").Not.Nullable();
            Map(x => x.Nationality).Column("nationality").Not.Nullable();
            Map(x => x.Dob).Column("dob").Not.Nullable();
            Map(x => x.Placeofbirth).Column("place_of_birth").Not.Nullable();
            Map(x => x.Race).Column("race").Not.Nullable();
            Map(x => x.Religion).Column("religion");
            Map(x => x.Isbumi).Column("is_bumi");
            HasOne(x => x.Employeecontact);
            HasOne(x => x.Employeejob);
            HasOne(x => x.Employeequalification);
            HasOne(x => x.Employeesalary);
            HasMany(x => x.Attendance).KeyColumn("staff_id").PropertyRef("Staffid");
        }
    }
}
