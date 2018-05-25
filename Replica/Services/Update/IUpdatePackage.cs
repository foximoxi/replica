using System;
using System.Collections.Generic;
using System.Text;
using R.Config;

namespace R.Config.Update
{
    public interface IUpdatePackage
    {
        bool Unpack();
        List<PackageFile> PackageFiles { get; set; }
    }
}
