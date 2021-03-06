﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using R.Component.Config;
using R.Config;

namespace R.Component
{
    public class StaticResourceComponent: RestComponent
    {
        public override async Task Invoke(IRequestContext ctx)
        {            
            await Task.Run(() => {
                ctx.Response = System.IO.File.ReadAllText(Cfg.FilePath, Encoding.UTF8);
                ctx.ResponseType = ResponseType.JSON;
            });
        }

        StaticResourceConfig Cfg
        {
            get => this.Configuration as StaticResourceConfig;
        }
    }
}