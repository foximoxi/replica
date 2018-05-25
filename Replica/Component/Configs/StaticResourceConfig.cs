using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace R.Component.Config
{
    public class StaticResourceConfig:IComponentConfig
    {
        public string Uri { get; set; }
        public string FilePath { get; set; }
        public int FileType { get; set; }
        public string Schema { get; set; }
    }
}