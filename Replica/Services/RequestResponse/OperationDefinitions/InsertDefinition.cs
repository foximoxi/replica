using System;
using System.Collections.Generic;

namespace R.Config
{
    public class InsertDefinition : IOperationDefinition
    {
        public EndPointUri Uri { get; set; }
        public R.Public.Op Operation { get; set; }
        public Type Type { get; set; }
        public string Database { get; set; }
        R.Public.ResourceViewType resViewType;
        public R.Public.ResourceViewType ReturnResourceViewType
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
