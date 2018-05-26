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
    public class ComponentFactory:IComponentFactory
    {
        public ILogger Log { get; set; }
        public IPseudoDbService DB;
        public ComponentFactory(IPseudoDbService db)
        {
            this.DB = db;
        }        

        public IComponent Create(IComponentConfig config)
        {
            IComponent res = null;
            if (config is RestConfig)
                res = new CustomRestComponent() { Configuration = config, Db = DB };
            else
            {
                if (config is StaticResourceConfig)
                    res = new StaticResourceComponent() { Configuration = config };
            }
            res.Init();
            return res;
        }
    }
}