using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Runtime.Loader;
using System.Collections.Generic;
using Newtonsoft.Json;
using R.Config;
using R.Component.Config;
using Newtonsoft.Json.Schema;

namespace R.Helpers
{
    public class ConfigExtractor
    {
        R.Helpers.JsonHelper jsonHelper = new JsonHelper();
        public void ReadConfig(R.Config.PackageFile pkg)
        {
            try
            {
                var res = jsonHelper.DeserializeFromDisk<StaticResourceConfig>(pkg.FileInfo.FullName);
                pkg.Config = res;
                if (!ValidateStaticConfig(pkg))
                {
                    var restCfg = jsonHelper.DeserializeFromDisk<RestConfig>(pkg.FileInfo.FullName);
                    pkg.Config = restCfg;
                    if (!String.IsNullOrEmpty(res.Schema))
                    {
                        string path = res.Schema;
                        if (!File.Exists(path))
                            path = System.IO.Path.Combine(pkg.FileInfo.DirectoryName, path);
                        if (File.Exists(path))
                        {
                            using (StreamReader file = File.OpenText(path))
                            using (JsonTextReader reader = new JsonTextReader(file))
                            {
                                restCfg.JsonSchema = JSchema.Load(reader);
                            }
                        }
                    }
                    ValidateRest(pkg);
                    
                }
            }
            catch (Exception ex)
            {
                string s=ex.Message;
                pkg.Status = PackageFileStatus.AnalyzedNotRecognized;
            }            
        }

        bool ValidateRest(PackageFile pkg)
        {
            var cfg = pkg.Config as RestConfig;
            if (cfg == null)
                return false;
            if (cfg.Uri == null)
                return false;

            pkg.Status = PackageFileStatus.AnalyzedReady;
            return true;
        }

        bool ValidateStaticConfig(PackageFile pkg)
        {
            var cfg=pkg.Config as StaticResourceConfig;
            if (cfg == null)
                return false;
            if (cfg.Uri == null)
                return false;
            if (cfg.FilePath == null)
                return false;
            
            if (!File.Exists(cfg.FilePath))
            {
                pkg.Status = PackageFileStatus.AnalyzedConfigurationError;
                var relativePath = System.IO.Path.Combine(pkg.FileInfo.DirectoryName, cfg.FilePath);
                if (File.Exists(relativePath))
                {
                    cfg.FilePath = relativePath;
                    pkg.Status = PackageFileStatus.AnalyzedReady;
                }
            }
            return true;
        }
    }
}