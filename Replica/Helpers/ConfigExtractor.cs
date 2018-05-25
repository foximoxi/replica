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
        public IComponentConfig ReadConfig(R.Config.PackageFile pkg)
        {
            try
            {
                var res = jsonHelper.DeserializeFromDisk<StaticResourceConfig>(pkg.FileInfo.FullName);
                pkg.Config = res;
                ValidateStaticConfig(pkg);
            }
            catch
            {
                try
                {
                    var res = jsonHelper.DeserializeFromDisk<RestConfig>(pkg.FileInfo.FullName);
                    return res;
                }
                catch
                {
                }
            }
            return null;
        }

        void ValidateStaticConfig(PackageFile pkg)
        {
            var cfg = pkg.Config as StaticResourceConfig;
            if (!File.Exists(cfg.FilePath))
            {
                pkg.Status = PackageFileStatus.AnalyzedConfigurationError;
                var relativePath=System.IO.Path.Combine(pkg.FileInfo.DirectoryName, cfg.FilePath);
                if (File.Exists(relativePath))
                {
                    cfg.FilePath = relativePath;
                    pkg.Status = PackageFileStatus.AnalyzedReady;                    
                }
            }
        }
    }
}