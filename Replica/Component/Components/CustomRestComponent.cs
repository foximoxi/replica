using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using R.Config;
using R.Public;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using R.Services;

namespace R.Component
{
    public class CustomRestComponent : RestComponent
    {
        public R.Services.IPseudoDbService Db { get; set; }
        string CollectionName { get; set; }
        R.Component.Config.RestConfig Config { get; set; }
        string SavePath { get; set; }
        string IdentityFieldName { get; set; }

        public async override Task Invoke(IRequestContext ctx)
        {
            switch (ctx.HttpMethod)
            {
                case HttpMethod.PUT:
                    await Put(ctx);
                    break;
                case HttpMethod.POST:
                    await Post(ctx);
                    break;
                case HttpMethod.DELETE:
                    await Delete(ctx);
                    break;
                case HttpMethod.GET:
                    Get(ctx);
                    break;
            }
        }

        public override void Init()
        {
            this.Config = this.Configuration as Config.RestConfig;
            IdentityFieldName = this.Config.Identity;
            CollectionName = this.Config.JsonSchema.Title;
            Db.AddCollection(CollectionName, this.Config.Identity);
        }

        string RemoveBrackets(string s)
        {
            var start = s.IndexOf("{");
            var end = s.IndexOf("}");
            if ((start != -1) && (end != -1))
                if (start < end)
                    s = s.Substring(0, start - 1) + s.Substring(end + 1);
            return s;
        }
        string PreparePath()
        {
            var s = this.Config.Uri.Replace("/", ".");
            s = RemoveBrackets(s);
            if (s[0] == '.')
                s = s.Substring(1);
            if (s[s.Length - 1] == '.')
                s = s.Substring(0, s.Length - 2);
            return s;
        }

        void Get(IRequestContext ctx)
        {
            var identity = "{" + IdentityFieldName + "}";
            if (ctx.InputParameters.Count > 0)
            {
                if (ctx.InputParameters.ContainsKey(identity))
                {
                    ctx.Response = this.Db.ObjectCollections[CollectionName].Get(ctx.InputParameters[identity]);
                    ctx.ResponseType = ResponseType.JSON;
                }
            }
            else
            {
                List<JObject> obj = new List<JObject>();
                foreach (var f in Directory.EnumerateFiles(SavePath, "*.json"))
                {
                    var o = JObject.Parse(System.IO.File.ReadAllText(f, Encoding.UTF8));
                    obj.Add(o);
                }
                ctx.ResponseType = ResponseType.JSON;
            }
        }

        async Task Delete(IRequestContext ctx)
        {
            await Task.Run(()=>
            {
                Db.ObjectCollections[CollectionName].Delete(ctx.InputParameters["{"+IdentityFieldName+"}"]);
            });
        }

        void GetAll(IRequestContext ctx)
        {

        }

        async Task Put(IRequestContext ctx)
        {
            await Task.Run(() =>
            {
                if (!String.IsNullOrEmpty(IdentityFieldName))
                {
                    ctx.Response = this.Db.ObjectCollections[CollectionName].Update(ctx.BodyString).ToString();
                    ctx.ResponseType = ResponseType.JSON;
                }
            });
        }

        async Task Post(IRequestContext ctx)
        {
            await Task.Run(() =>
            {
                if (!String.IsNullOrEmpty(IdentityFieldName))
                {
                    ctx.Response = this.Db.ObjectCollections[CollectionName].Insert(ctx.BodyString).ToString();
                    ctx.ResponseType = ResponseType.JSON;
                }
            });
        }

        long MaxIdentity()
        {
            var files = System.IO.Directory.GetFiles(SavePath);
            long max = 0;
            foreach (var f in files)
            {
                if (Int64.TryParse(System.IO.Path.GetFileName(f), out long res))
                    if (res > max)
                        max = res;
            }
            return max;
        }

        string GetNextIdentity()
        {
            lock (this)
            {
                var cnt = MaxIdentity();
                cnt++;
                return cnt.ToString();
            }
        }
    }

}