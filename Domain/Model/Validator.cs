using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NHibernate;
using NHibernate.Criterion;

namespace Domain.Model
{
    public class Validator
    {
        private Dictionary<string, List<string>> errors;

        public Dictionary<string, List<string>> Errors
        {
            get
            {
                if (errors == null)
                    errors = new Dictionary<string, List<string>>();

                return errors;
            }
        }

        public List<string> GetErrorList(string key)
        {
            if (!Errors.ContainsKey(key))
                return new List<string>();

            return Errors[key];
        }

        public void ValidatesPresenceOf(string field, string key, string message)
        {
            if (string.IsNullOrEmpty(field))
            {
                SetError(key, message);
            }
        }

        public void ValidatesUniquenessOf<T>(ISession se, string field, string propertyName, string key, string message, Action<T, string, string> act) where T : class
        {
            if (string.IsNullOrEmpty(field))
                return;

            ICriteria cr = se.CreateCriteria<T>();
            cr.Add(Restrictions.Eq(propertyName, field));
            cr.SetFirstResult(0);
            cr.SetMaxResults(1);
            T o = cr.List<T>().FirstOrDefault();
            act(o, key, message);
        }

        public void ValidatesLength(string field, int min, int max, string key, string message)
        {
            if (string.IsNullOrEmpty(field))
                return;

            if (field.Length < min || field.Length > max)
            {
                SetError(key, message);
            }
        }

        public void ValidatesMatch(string field, string fieldToMatch, string key, string message)
        {
            if (string.IsNullOrEmpty(field))
                return;

            if (field != fieldToMatch)
            {
                SetError(key, message);
            }
        }

        public void SetError(string key, string message)
        {
            List<string> l = GetErrorList(key);
            l.Add(message);
            Errors[key] = l;
        }

        public bool HasError
        {
            get
            {
                return Errors.Keys.Count > 0;
            }
        }
    }
}
