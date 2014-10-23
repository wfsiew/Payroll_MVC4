using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

using NHibernate.Validator.Constraints;

using NHibernate;
using Domain.Validator;
using FluentValidation.Results;

namespace Domain.Model
{
    public class User
    {
        public const string UNCHANGED_PASSWORD = "********";
        public const int ADMIN = 1;
        public const int NORMAL_USER = 2;

        public User()
        {
            Validator = new UserValidator();
        }

        public virtual Guid Id { get; set; }
        [NotNullNotEmpty]
        public virtual int Role { get; set; }
        [NotNullNotEmpty]
        public virtual string Username { get; set; }
        [NotNullNotEmpty]
        public virtual bool Status { get; set; }
        [NotNullNotEmpty]
        public virtual string Password { get; set; }
        public virtual Employee Employee { get; set; }

        public virtual string PasswordConfirmation { get; set; }

        public virtual UserValidator Validator { get; set; }

        public virtual string GetRoleDisplay()
        {
            return Role == 1 ? "Admin" : "Normal User";
        }

        public virtual string GetStatusDisplay()
        {
            return Status ? "Enabled" : "Disabled";
        }

        public virtual void EncryptPassword()
        {
            if (string.IsNullOrEmpty(Password))
                return;

            if (Password != UNCHANGED_PASSWORD)
                Password = GetSHA1HashData(Password);
        }

        public virtual bool IsPasswordRequired()
        {
            if (Password == UNCHANGED_PASSWORD)
                return false;

            return true;
        }

        public virtual bool IsAuthenticated(string password)
        {
            return Password == GetSHA1HashData(password);
        }

        public virtual bool IsEnabled()
        {
            return Status == true;
        }

        public virtual Dictionary<string, object> IsValid(ISession se)
        {
            Validator.Id = Id;
            Validator.Session = se;

            ValidationResult r = Validator.Validate(this);

            return ValidationHelper.GetErrors(r);
        }

        public static User Authenticate(ISession se, string username, string password)
        {
            User user = se.QueryOver<User>()
                .Where(x => x.Username == username)
                .Skip(0)
                .Take(1)
                .SingleOrDefault();

            if (user != null && user.IsAuthenticated(password) && user.IsEnabled())
                return user;

            return null;
        }

        private string GetSHA1HashData(string data)
        {
            //create new instance of md5
            SHA1 sha1 = SHA1.Create();

            //convert the input text to array of bytes
            byte[] hashData = sha1.ComputeHash(Encoding.UTF8.GetBytes(data));

            //create new instance of StringBuilder to save hashed data
            StringBuilder returnValue = new StringBuilder();

            //loop for each byte and add it to StringBuilder
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(string.Format("{0:x2}", hashData[i]));
            }

            // return hexadecimal string
            return returnValue.ToString();
        }
    }
}
