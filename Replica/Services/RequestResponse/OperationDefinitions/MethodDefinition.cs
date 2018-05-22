using System;
using System.Collections.Generic;
using System.Reflection;

namespace X.Config
{
    public class MethodDefinition : IOperationDefinition
    {
        public EndPointUri Uri { get; set; }
        public X.Public.Op Operation { get; set; }
        public Type ComponentType { get; set; }
        public MethodInfo MethodInfo { get; set; }
        public Type ExposedType
        {
            get { return null; }
        }
        public string Database { get; set; }
    }
}
