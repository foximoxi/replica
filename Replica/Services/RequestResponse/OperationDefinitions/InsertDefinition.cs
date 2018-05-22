using System;
using System.Collections.Generic;

namespace X.Config
{
    public class InsertDefinition : IOperationDefinition
    {
        public EndPointUri Uri { get; set; }
        public X.Public.Op Operation { get; set; }
        public Type Type { get; set; }
        public string Database { get; set; }
        X.Public.ResourceViewType resViewType;
        public X.Public.ResourceViewType ReturnResourceViewType
        {
            get
            {
                return resViewType;
            }
            set
            {
                resViewType = value;
            }
        }
        public Type ExposedType
        {
            get { return Type; }
        }
    }
}
