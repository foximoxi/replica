using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.Public;

namespace X.Config
{
    public class Validator:IValidator
    {
        protected List<ValidationInfo> validations = new List<ValidationInfo>();
        public void RegisterValidationMessage(string message, string propertyName = null)
        {
            RegisterValidationMessage(new ValidationInfo() { Message = message, Name = propertyName });
        }

        public void RegisterValidationMessage(X.Public.ValidationInfo info)
        {
            validations.Add(info);
        }

        public ValidationInfo ValidateUniqueCaseInsensitive(string[] strings)
        {
            return ValidateUnique(strings.Select(x => x.ToLowerInvariant()).ToArray());
        }

        public ValidationInfo ValidateUnique(string[] strings)
        {
            if (strings.Distinct().Count() != strings.Count())
            {
                List<String> duplicated = strings.GroupBy(x => x)
                             .Where(g => g.Count() > 1)
                             .Select(g => g.Key)
                             .ToList();
                var info = new ValidationInfo() { Message = string.Join(".", duplicated) };
                validations.Add(info);
                return info;
            }
            return null;
        }

        public ValidationResult ValidationResult
        {
            get
            {
                var res = new ValidationResult();
                if (validations.Count > 0)
                {
                    res.IsValid = false;
                    res.ValidationResults = validations.ToArray();
                }
                else
                    res.IsValid = true;
                return res;
            }
        }
    }
}
