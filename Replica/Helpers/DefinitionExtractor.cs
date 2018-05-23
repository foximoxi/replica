using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyModel;
using Newtonsoft.Json;
using R.Public;
using R.Config;
using System.ComponentModel.DataAnnotations.Schema;

namespace R.Helpers
{
    public class DefinitionExtractor : ReflectionHelper
    {
        public Assembly Assembly { get; private set; }
        public Dictionary<string, Type> ExtractedTypes { get; private set; }
        public DefinitionExtractor(Assembly assembly)
        {
            Assembly = assembly;
            ExtractedTypes = ExtractAllTypes(Assembly);
        }
        public DefinitionExtractor(string filePath)
        {
            Assembly = this.LoadAssembly(filePath);
            ExtractedTypes = ExtractAllTypes(Assembly);
        }

        public R.Config.Structure[] ExtractStructures()
        {
            var ret = new List<R.Config.Structure>();
            var tables = ExtractedTypes.Where(x => x.Value.GetTypeInfo().GetCustomAttribute<TableAttribute>(false) != null);
            var converter = new R.Helpers.StructureConverter();

            foreach (var t in tables.Where(x => x.Value.GetTypeInfo().GetCustomAttribute<CreateIfNotExistAttribute>(false) != null))
            {
                var structure=converter.ConvertDaoType(t.Value);
                ret.Add(structure);
            }
            return ret.ToArray();
        }

        public R.Config.ViewDefinition[] ExtractTypedViews()
        {
            var ret = new List<R.Config.ViewDefinition>();
            foreach (var t in ExtractedTypes.Where(x => x.Value.GetTypeInfo().GetCustomAttribute<RestGetAttribute>(false) != null))
                ret.Add(CreateViewDefinition(t,Op.GET));
            foreach (var t in ExtractedTypes.Where(x => x.Value.GetTypeInfo().GetCustomAttribute<RestLocatorAttribute>(false) != null))
                ret.Add(CreateViewLocatorDefinition(t, Op.GET_ONE));
            foreach (var t in FromRestPack(ExtractedTypes, Op.GET))
                ret.Add(CreateViewDefinition(t, Op.GET));
            foreach (var t in FromRestPack(ExtractedTypes, Op.GET_ONE))
                ret.Add(CreateViewDefinition(t, Op.GET_ONE));
            return ret.Where(x => typeof(R.Config.IComponent).IsAssignableFrom(x.TypeForQuery)==false).ToArray();            
        }        

        public R.Config.InsertDefinition[] ExtractTypedPosts()
        {
            var ret = new List<InsertDefinition>();
            foreach (var t in ExtractedTypes.Where(x => x.Value.GetTypeInfo().GetCustomAttribute<RestPostAttribute>(false) != null).ToArray())
                ret.Add(CreateInsertDefinition(t, Op.POST));
            foreach (var t in FromRestPack(ExtractedTypes, Op.POST))
                ret.Add(CreateInsertDefinition(t,Op.POST));
            return ret.Where(x => typeof(R.Config.IComponent).IsAssignableFrom(x.Type) == false).ToArray();
        }

        public R.Config.UpdateDefinition[] ExtractTypedPuts()
        {
            var ret = new List<UpdateDefinition>();

            foreach (var t in ExtractedTypes.Where(x => x.Value.GetTypeInfo().GetCustomAttribute<RestPutAttribute>(false) != null).ToArray())
                ret.Add(CreateUpdateDefinition(t,Op.PUT));
            foreach (var t in FromRestPack(ExtractedTypes, Op.PUT))
                ret.Add(CreateUpdateDefinition(t, Op.PUT));
            return ret.Where(x => typeof(R.Config.IComponent).IsAssignableFrom(x.Type) == false).ToArray();
        }

        public R.Config.DeleteDefinition[] ExtractTypedDeletes()
        {
            var ret = new List<DeleteDefinition>();

            foreach (var t in ExtractedTypes.Where(x => x.Value.GetTypeInfo().GetCustomAttribute<RestDeleteAttribute>(false) != null).ToArray())
                ret.Add(CreateDeleteDefinition(t,Op.DELETE));
            foreach (var t in FromRestPack(ExtractedTypes, Op.DELETE))
                ret.Add(CreateDeleteDefinition(t, Op.DELETE));
            return ret.Where(x => typeof(R.Config.IComponent).IsAssignableFrom(x.Type) == false).ToArray();
        }

