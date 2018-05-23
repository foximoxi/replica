using System;
using System.Collections.Generic;
using System.Reflection;

namespace R.Config
{
    public class MethodDefinition : IOperationDefinition
    {
        public EndPointUri Uri { get; set; }
        public R.Public.Op Operation { get; set; }
        public Type ComponentType { get; set; }
        public MethodInfo MethodInfo { get; set; }
        public Type ExposedType
        {
            get { return null; }
        }
        public string Database { get; set; }
    }
}
