using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Web.Mvc;

using NHibernate.Validator.Constraints;

using NHibernate;
using NHibernate.Criterion;

namespace Domain.Model
{
    public class User
    {
        public const string UNCHANGED_PASSWORD = "********";

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

            Password = GetSHA1HashData(Password);
        }

        public virtual void SetProperties(FormCollection fc)
        {
            string paramStatus = fc.Get("status");
            bool status = paramStatus == "1" ? true : false;

            string paramRole = fc.Get("role");
            int role = Convert.ToInt32(paramRole);

            string paramUsername = fc.Get("username");
            string paramPwd = fc.Get("pwd");
            string paramPwdConfirm = fc.Get("pwdconfirm");

            Role = role;
            Username = paramUsername;
            Status = status;
            Password = paramPwd;
            PasswordConfirmation = paramPwdConfirm;
        }

        public virtual Dictionary<string, object> IsValid(ISession se)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            Dictionary<string, List<string>> m = new Dictionary<string, List<string>>();

            if (string.IsNullOrEmpty(Username))
            {
                List<string> l = GetErrorList("username", m);
                l.Add("Username is required");
                m["username"] = l;
            }

            else
            {
                ICriteria cr = se.CreateCriteria<User>();
                cr.Add(Restrictions.Eq("Username", Username));
                cr.SetFirstResult(0);
                cr.SetMaxResults(1);
                User k = cr.List<User>().FirstOrDefault();

                if (k != null)
                {
                    if (k.Id != Id)
                    {
                        List<string> l = GetErrorList("username", m);
                        l.Add(string.Format("Username {0} already exist", Username));
                        m["username"] = l;
                    }
                }

                if (Username.Length < 3 || Username.Length > 50)
                {
                    List<string> l = GetErrorList("username", m);
                    l.Add("Minimum is 3 characters");
                    m["username"] = l;
                }
            }

            if (IsPasswordRequired())
            {
                if (string.IsNullOrEmpty(Password))
                {
                    List<string> l = GetErrorList("pwd", m);
                    l.Add("Password is required");
                    m["pwd"] = l;
                }

                else
                {
                    if (Password.Length < 4 || Password.Length > 20)
                    {
                        List<string> l = GetErrorList("pwd", m);
                        l.Add("Minimum is 4 characters");
                        m["pwd"] = l;
                    }

                    else
                    {
                        if (Password != PasswordConfirmation)
                        {
                            List<string> l = GetErrorList("pwd", m);
                            l.Add("Password doesn't match confirmation");
                            m["pwd"] = l;
                        }
                    }
                }
            }

            if (m.Keys.Count > 0)
            {
                dic.Add("error", 1);
                dic.Add("errors", m);
            }

            return dic;
        }

        private bool IsPasswordRequired()
        {
            if (Password == UNCHANGED_PASSWORD)
                return false;

            return true;
        }

        private List<string> GetErrorList(string key, Dictionary<string, List<string>> dic)
        {
            if (!dic.ContainsKey(key))
                return new List<string>();

            return dic[key];
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
