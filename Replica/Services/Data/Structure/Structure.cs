using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace R.Config
{
    public class Structure:IStructure
    {
        private string name;
        [XmlAttribute]
        public string Name {
            get
            {
                return name;
            }
            set
            {
                name = value.ToLower();
            }
        }
        [XmlAttribute]
        public string Database { get; set; }
        [XmlAttribute]
        public string PK { get; set; }

        FieldCollection _fields;
        public FieldCollection Fields
        {
            get { return _fields; }
            set
            {
                _fields = value;
                var pks = _fields.Where(x => x.Marker== FieldMarker.PrimaryKey).FirstOrDefault();
                if (pks != null)
                    this.PK = pks.Name;
            }
        }
        [XmlAttribute]
        public int TestRows { get; set; }
    }
}