        public R.Config.CustomDefinition[] ExtractCustomComponents()
        {
            var components = ExtractedTypes.Values.Where(x => typeof(R.Config.IComponent).IsAssignableFrom(x)).ToList();
            var ret = new List<CustomDefinition>();
            foreach (var t in components.Where(x => x.GetTypeInfo().GetCustomAttribute<RestGetAttribute>(false) != null).ToArray())
                ret.Add(CreateCustomDefinition(t, Op.GET,typeof(RestGetAttribute)));
            foreach (var t in components.Where(x => x.GetTypeInfo().GetCustomAttribute<RestPostAttribute>(false) != null).ToArray())
                ret.Add(CreateCustomDefinition(t,Op.POST, typeof(RestPostAttribute)));
            foreach (var t in components.Where(x => x.GetTypeInfo().GetCustomAttribute<RestPutAttribute>(false) != null).ToArray())
                ret.Add(CreateCustomDefinition(t, Op.PUT,typeof(RestPutAttribute)));
            foreach (var t in components.Where(x => x.GetTypeInfo().GetCustomAttribute<RestDeleteAttribute>(false) != null).ToArray())
                ret.Add(CreateCustomDefinition(t, Op.DELETE,typeof(RestDeleteAttribute)));

            //extract restpacks
            foreach (var op in Enum.GetValues(typeof(Op)))
            {
                foreach (var t in FromRestPack2(components, (Op)op))
                    ret.Add(CreateCustomDefinition(t,(Op)op,null));
            }
            return ret.ToArray();
        }

        public List<MethodDefinition> ExtractMethodsFromCustomComponents()
        {
            var components = ExtractedTypes.Values.Where(x => typeof(R.Config.IComponent).IsAssignableFrom(x)).ToList();
            var ret = new List<MethodDefinition>();
            foreach (var component in components)
            {
                var methods = component.GetMethods();
                var inv = methods.Where(x => x.GetCustomAttribute<RestGetAttribute>(false) != null).ToArray();
                foreach (var t in component.GetMethods().Where(x => x.GetCustomAttribute<RestGetAttribute>(false) != null).ToArray())
                    ret.Add(CreateCustomMethodDefinition(component, Op.GET, typeof(RestGetAttribute),t));
                foreach (var t in component.GetMethods().Where(x => x.GetCustomAttribute<RestPostAttribute>(false) != null).ToArray())
                    ret.Add(CreateCustomMethodDefinition(component, Op.POST, typeof(RestPostAttribute),t));
                foreach (var t in component.GetMethods().Where(x => x.GetCustomAttribute<RestPutAttribute>(false) != null).ToArray())
                    ret.Add(CreateCustomMethodDefinition(component, Op.PUT, typeof(RestPutAttribute),t));
                foreach (var t in component.GetMethods().Where(x => x.GetCustomAttribute<RestDeleteAttribute>(false) != null).ToArray())
                    ret.Add(CreateCustomMethodDefinition(component, Op.DELETE, typeof(RestDeleteAttribute),t));
            }
            return ret;
        }

        EndPointUri ExtractUri(Type t, Type attrType,Op opForRestPack, MethodInfo mi=null)
        {
            Attribute attr = null;
            if (mi != null)
                attr = mi.GetCustomAttribute(attrType, false);
            else
            {
                if (attrType != null)
                    attr = t.GetTypeInfo().GetCustomAttribute(attrType, false);
            }

            if (attr != null)
            {
                var a = ((IRestAttr)attr);
                return new EndPointUri(a.Url, a.Method);
            }
            else
            {
                var pack = t.GetTypeInfo().GetCustomAttribute<RestPackAttribute>(false);
                var mth = attrType.Name.ToUpper().Replace("REST", "").Replace("ATTRIBUTE", "").ToUpper();
                if (opForRestPack == Op.GET_ONE | opForRestPack == Op.PUT | opForRestPack == Op.DELETE)
                    return new EndPointUri(pack.LocatorUrl,mth);
                else
                    return new EndPointUri(pack.Url, mth);
            }
        }

