using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

using R.Config;

namespace R.Component
{
    public class StaticResourceComponent: RestComponent
    {
        public override async Task Invoke(IRequestContext ctx)
        {            
            await Task.Run(() => {
                ctx.Response = System.IO.File.ReadAllText(Cfg.FilePath);
                ctx.ResponseType = ResponseType.JSON;
            });
        }

        StaticResourceConfig Cfg
        {
            get => this.Config as StaticResourceConfig;
        }
    }
}