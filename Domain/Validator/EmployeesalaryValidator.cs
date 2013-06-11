using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using Domain.Model;

namespace Domain.Validator
{
    public class EmployeesalaryValidator : AbstractValidator<Employeesalary>
    {
        public EmployeesalaryValidator()
        {
            RuleFor(o => o.Salary).NotNull().OverridePropertyName("salary")
                .WithMessage("Salary is required");
            RuleFor(o => o.Bankname).NotEmpty().OverridePropertyName("bank_name")
                .WithMessage("Bank Name is required");
            RuleFor(o => o.Bankaccno).NotEmpty().OverridePropertyName("bank_acc_no")
                .WithMessage("Bank Account No. is required");
            RuleFor(o => o.Bankacctype).NotEmpty().OverridePropertyName("bank_acc_type")
                .WithMessage("Bank Account Type is required");
            RuleFor(o => o.Bankaddress).NotEmpty().OverridePropertyName("bank_address")
                .WithMessage("Bank Address is required");
            RuleFor(o => o.Epfno).NotEmpty().OverridePropertyName("epf_no")
                .WithMessage("EPF No. is required");
            RuleFor(o => o.Paytype).NotEmpty().OverridePropertyName("pay_type")
                .WithMessage("Pay Type is required");
            RuleFor(o => o.Epf).NotNull().OverridePropertyName("epf")
                .WithMessage("EPF Deduction is required");

            RuleFor(o => o.Salary).GreaterThanOrEqualTo(0).OverridePropertyName("salary")
                .WithMessage("Salary is invalid");
            RuleFor(o => o.Allowance).GreaterThanOrEqualTo(0).OverridePropertyName("allowance")
                .WithMessage("Allowance is invalid");
            RuleFor(o => o.Epf).GreaterThanOrEqualTo(0).OverridePropertyName("epf")
                .WithMessage("EPF Deduction is invalid");
            RuleFor(o => o.Socso).GreaterThanOrEqualTo(0).OverridePropertyName("socso")
                .WithMessage("SOCSO Deduction is invalid");
            RuleFor(o => o.Incometax).GreaterThanOrEqualTo(0).OverridePropertyName("income_tax")
                .WithMessage("Income Tax Deduction is invalid");
        }
    }
}
