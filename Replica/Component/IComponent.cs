using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using R.Public;
using Microsoft.Extensions.Logging;
using R.Component.Config;

namespace R.Component
{
    public interface IComponent
    {
        IComponentConfig Configuration { get; set; }
        ILogger Log { get; set; }
        Task Invoke(R.Config.IRequestContext context);
        R.Config.EndPointUri CustomUri { get; }
        void Init();
    }
}