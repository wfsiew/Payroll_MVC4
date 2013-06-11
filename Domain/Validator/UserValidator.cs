using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using NHibernate;
using NHibernate.Criterion;
using Domain.Model;

namespace Domain.Validator
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(o => o.Username).NotEmpty().OverridePropertyName("username")
                .WithName("Username").WithMessage("{PropertyName} is required");
            RuleFor(o => o.Username).Must(UniqueUsername).OverridePropertyName("username")
                .WithName("Username").WithMessage("{PropertyName} {PropertyValue} already exist");
            RuleFor(o => o.Username).Length(3, 50).OverridePropertyName("username")
                .WithMessage("Minimum is {MinLength} characters");

            When(o => o.IsPasswordRequired(), () =>
            {
                RuleFor(o => o.Password).NotEmpty().OverridePropertyName("pwd")
                    .WithName("Password").WithMessage("{PropertyName} is required");
                RuleFor(o => o.Password).Length(4, 20).OverridePropertyName("pwd")
                    .WithMessage("Minimum is {MinLength} characters");
                RuleFor(o => o.Password).Equal(o => o.PasswordConfirmation).OverridePropertyName("pwd")
                    .WithMessage("Password doesn't match confirmation");
            });
        }

        public ISession Session { get; set; }
        public Guid Id { get; set; }

        private bool UniqueUsername(string field)
        {
            ICriteria cr = Session.CreateCriteria<User>();
            cr.Add(Restrictions.Eq("Username", field));
            cr.SetFirstResult(0);
            cr.SetMaxResults(1);
            User o = cr.List<User>().FirstOrDefault();
            bool a = true;

            if (o != null)
            {
                if (o.Id != Id)
                    a = false;
            }

            return a;
        }
    }
}
