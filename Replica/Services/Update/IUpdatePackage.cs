using System;
using System.Collections.Generic;
using System.Text;
using R.Config;

namespace R.Config.Update
{
    public interface IUpdatePackage
    {
        ICollection<string> UnrecognizedFiles { get; set; }
        Dictionary<string, FileType> RecognizedFiles { get; set; }
        bool DeserializeAll();
        void RecognizeFiles();
    }

    public enum FileType
    {
        Unknown,
        JsonPlainFile,
        ReverseProxyConfig,
        InvalidJson
    }
}
