using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace X.Public
{
    public class ValidationInfo
    {
        string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value?.ToLower();
            }
        }

        public string Message { get; set; }

        public override string ToString()
        {
            if (!String.IsNullOrEmpty(Message) && !String.IsNullOrEmpty(name))
                return String.Format(Message, name);            
            return "";
        }
    }
}