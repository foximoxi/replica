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
        void RecognizeFiles();
        bool Unpack();
    }

    public enum FileType
    {
        Unknown,
        StaticResource,
        JsonPlainFile,
        RestService,
        ReverseProxyLink,
        InvalidJson
    }
}
