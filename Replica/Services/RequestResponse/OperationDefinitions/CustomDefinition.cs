using System;
using System.Collections.Generic;

namespace X.Config
{
    public class CustomDefinition : IOperationDefinition
    {
        public EndPointUri Uri { get; set; }
        public string Database { get; set; }
        public Type ComponentType { get; set; }
        public IComponent Component { get; set; }
        public X.Public.Op Operation { get; set; }
        public Type ExposedType
        {
            get { return null; }
        }
    }
}
