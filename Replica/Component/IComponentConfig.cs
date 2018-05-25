using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace R.Config
{
    public interface IComponentConfig
    {
        string Uri { get; set; }
    }
}