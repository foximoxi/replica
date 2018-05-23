using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace R.Config
{
    public class FieldTestRow
    {
        [XmlAttribute]
        public string Value { get; set; }
    }
}