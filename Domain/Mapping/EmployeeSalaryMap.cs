using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using Domain.Model;

namespace Domain.Mapping
{
    public class EmployeeSalaryMap : ClassMap<EmployeeSalary>
    {
        public EmployeeSalaryMap()
        {
            Table("employee_salary");
            Id(o => o.Id).GeneratedBy.Foreign("Employee");
            Map(o => o.Salary).Column("salary").Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.Allowance).Column("allowance").Access.Property().Generated.Never();
            Map(o => o.Epf).Column("epf").Access.Property().Generated.Never();
            Map(o => o.Socso).Column("socso").Access.Property().Generated.Never();
            Map(o => o.IncomeTax).Column("income_tax").Access.Property().Generated.Never();
            Map(o => o.BankName).Column("bank_name").Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.BankAccNo).Column("bank_acc_no").Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.BankAccType).Column("bank_acc_type").Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.BankAddress).Column("bank_address").Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.EpfNo).Column("epf_no").Access.Property().Generated.Never().Not.Nullable();
            Map(o => o.SocsoNo).Column("socso_no").Access.Property().Generated.Never();
            Map(o => o.IncomeTaxNo).Column("income_tax_no").Access.Property().Generated.Never();
            Map(o => o.PayType).Column("pay_type").Access.Property().Generated.Never();
            HasOne(o => o.Employee)
                .Constrained().ForeignKey()
                .LazyLoad();
        }
    }
}
