using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using R.Public;

namespace R.Config
{
    /// <summary>
    /// Interfejs profilu uzytkownika zwracanego z bazy
    /// </summary>
    public class RequestContext:IRequestContext
    {
        public R.Security.UserProfile User { get; set; }
        public object HttpContext { get; set; }
        public HttpMethod HttpMethod { get; set; }
        public Dictionary<string,string> InputParameters { get; set; }
        public RequestStatus Status { get; set; }
        public ResponseType ResponseType { get; set; }
        public int ThreadId { get; set; }
        public String BodyString { get; set; }

        object response;        
        public object Response
        {
            get
            {
                return response;
            }
            set
            {
                response = value;
                Status = RequestStatus.ResponsePrepared;
            }
        }

        public async void Respond()
        {
            var ctx = this.HttpContext as HttpContext;
            if (ResponseType == ResponseType.JSON)
            {
                var helper = new R.Helpers.JsonHelper();
                await ctx.Response.WriteAsync(helper.SerializeObject(ctx.Response));
            }
        }
    }
}
