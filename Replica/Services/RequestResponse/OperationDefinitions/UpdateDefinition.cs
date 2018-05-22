using System;
using System.Collections.Generic;

namespace X.Config
{
    public class UpdateDefinition : IOperationDefinition
    {
        public EndPointUri Uri { get; set; }
        public X.Public.Op Operation { get; set; }
        public Type Type { get; set; }
        public string Database { get; set; }
        public X.Public.ResourceViewType ReturnResourceViewType { get; set; }
        public Type ExposedType
        {
            get { return Type; }
        }
    }
}
