using System;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Collections.Generic;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;

namespace R.Config
{
    public class Field
    {
        //Unique field name for collection
        [XmlAttribute]
        public string Name;

        [XmlIgnore]
        public FieldMarker Marker
        {
            get; set;
        }

        [XmlAttribute("Marker")]
        public string MarkerXml
        {
            get { return Marker.ToString().ToUpperInvariant(); }
            set { Marker = (FieldMarker)Enum.Parse(typeof(FieldMarker), value, true); }
        }

        [XmlAttribute]
        public bool CanBeNull
        {
            get; set;
        }

        //optional value for field description like fieldLength for VARCHARS
        [XmlAttribute]
        public string OptionalValue { get; set; }

        [XmlIgnore]
        //[JsonConverter(typeof(StringEnumConverter))]
        public FieldType Type { get; set; }

        //[System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        [XmlAttribute("Type")]
        public string TypeXml
        {
            get { return Type.ToString().ToUpperInvariant(); }
            set { Type = (FieldType)Enum.Parse(typeof(FieldType), value, true); }
        }

        public static FieldType ConvertFromType(Type type)
        {
            if (type == typeof(System.String))
                return FieldType.Text;
            if (type == typeof(System.Int32))
                return FieldType.Int;
            if (type == typeof(System.Int64))
                return FieldType.Long;
            if (type == typeof(System.Decimal))
                return FieldType.Decimal;
            if (type == typeof(System.DateTime))
                return FieldType.DateTime;
            if (type == typeof(Guid))
                return FieldType.Guid;
            return FieldType.Text;
        }


        public static Type ConvertToType(FieldType type)
        {
            switch (type)
            {
                case FieldType.Text:
                    return typeof(System.String);
                case FieldType.Int:
                    return typeof(System.Int32);
                case FieldType.Decimal:
                    return typeof(System.Decimal);
                case FieldType.Long:
                    return typeof(System.Int64);
                case FieldType.DateTime:
                    return typeof(System.DateTime);
                case FieldType.Guid:
                    return typeof(Guid);
            }
            return typeof(System.String);
        }

        public static object ConvertValueToType(FieldType type, string value)
        {
            switch (type)
            {              
                case FieldType.Int:
                    return System.Convert.ToInt32(value);
                case FieldType.Decimal:
                    return System.Convert.ToDecimal(value);
                case FieldType.Long:
                    return System.Convert.ToInt64(value);
                case FieldType.DateTime:
                    return DateTime.Now;
                case FieldType.Guid:
                    return new Guid(value);
                default:
                    return value;
            }
        }

        [XmlAttribute]
        public bool Required { get; set; }

        [XmlAttribute]
        public string DefaultValue { get; set; }

        [XmlAttribute]
        public bool SaveInDb { get; set; } = true;

        public ForeignKeySettings ForeignKeySettings { get; set; }

        [XmlArray("rows")]
        [XmlArrayItem("row")]
        public FieldTestRow[] FieldTestRows { get; set; }
    }
}

