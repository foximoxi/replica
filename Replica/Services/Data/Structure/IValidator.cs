using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace R.Config
{
    public interface IValidator
    {
        R.Public.ValidationResult ValidationResult { get; }
        void RegisterValidationMessage(string message, string propertyName = null);
        void RegisterValidationMessage(R.Public.ValidationInfo info);
    }
}