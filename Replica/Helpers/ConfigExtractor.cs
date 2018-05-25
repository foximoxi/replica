using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyModel;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using R.Public;
using R.Config;
using R.Component.Config;

namespace R.Helpers
{
    public class ConfigExtractor : ReflectionHelper
    {
        R.Helpers.JsonHelper jsonHelper = new JsonHelper();
        public IComponentConfig ReadConfig(R.Config.PackageFile pf)
        {
            try
            {
                var res = jsonHelper.DeserializeFromDisk<StaticResourceConfig>(pf.FileInfo.FullName);
                pf.Config = res;
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
    }
}