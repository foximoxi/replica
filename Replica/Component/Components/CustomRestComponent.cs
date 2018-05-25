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
        R.Component.Config.RestConfig Config { get; set; }
        string SavePath { get; set; }
        
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

        public override void Init()
        {
            this.Config = this.Configuration as Config.RestConfig;
            this.SavePath = System.IO.Path.Combine("c:\\rest" + R.Component.RestComponent.SHA1(this.Config.Uri));
        }


        void Get(IRequestContext ctx)
        {
            if (ctx.InputParameters.ContainsKey("{" + Config.Identity + "}"))
            {

            }
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
            System.IO.File.WriteAllText(SavePath + ".json", ctx.BodyString,Encoding.UTF8);
            await Task.Run(() => { ctx.Response = "{\"msg\":\"afs\"}"; });
        }
    }
}