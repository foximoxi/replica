﻿using System;
using System.Collections.Generic;
using System.Linq;
using R.Config;
using R.Config.Update;
using Newtonsoft.Json.Linq;

namespace R.Config
{
    public class PackageFile
    {
        public PackageFile(string path)
        {
            FileInfo = new System.IO.FileInfo(path);
        }

        public R.Component.Config.IComponentConfig Config { get; set; }
        public System.IO.FileInfo FileInfo { get; set; }
        public PackageFileStatus Status { get; set; } = PackageFileStatus.Unknown;
    }

    public enum PackageFileStatus
    {
        Unknown,
        ShouldAnalyse,
        NotModified,
        Modified
    }
}