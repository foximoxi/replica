﻿using System;
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
    public class ComponentFactory:IComponentFactory
    {
        public ILogger Log { get; set; }

        public IComponent Create(IComponentConfig config)
        {
            IComponent res = null;
            if (config is RestConfig)
                res=CreateComponent(config as RestConfig);
            if (config is StaticResourceConfig)
                res=CreateComponent(config as StaticResourceConfig);
            res.Init();
            return res;
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