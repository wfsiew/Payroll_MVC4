using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation.Results;

namespace Domain.Validator
{
    public class ValidationHelper
    {
        public static Dictionary<string, object> GetErrors(ValidationResult r)
        {
            Dictionary<string, object> dic = null;
            Dictionary<string, List<string>> m = new Dictionary<string, List<string>>();

            if (!r.IsValid)
            {
                foreach (ValidationFailure e in r.Errors)
                {
                    SetError(e.PropertyName, e.ErrorMessage, m);
                }

                dic = new Dictionary<string, object>
                {
                    { "error", 1 },
                    { "errors", m }
                };
            }

            return dic;
        }

        private static List<string> GetErrorList(string key, Dictionary<string, List<string>> m)
        {
            if (!m.ContainsKey(key))
                return new List<string>();

            return m[key];
        }

        private static void SetError(string key, string message, Dictionary<string, List<string>> m)
        {
            List<string> l = GetErrorList(key, m);
            l.Add(message);
            m[key] = l;
        }
    }
}
