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

        private StructureValidator structureValidator = new StructureValidator();

        R.Helpers.FileSerializer fileSerializer = new Helpers.FileSerializer();
        
        public UpdatePackage(ICollection<string> allFiles)
        {
            this.UnrecognizedFiles = allFiles;
            RecognizeFiles();
        }

        public bool DeserializeAll()
        {
            return Validate();
        }

        bool Validate()
        {
            return structureValidator.ValidationResult.IsValid;
        }

        public ICollection<R.Public.ValidationInfo> PackageValidationOutput
        {
            get
            {
                return structureValidator.ValidationResult.ValidationResults;
            }
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
            if (IgnoreFile(file) == false)
            {
                var extractor = new R.Helpers.DefinitionExtractor(file);
            }
        }

        bool IgnoreFile(string file)
        {
            if (file.ToLower().Contains("iconfig.dll"))
                return true;
            if (file.ToLower().Contains("iux.dll"))
                return true;
            if (file.ToLower().Contains("bouncycastle.crypto.dll"))
                return true;
            if (file.ToLower().Contains("mailkit.dll"))
                return true;
            return false;
        }
    }
}