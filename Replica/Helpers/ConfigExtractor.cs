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
using R.Component.Config;
using System.ComponentModel.DataAnnotations.Schema;

namespace R.Helpers
{
    public class ConfigExtractor : ReflectionHelper
    {
        R.Helpers.JsonHelper jsonHelper = new JsonHelper();
        public IComponentConfig Read(R.Config.PackageFile pf)
        {
            try
            {
                var res = jsonHelper.DeserializeFromDisk<StaticResourceConfig>(pf.FileInfo.FullName);
                return res;
            }
            catch
            {
                try
                {
                    var res = jsonHelper.DeserializeFromDisk<RestConfig>(pf.FileInfo.FullName);
                    return res;
                }
                catch
                {
                }
            }
            return null;
        }


        EndPointUri ExtractUri(Type t, Type attrType, Op opForRestPack, MethodInfo mi = null)
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
                    return new EndPointUri(pack.LocatorUrl, mth);
                else
                    return new EndPointUri(pack.Url, mth);
            }
        }
    }
}