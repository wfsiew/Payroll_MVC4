using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using Domain.Model;

namespace Domain.Mapping
{
    public class EmployeesalaryMap : ClassMap<Employeesalary>
    {
        public EmployeesalaryMap()
        {
            Table("employee_salary");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
            Map(x => x.Salary).Column("salary").Not.Nullable();
            Map(x => x.Allowance).Column("allowance");
            Map(x => x.Epf).Column("epf");
            Map(x => x.Socso).Column("socso");
            Map(x => x.Incometax).Column("income_tax");
            Map(x => x.Bankname).Column("bank_name").Not.Nullable();
            Map(x => x.Bankaccno).Column("bank_acc_no").Not.Nullable();
            Map(x => x.Bankacctype).Column("bank_acc_type").Not.Nullable();
            Map(x => x.Bankaddress).Column("bank_address").Not.Nullable();
            Map(x => x.Epfno).Column("epf_no").Not.Nullable();
            Map(x => x.Socsono).Column("socso_no");
            Map(x => x.Incometaxno).Column("income_tax_no");
            Map(x => x.Paytype).Column("pay_type");
        }
    }
}
