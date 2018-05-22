using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace X.Config
{
    public interface IValidator
    {
        X.Public.ValidationResult ValidationResult { get; }
        void RegisterValidationMessage(string message, string propertyName = null);
        void RegisterValidationMessage(X.Public.ValidationInfo info);
    }
}