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

namespace R.Component
{
    public class ComponentFactory
    {
        public ILogger Log { get; set; }

        public IComponent Create(IComponentConfig config)
        {
            if (config is RestConfig)
                return CreateComponent(config as RestConfig);
            if (config is StaticResourceConfig)
                return CreateComponent(config as StaticResourceConfig);
            return null;
        }

        public RestComponent CreateComponent(RestConfig config)
        {
            return new CustomRestComponent() { Configuration = config };
        }

        public StaticResourceComponent CreateComponent(StaticResourceConfig config)
        {
            return new StaticResourceComponent() { Configuration = config };
        }
    }
}