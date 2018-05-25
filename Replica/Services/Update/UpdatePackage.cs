using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using R.Config;
using R.Config.Update;
using Newtonsoft.Json.Linq;
using R.Component;

namespace R.Services.Update
{
    public class UpdatePackage : IUpdatePackage
    {
        public Dictionary<string, FileType> RecognizedFiles { get; set; } = new Dictionary<string, FileType>();
        R.Helpers.FileSerializer fileSerializer = new Helpers.FileSerializer();
        public List<PackageFile> PackageFiles { get; set; }

        public UpdatePackage(ICollection<string> allFiles)
        {
            PackageFiles = new List<PackageFile>();
            foreach (var path in allFiles)
                PackageFiles.Add(new PackageFile(path));
        }

        public bool Unpack(DateTime lastUpdateTime)
        {
            var extractor = new Helpers.ConfigExtractor();
            foreach (var p in PackageFiles)
            {
                Analyse(p, lastUpdateTime);
                if (p.Status == PackageFileStatus.BeforeAnalysis)
                {
                    extractor.ReadConfig(p);
                }
            }
            return true;
        }

        void Analyse(PackageFile p, DateTime lastUpdateTime)
        {
            if (p.FileInfo.LastWriteTime < lastUpdateTime)
                p.Status = PackageFileStatus.NotModified;
            else
                p.Status = PackageFileStatus.BeforeAnalysis;
        }
    }
}