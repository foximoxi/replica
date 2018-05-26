using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using R.Config;
using R.Public;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Reflection;
using R.Component.Config;
using R.Component;

namespace R.Services
{
    public interface IComponentFactory
    {
        ILogger Log { get; set; }
        IComponent Create(IComponentConfig config);
    }
}