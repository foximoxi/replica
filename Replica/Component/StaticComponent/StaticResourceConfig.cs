using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

using R.Config;

namespace R.Component
{
    public class StaticResourceConfig
    {
        public string Uri { get; set; }
        public string FilePath { get; set; }
        public int FileType { get; set; }
        public string Schema { get; set; }
    }
}