        ResourceViewType ExtractResourceViewType(Type t, Type attrType)
        {
            var attr = t.GetTypeInfo().GetCustomAttribute(attrType,false) as IRestAttrEx;
            if (attr != null)
            {
                if (attr.ReturnViewType == ResourceViewType.AsLocationUri)
                    if (t.GetTypeInfo().GetCustomAttribute<R.Public.RestLocatorAttribute>(false) == null)
                        throw new Exception("Missing RestLocator in " + attrType.Name + " attribute of type:" + t.Name);
                return attr.ReturnViewType;
            }
            else
            {
                var pack = t.GetTypeInfo().GetCustomAttribute<RestPackAttribute>(false);
                return ResourceViewType.AsFullObject;
            }            
        }

        IEnumerable<KeyValuePair<string, Type>> FromRestPack(Dictionary<string, Type> types, R.Public.Op flag)
        {
            return ExtractedTypes.Where(x => ((x.Value.GetTypeInfo().GetCustomAttribute<RestPackAttribute>(false) != null) && ((x.Value.GetTypeInfo().GetCustomAttribute<RestPackAttribute>(false).Operations & flag) == flag)));
        }

        IEnumerable<Type> FromRestPack2(IEnumerable<Type> types, R.Public.Op flag)
        {
            return types.Where(x => ((x.GetTypeInfo().GetCustomAttribute<RestPackAttribute>(false) != null) && ((x.GetTypeInfo().GetCustomAttribute<RestPackAttribute>(false).Operations & flag) == flag)));
        }

        ViewDefinition CreateViewLocatorDefinition(KeyValuePair<string, Type> t, Op op)
        {
            return new ViewDefinition()
            {
                TypeForQuery = t.Value,
                Uri = ExtractUri(t.Value, typeof(RestLocatorAttribute), op),
                ReturnType = ReturnType.Single,
                Operation = op
            };
        }
        ViewDefinition CreateViewDefinition(KeyValuePair<string, Type> t, Op op)
        {
            return new ViewDefinition()
            {
                TypeForQuery = t.Value,
                Uri = ExtractUri(t.Value, typeof(RestGetAttribute), op),
                Operation = op,
                ReturnType = op == Op.GET_ONE ? ReturnType.Single : ReturnType.Default
            };
        }

        InsertDefinition CreateInsertDefinition(KeyValuePair<string, Type> t, Op op)
        {
            return new InsertDefinition()
            {
                Type = t.Value,
                Uri = ExtractUri(t.Value, typeof(RestPostAttribute),op),
                ReturnResourceViewType = ExtractResourceViewType(t.Value,typeof(RestPostAttribute)),
                Operation = op
            };
        }

        UpdateDefinition CreateUpdateDefinition(KeyValuePair<string, Type> t, Op op)
        {
            return new UpdateDefinition()
            {
                Type = t.Value,
                Uri = ExtractUri(t.Value, typeof(RestPutAttribute),op),
                ReturnResourceViewType = ExtractResourceViewType(t.Value, typeof(RestPutAttribute)),
                Operation = op
            };
        }

        DeleteDefinition CreateDeleteDefinition(KeyValuePair<string, Type> t, Op op)
        {
            return new DeleteDefinition()
            {
                Type = t.Value,
                Uri = ExtractUri(t.Value, typeof(RestDeleteAttribute),op),
                Operation = op
            };
        }

        CustomDefinition CreateCustomDefinition(Type t,Op op,Type attrType)
        {
            var res = new CustomDefinition()
            {
                ComponentType = t,
                Operation = op,
                Uri=ExtractUri(t, attrType, op)
            };
            return res;
        }

        MethodDefinition CreateCustomMethodDefinition(Type t, Op op,Type attrType,MethodInfo mi)
        {
            var res = new MethodDefinition()
            {
                ComponentType = t,
                Operation = op,
                Uri = ExtractUri(t, attrType, op,mi),
                MethodInfo=mi
            };
            return res;
        }
    }
}