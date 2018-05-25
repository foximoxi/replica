using System;
using System.Collections.Generic;
using System.Linq;
using R.Config;
using R.Config.Update;
using Newtonsoft.Json.Linq;

namespace R.Services.Update
{
    public class PackageFile
    {
        public PackageFile(string path)
        {
            FileInfo = new System.IO.FileInfo(path);
        }
        public System.IO.FileInfo FileInfo { get; set; }
        
    }
}