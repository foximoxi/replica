using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using R.Config;
using R.Public;
using System.Security.Cryptography;

namespace R.Component
{
    public class CustomRestComponent : RestComponent
    {
        //var dir = R.Component.RestComponent.SHA1(cfg.Uri);
        public async override Task Invoke(IRequestContext ctx)
        {
            switch (ctx.HttpMethod)
            {
                case HttpMethod.PUT:
                    Put(ctx);
                    break;
                case HttpMethod.POST:
                    await Post(ctx);
                    break;
                case HttpMethod.DELETE:
                    Delete(ctx);
                    break;
                case HttpMethod.GET:
                    Get(ctx);
                    break;
            }

        }

        Config.RestConfig Cfg
        {
            get => this.Config as Config.RestConfig;
        }

        void Get(IRequestContext ctx)
        {

        }

        void Delete(IRequestContext ctx)
        {

        }

        void GetAll(IRequestContext ctx)
        {

        }

        void Put(IRequestContext ctx)
        {

        }

        async Task Post(IRequestContext ctx)
        {
            var cfg = this.Cfg;
            var dir=R.Component.RestComponent.SHA1(cfg.Uri);
            System.IO.File.WriteAllText("c:\\rest\\"+dir,ctx.BodyString+".json",Encoding.UTF8);
            await Task.Run(() => { ctx.Response = "{\"msg\":\"afs\"}"; });
        }
    }
}