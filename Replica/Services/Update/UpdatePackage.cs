using System;
using System.Collections.Generic;
using System.Linq;
using X.Config;
using X.Config.Update;
using Newtonsoft.Json.Linq;

namespace X.Services.Update
{
    public class UpdatePackage : X.Config.Update.IUpdatePackage
    {
        public ICollection<string> AllFiles { get; set; }
        public Dictionary<string, FileType> RecognizedFiles { get; set; }= new Dictionary<string, FileType>();
        public List<string> CompiledFiles { get; set; } = new List<string>();

        public List<X.Config.DataSource> DataSources { get; private set; } = new List<DataSource>();
        public List<Structure> Structures { get; private set; } = new List<Structure>();
        public List<ViewDefinition> Views { get; private set; } = new List<ViewDefinition>();

        public List<ViewDefinition> TypedViews { get; private set; }
        public List<InsertDefinition> TypedPosts { get; private set; }
        public List<UpdateDefinition> TypedPuts { get; private set; }
        public List<DeleteDefinition> TypedDeletes { get; private set; }
        public List<CustomDefinition> CustomComponents { get; private set; }
        public List<MethodDefinition> CustomMethodComponents { get; private set; }
        public List<Type> ObjectServices { get; private set; }
        public List<Type> GlobalFilters { get; private set; }
        public List<Type> Plugins { get; private set; }

        private StructureValidator structureValidator = new StructureValidator();

        X.Helpers.FileSerializer fileSerializer = new Helpers.FileSerializer();

        public bool DeserializeAll()
        {
            DeassemblyFiles();
            return Validate();
        }

        bool Validate()
        {
            return structureValidator.ValidationResult.IsValid;
        }

        public ICollection<X.Public.ValidationInfo> PackageValidationOutput
        {
            get
            {
                return structureValidator.ValidationResult.ValidationResults;
            }
        }

        public void RecognizeFiles()
        {
            foreach (var f in AllFiles)
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
                case FileType.DataSource:
                    DeserializeJsonDataSource(fileName);
                    break;
                case FileType.JsonSettings:
                    break;
                case FileType.JsonView:
                    DeserializeJsonView(fileName);
                    break;
                case FileType.XmlStructure:
                    DeserializeXmlStructure(fileName);
                    break;
                case FileType.Binary:
                    CompiledFiles.Add(fileName);
                    break;
            }
        }

        void DetectFile(string fileName)
        {
            var f = System.IO.File.ReadAllText(fileName, System.Text.Encoding.UTF8);

            var ext = System.IO.Path.GetExtension(fileName);
            if (ext == ".settings")
            {
                return;
            }
            if (ext == ".dll")
            {
                RecognizedFiles[fileName] = FileType.Binary;
                CompiledFiles.Add(fileName);
                return;
            }

            switch (f[0])
            {
                case '<':
                    {
                        if (DeserializeXmlStructure(fileName, false))
                            RecognizedFiles[fileName] = FileType.XmlStructure;
                        else
                            throw new Exception("undetected format for file " + fileName);
                    }
                    break;
                case '{':
                    {
                        var t = DetectJson(fileName);
                        if (t==FileType.JsonView)
                            DeserializeJsonView(fileName);
                        else
                            DeserializeJsonDataSource(fileName);
                        RecognizedFiles[fileName] = t;
                    }
                    break;
            }
        }

        FileType DetectJson(string fileName)
        {
            var json=JObject.Parse(System.IO.File.ReadAllText(fileName, System.Text.Encoding.UTF8));
            if ((json["engine"] != null) && (json["connectionString"] != null))
                return FileType.DataSource;
            if ((json["query"] != null) && (json["uri"] != null))
                return FileType.JsonView;
            throw new Exception("undetected format for file " + fileName);
        }

        bool DeserializeXmlStructure(string fileName, bool throwIfInvalid = true)
        {
            try
            {
                var structure = fileSerializer.DeserializeFromDisk<Structure>(fileName, X.Helpers.SerializationType.XML, System.Text.Encoding.UTF8);
                this.Structures.Add(structure);
            }
            catch (Exception ex)
            {
                if (throwIfInvalid)
                    throw ex;
                else
                    return false;
            }
            return true;
        }


        X.Helpers.JsonHelper jsonHelper { get; set; } = new X.Helpers.JsonHelper();
        bool DeserializeJsonDataSource(string fileName, bool throwInvalid = true)
        {
            try
            {
                var res = jsonHelper.DeserializeFromDisk<X.Config.DataSource>(fileName);
                DataSources.Add(res);
            }
            catch (Exception ex)
            {
                if (throwInvalid)
                    throw ex;
                else
                    return false;
            }
            return true;
        }

        bool DeserializeJsonView(string viewFile, bool throwInvalid = true)
        {
            try
            {
                var res = jsonHelper.DeserializeFromDisk<X.Config.ViewDefinition>(viewFile);
                Views.Add(res);
            }
            catch (Exception ex)
            {
                if (throwInvalid)
                    throw ex;
                else
                    return false;
            }
            return true;
        }

        void InitLists()
        {
            TypedViews = new List<ViewDefinition>();
            TypedPosts = new List<InsertDefinition>();
            TypedPuts = new List<UpdateDefinition>();
            TypedDeletes = new List<DeleteDefinition>();
            ObjectServices = new List<Type>();
            GlobalFilters = new List<Type>();
            CustomComponents = new List<CustomDefinition>();
            Plugins = new List<Type>();
            CustomMethodComponents = new List<MethodDefinition>();
        }

        void DeassemblyFiles()
        {
            if (this.CompiledFiles != null)
            {
                InitLists();
                foreach (var file in CompiledFiles)
                    AddFile(file);
            }
        }

        void AddFile(string file)
        {
            if (IgnoreFile(file) == false)
            {
                var extractor = new X.Helpers.DefinitionExtractor(file);
                Structures.AddRange(extractor.ExtractStructures());
                TypedViews.AddRange(extractor.ExtractTypedViews());
                TypedPosts.AddRange(extractor.ExtractTypedPosts());
                TypedPuts.AddRange(extractor.ExtractTypedPuts());
                TypedDeletes.AddRange(extractor.ExtractTypedDeletes());
                CustomComponents.AddRange(extractor.ExtractCustomComponents());
                CustomMethodComponents.AddRange(extractor.ExtractMethodsFromCustomComponents());
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