using System;
using System.Collections.Generic;
using System.Linq;
using R.Config;
using R.Config.Update;
using Newtonsoft.Json.Linq;

namespace R.Services.Update
{
    public class UpdatePackage : R.Config.Update.IUpdatePackage
    {
        public ICollection<string> UnrecognizedFiles { get; set; }
        public Dictionary<string, FileType> RecognizedFiles { get; set; } = new Dictionary<string, FileType>();
        R.Helpers.FileSerializer fileSerializer = new Helpers.FileSerializer();
        Dictionary<string, IComponent> Components = new Dictionary<string, IComponent>();

        public UpdatePackage(ICollection<string> allFiles)
        {
            this.UnrecognizedFiles = allFiles;
            RecognizeFiles();
        }

        public bool Unpack()
        {
            return true;
        }
        
        public void RecognizeFiles()
        {
            foreach (var f in UnrecognizedFiles)
            {
                if (RecognizedFiles.ContainsKey(f))
                    AddToPackage(f, RecognizedFiles[f]);
                else
                    AddToPackage(f);
            }
        }

        void AddToPackage(string fileName, FileType type = FileType.Unknown)
        {
            switch (type)
            {
                case FileType.Unknown:
                    DetectFile(fileName);
                    break;
            }
        }

        void DetectFile(string fileName)
        {
            var f = System.IO.File.ReadAllText(fileName, System.Text.Encoding.UTF8);
            var ext = System.IO.Path.GetExtension(fileName);
            if (ext == ".json")
            {
                RecognizedFiles[fileName] = R.Helpers.JsonHelper.ValidateJson(System.IO.File.ReadAllText(fileName, System.Text.Encoding.UTF8)) ? FileType.JsonPlainFile : FileType.InvalidJson;
            }
        }

        void AddFile(string file)
        {
            var extractor = new R.Helpers.DefinitionExtractor(file);            
        }
    }
}