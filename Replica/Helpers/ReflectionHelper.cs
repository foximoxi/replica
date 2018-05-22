using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;
using System.Runtime.Loader;
using System.IO;

namespace X.Helpers
{
    //interface for extracting class properties and adnotations to create servicig form
    public class ReflectionHelper
    {
        protected Dictionary<string, Type> ExtractAssignableTypes(Assembly assembly, Type type)
        {
            var types = assembly.GetTypes();
            var found = types.Where(x => type.IsAssignableFrom(x)).ToList();
            return found.ToDictionary(t => t.Name, t => t);
        }

        protected Dictionary<string, Type> ExtractAssignableTypes<T>(Assembly assembly) where T : class
        {
            return ExtractAssignableTypes(assembly, typeof(T));
        }

        public Assembly LoadAssembly(string assemblyPath)
        {
            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);
            return assembly;
        }

        public Dictionary<string, Type> ExtractAllTypes(Assembly assembly)
        {
            var o = assembly.GetTypes();
            return o.ToDictionary(t => t.FullName, t => t);
        }

        public static Tuple<Assembly, IList<Type>> LoadAndGetAssignableTypes(Type type, string assemblyPath)
        {
            var a = AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);
            var types = a.GetTypes().Where(t => type.IsAssignableFrom(t)).ToList();
            return new Tuple<Assembly, IList<Type>>(a, types);
        }
    }
}