using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using R.Config;
using R.Config.Update;
using Newtonsoft.Json.Linq;

namespace R.Services.Update
{
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