using System;
using System.Collections.Generic;
using R.Component;

namespace R.Config
{
    public class CustomDefinition : IOperationDefinition
    {
        public EndPointUri Uri { get; set; }
        public string Database { get; set; }
        public Type ComponentType { get; set; }
        public IComponent Component { get; set; }
        public R.Public.Op Operation { get; set; }
        public Type ExposedType
        {
            get { return null; }
        }
    }
}
