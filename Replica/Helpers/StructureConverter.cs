using System;
using Microsoft.CSharp;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using System.CodeDom;
using System.Reflection;
using System.Text;
using System.IO;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace X.Helpers
{
    public class StructureConverter
    {
        protected string ExtractStructureName(Type t)
        {
#if NETCOREAPP2_0 || NETCOREAPP2_1
            var table = t.GetTypeInfo().GetCustomAttribute<TableAttribute>();
#endif
#if NET450
            var table = t.GetCustomAttribute<TableAttribute>();
#endif

            string name = "";
            if (table != null)
            {
                name = table.Name;
            }
            else
            {
                name = t.Namespace;
                if (String.IsNullOrEmpty(name) == false)
                    name += ".";
                name += t.Name;
            }
            return name;
        }

        public X.Config.Structure ConvertDaoType(Type t)
        {
            var name = ExtractStructureName(t);
            var structure = new X.Config.Structure()
            {
                Name = name,
                Fields = new Config.FieldCollection()
            };
#if NETCOREAPP2_0 || NETCOREAPP2_1
            if (t.GetTypeInfo().GetCustomAttribute<X.Public.TestRowsAttribute>() != null)
            {
                var attr = t.GetTypeInfo().GetCustomAttribute<X.Public.TestRowsAttribute>();
                structure.TestRows = attr.NumberOfRows;
            }
#endif
#if NET450
            if (t.GetCustomAttribute<X.Public.TestRowsAttribute>() != null)
            {
                var attr = t.GetCustomAttribute<X.Public.TestRowsAttribute>();
                structure.TestRows = attr.NumberOfRows;
            }
#endif
#if NETCOREAPP2_0 || NETCOREAPP2_1
            var props = t.GetRuntimeProperties();
#endif
#if NET450
            var props = t.GetProperties();
#endif

            foreach (var f in props)
            {
                var structureField = new X.Config.Field()
                {
                    Name = f.Name,
                    Type = X.Config.Field.ConvertFromType(f.PropertyType),
                    Required = (f.GetCustomAttribute<System.ComponentModel.DataAnnotations.RequiredAttribute>() != null)
                };
                ExtractAttributes(f, structureField, structure);
                structure.Fields.Add(structureField);
            };
            return structure;
        }

        protected void ExtractAttributes(PropertyInfo f, X.Config.Field structureField, X.Config.Structure structure)
        {
            if (f.GetCustomAttribute<System.ComponentModel.DataAnnotations.KeyAttribute>() != null)
            {
                structureField.Marker = Config.FieldMarker.PrimaryKey;
                structure.PK = structureField.Name;
            }
            if (f.GetCustomAttribute<X.Public.CreatedAttribute>() != null)
                structureField.Marker = Config.FieldMarker.Created;
            if (f.GetCustomAttribute<X.Public.ModifiedAttribute>() != null)
                structureField.Marker = Config.FieldMarker.Modified;

            structureField.SaveInDb = (f.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute>() == null);

            var fkAttr = f.GetCustomAttribute<X.Public.ForeignKeyAttribute>();
            if (fkAttr != null)
            {
                structureField.ForeignKeySettings = new Config.ForeignKeySettings() { ReferencedStructure = fkAttr.ReferencedType, CreateConstraint = fkAttr.CreateConstraint };
            }
            var testRowAttr = f.GetCustomAttribute<X.Public.TestRowAttribute>();
            if (testRowAttr != null)
            {
                var rows = new List<Config.FieldTestRow>();
                foreach (var t in testRowAttr.TestValues)
                    rows.Add(new Config.FieldTestRow() { Value = t });
                structureField.FieldTestRows = rows.ToArray();
            }
        }

        protected string ClearNamespace(string name)
        {
            var idx = name.LastIndexOf('.');
            if (idx != -1)
                return name.Substring(idx + 1);
            return name;
        }

        protected void DetectForeignKeys(List<X.Config.Structure> structures)
        {
            foreach (var structure in structures)
            {
                foreach (var field in structure.Fields)
                {
                    string name = field.Name;
                    //detect all fields with id ending, find related structure
                    if ((name.EndsWith("id", StringComparison.OrdinalIgnoreCase)) && (name.Length > 2))
                    {
                        var referencingStructurename = name.Substring(0, name.Length - 2);
                        var referencedStructure = structures.Where(x => String.Compare(ClearNamespace(x.Name), referencingStructurename, true) == 0).FirstOrDefault();
                        if (referencedStructure != null)
                        {
                            field.Marker = Config.FieldMarker.ForeignKey;
                            if (field.ForeignKeySettings == null)
                                field.ForeignKeySettings = new Config.ForeignKeySettings();
                            if (String.IsNullOrEmpty(field.ForeignKeySettings.ReferencedStructure))
                                field.ForeignKeySettings.ReferencedStructure = referencedStructure.Name;
                            field.ForeignKeySettings.CreateConstraint = true;
                        }
                    }
                }
            }
        }
    }
}