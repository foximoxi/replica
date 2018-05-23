using System;
using System.Collections.Generic;
using System.Text;
using X.Config;

namespace X.Config.Update
{
    public interface IUpdatePackage
    {
        ICollection<string> AllFiles { get; set; }
        Dictionary<string, FileType> RecognizedFiles { get; set; }
        List<string> CompiledFiles { get; set; }

        List<DataSource> DataSources { get; }
        List<ViewDefinition> Views { get; }
        List<ViewDefinition> TypedViews { get; }
        List<InsertDefinition> TypedPosts { get; }
        List<UpdateDefinition> TypedPuts { get; }
        List<DeleteDefinition> TypedDeletes { get; }
        List<CustomDefinition> CustomComponents { get; }
        List<MethodDefinition> CustomMethodComponents { get; }
        List<Type> ObjectServices { get; }
        List<Type> GlobalFilters { get; }
        List<Type> Plugins { get; }

        bool DeserializeAll();
        void RecognizeFiles();
    }

    public enum FileType
    {
        Unknown,
        JsonPlainFile,
        ReverseProxyConfig,
    }
}
