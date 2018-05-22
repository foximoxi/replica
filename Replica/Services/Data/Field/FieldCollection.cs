using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace X.Config
{
    public class FieldCollection : ICollection<Field>
    {
        public FieldCollection() { }
        public FieldCollection(IEnumerable<Field> fields)
        {
            foreach (var f in fields)
                _fields[f.Name] = f;
        }

        public bool IsReadOnly
        {
            get { return false; }
        }
        public void CopyTo(Field[] array, int arrayIndex)
        {
        }

        public void Clear()
        {
            _fields.Clear();
        }
        public bool Contains(Field field)
        {
            return _fields.ContainsValue(field);
        }

        public bool Remove(Field field)
        {
            foreach (var f in _fields)
            {
                if (f.Value == field)
                {
                    _fields.Remove(f.Key);
                    return true;
                }
            }
            return false;

        }

        Dictionary<string, Field> _fields = new Dictionary<string, Field>();

        public List<Field> WithDefaultValues { get; private set; }

        public Field Add(string name, FieldType type)
        {
            return Add(name, type, null, null, null);
        }

        public Field this[string fieldName]
        {
            get
            {
                return GetByName(fieldName);
            }
        }

        public Field GetByName(string name)
        {
            if (_fields.ContainsKey(name))
                return _fields[name];
            return null;
        }

        public Field AddLookup(string name, FieldType type, string structureName, string[] visibleFields, string suggestedControl)
        {
            return Add(name, type, visibleFields, structureName, suggestedControl);
        }

        public void Add(Field field)
        {
            //Add(field.Name, field.Type, null, null, field.DefaultValue, field.Required);
            _fields.Add(field.Name,field);
        }

        public Field Add(string name, FieldType type, object values, string parentStructureName, object defaultValue, bool required = false)
        {
            Field f = null;
            switch (type)
            {
                /*case FieldType.Xml:
                    {
                        var x1 = new FieldXml() { Name = name, Type = type, DefaultValue = defaultValue };
                        f = x1;
                    }
                    break;*/
                /*case FieldType.Lookup:
                    {
                        var fl = new FieldLookup() { Name = name, Type = type, DefaultValue=defaultValue };
                        try
                        {
                            X.Core.Structures.Schema.Lookup look = new Schema.Structure.Lookup();
                            if (values != null)
                            {
                                look.loadFromString(values.ToString());
                                fl.LookupStructure=look.structure;
                                fl.VisibleFields=look.visibleFields.Split(new[] {','});
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Debug(String.Format("Failed to read lookup settings for field name {0} Exception {1}", name,ex.Message));
                        }                        
                        //fl.DefaultValue = defaultValue.ToString();
                        f = fl;                        
                    }
                    break;*/
                default:
                    {
                        f = new Field() { Name = name, Type = type };
                    }
                    break;
            }
            if (f != null)
            {
                //f.Id = GetNextId();
                f.Required = required;
                _fields.Add(name, f);
                if (IsNullOrEmpty(f.DefaultValue) == false)
                {
                    if (WithDefaultValues == null)
                        WithDefaultValues = new List<Field>();
                    WithDefaultValues.Add(f);
                }
            }
            else
            {
                //log.Debug("Failed to insert field to structure. Name:" + name + " Type:" + type.ToString());
            }
            return f;
        }

        static bool IsNullOrEmpty(object element)
        {
            if (element == null)
                return true;
            if (element.ToString() == "")
                return true;
            return false;
        }


        public Field Get(string fieldName)
        {
            if (_fields.ContainsKey(fieldName))
                return _fields[fieldName];
            return null;
        }
        int _lastId = 0;
        int GetNextId()
        {
            _lastId++;
            return _lastId - 1;
        }

        public Field[] ToArray()
        {
            return _fields.Values.ToArray();
        }

        public IList<Field> AllFields()
        {
            return _fields.Values.ToList();
        }

        public IList<Field> ToList()
        {
            return AllFields();
        }

        public IList<Field> ForSave
        {
            get
            {
                //TODO - caching, flag where collection is changed or refreshed
                return _fields.Values.Where(x => ((x.Marker!= FieldMarker.PrimaryKey) && (x.SaveInDb == false))).ToList();
            }
        }

        public int Count
        {
            get
            {
                return _fields.Count;
            }
        }
        public IEnumerator<Field> GetEnumerator()
        {
            foreach (var f in _fields)
            {
                yield return f.Value;
            }
            //return (IEnumerator<Field>)_fields.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }
    }
}


