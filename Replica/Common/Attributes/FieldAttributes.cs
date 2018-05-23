using System;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.CodeDom;
using System.Reflection;
using System.Text;
using System.Linq;

namespace R.Public
{
    public class ForeignKeyAttribute : Attribute
    {
        string _referencedType;
        bool _createContstraint;
        public string ReferencedType { get { return _referencedType; } }
        public bool CreateConstraint { get { return _createContstraint; } }
        public ForeignKeyAttribute(bool createConstraint = false)
        {
            _createContstraint = createConstraint;
        }
        public ForeignKeyAttribute(Type type, bool createConstraint = false)
        {
            _referencedType = type.Name;
            _createContstraint = createConstraint;
        }
    }

    public class UniqueAttribute : Attribute { }
    public class IndexedAttribute : Attribute { }

    public class ModifiedAttribute : Attribute { }
    public class CreatedAttribute : Attribute { }

    public class TestRowAttribute : Attribute
    {
        public string[] TestValues { get; private set; }
        public TestRowAttribute(params string[] values)
        {
            TestValues = values;
        }
        public TestRowAttribute(params int[] values)
        {
            TestValues = values.Select(x => x.ToString()).ToArray();
        }
        public TestRowAttribute(params bool[] values)
        {
            TestValues = values.Select(x => x.ToString()).ToArray();
        }
        public TestRowAttribute(int count, object rangeMin, object rangeMax)
        {
        }
    }

    public class VirtualAttribute : Attribute { }

    //rest view attribute for filtering records in view
    public class ParameterAttribute : Attribute
    {
        public string ParamName { get; private set; }
        public string Structure { get; private set; }
        public string FieldName { get; private set; }
        public string Value { get; private set; }
        public ParameterAttribute(string paramName, Type dao, string fieldName,string value=null)
        {
            ParamName = paramName;
            Structure = dao.Name;
            FieldName = fieldName;
        }

        public ParameterAttribute(string paramName, string dao, string fieldName, string value = null)
        {
            ParamName = paramName;
            Structure = dao;
            FieldName = fieldName;
        }
    }

    public class Parameter2Attribute : ParameterAttribute
    {
        public Parameter2Attribute(string paramName, string dao, string fieldName, string value = null) : base(paramName, dao, fieldName, value)
        {
        }
        public Parameter2Attribute(string paramName, Type dao, string fieldName, string value = null) : base(paramName, dao, fieldName, value)
        {
        }
    }
}