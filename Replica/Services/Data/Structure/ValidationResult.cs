using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace X.Public
{
    public class ValidationResult
    {
        public ICollection<ValidationInfo> ValidationResults { get; set; }
        public bool IsValid { get; set; }
        public object ValidatedObject { get; set; }
        public string GetFormattedResults()
        {
            if (ValidationResults!=null)
                return String.Join(",", ValidationResults.Select(x => x.ToString()));
            return "";
        }
    }
}