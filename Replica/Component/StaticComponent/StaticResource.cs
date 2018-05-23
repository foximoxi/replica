using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

using R.Config;

namespace R.Component
{
    public class StaticResourceComponent: RestComponent
    {
        StaticResourceConfig Config { get; set; }
        public override async Task Invoke(IRequestContext ctx)
        {            
            await Task.Run(() => {
                ctx.Response = System.IO.File.ReadAllText(Config.FilePath);
                ctx.ResponseType = ResponseType.JSON;
            });
        }
    }
}