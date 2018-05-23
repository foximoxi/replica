using System;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.CodeDom;
using System.Reflection;
using System.Text;
using System.Linq;

namespace X.Public
{
    public class TestRowsAttribute : Attribute
    {
        public int NumberOfRows { get; private set; }
        
        public TestRowsAttribute(int numberOfRows)
        {
            this.NumberOfRows = numberOfRows;
        }
    }

    public class DescriptionAttribute : Attribute
    {
        string _description;
        X.Language _language;
        public string DescriptionText { get { return _description; } }
        public DescriptionAttribute(string description,X.Language language= Language.English)
        {
            _language = language;
            _description = description;
        }        
    }

    public class NameAttribute : Attribute
    {
        string _name;
        public string Name { get { return _name; } }
        public NameAttribute(string name)
        {
            _name = name;
        }
    }

    public class CreateIfNotExistAttribute : Attribute {}
}