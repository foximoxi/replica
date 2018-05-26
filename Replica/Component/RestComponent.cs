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
    public class RestComponent : IComponent
    {
        public R.Config.EndPointUri CustomUri { get; }
        public ILogger Log { get; set; }
        public IComponentConfig Configuration { get; set; }

        public virtual void Init()
        {
        }

        public async virtual Task Invoke(IRequestContext ctx)
        {
            await Task.Run(() => { ctx.Response = "{\"msg\":\"Something went wrong during development stage. It's empty endpoint!\"}"; });
            return;
        }

        protected async void ReturnErrorMessage(RequestStatus stage, HttpContext context, Exception ex)
        {
            //var err = new ErrorResponder();
            //var errMsg = err.PrepareErrorMessage(ex, stage);
            //context.Response.StatusCode = errMsg.StatusCode;
            //await context.Response.WriteAsync(new Helpers.JsonHelper().SerializeObject(errMsg));
            await context.Response.WriteAsync("hello");
        }

        protected async void ReturnErrorMessage(RequestStatus stage, HttpContext context)
        {
            //var err = new ErrorResponder();
            //var errMsg = err.PrepareErrorMessage(validationResult, stage);
            //context.Response.StatusCode = errMsg.StatusCode;
            //await context.Response.WriteAsync(new Helpers.JsonHelper().SerializeObject(errMsg));
            await context.Response.WriteAsync("hello");
        }

        protected object Deserialize(IRequestContext ctx, Type type)
        {
            R.Helpers.JsonHelper s = new R.Helpers.JsonHelper();
            var item = s.DeserializeObject(ctx.BodyString, type);
            return item;
        }

        public static string SHA1(string randomString)
        {
            var crypt = new System.Security.Cryptography.SHA1Managed();//.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(System.Text.Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
    }
}