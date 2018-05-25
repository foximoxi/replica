using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using R.Config;
using R.Public;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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
            this.SavePath = System.IO.Path.Combine("c:\\rest" + R.Component.RestComponent.SHA1(this.Config.Uri) + "\\");
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
            await Task.Run(() =>
            {
                if (!String.IsNullOrEmpty(Config.Identity))
                {
                    JObject o = JObject.Parse(ctx.BodyString);
                    var id = (string)o[Config.Identity];
                    if (id == null)
                    {
                        o[Config.Identity] = id = GetNextIdentity();
                    }
                    var str = Newtonsoft.Json.JsonConvert.SerializeObject(o);
                    if (System.IO.Directory.Exists(SavePath) == false)
                        System.IO.Directory.CreateDirectory(SavePath);
                    System.IO.File.WriteAllText(SavePath + id + ".json", Newtonsoft.Json.JsonConvert.SerializeObject(str), Encoding.UTF8);
                    ctx.Response = str;
                }
            });
        }

        int cnt = 0;
        string GetNextIdentity()
        {
            cnt++;
            return cnt.ToString();
        }
    }

}