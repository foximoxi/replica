using System;
using System.Collections.Generic;

namespace R.Config
{
    public class DeleteDefinition : IOperationDefinition
    {
        public EndPointUri Uri { get; set; }
        public R.Public.Op Operation { get; set; }
        public Type Type { get; set; }
        public string Database { get; set; }
        public Type ExposedType
        {
            get { return Type; }
        }
    }
}