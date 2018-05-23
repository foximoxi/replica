using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using X.Config;
using X.Public;

namespace X.Services
{
    public class SingleCrudTest
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }

    /// <summary>
    /// request processing and response preparation
    /// </summary>
    public class RequestResponseService : IRequestResponseService
    {
        public ILogger Log { get; private set; }

        JsonSerializerSettings InitSerialization()
        {
            var SerializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateFormatString = "yyyy-MM-ddThh:mm:ssZ"
            };
#if DEBUG
            SerializerSettings.Formatting = Formatting.Indented;
#endif
            return SerializerSettings;
        }

        public IRequestContext PrepareRequest(object context, Dictionary<string, string> inputParams, HttpMethod method)
        {
            var rx = new RequestContext()
            {
                HttpContext = context,
                InputParameters = inputParams,
                User = null,
                HttpMethod = method,
                ThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId,
                //BodyStream = new System.IO.StreamReader().ReadToEnd(System.Text.Encoding.UTF8.GetString(((HttpContext)context).Request.Body.re
            };

            {//workfloroug - nie da sie przeczytac 2x request.body, wiec zrzucane jest do stringa i wszystkie uslugi nie pracuja na Request.Body
                var httpContext = (HttpContext)context;
                //var initialBody = httpContext.Request.Body;
                //httpContext.Request.EnableRewind();
                using (StreamReader reader = new StreamReader(httpContext.Request.Body))
                {
                    rx.BodyString = reader.ReadToEnd();
                    //httpContext.Request.Body.Position = 0;
                }
                //httpContext.Request.Body = initialBody; // Workaround
            }
            return rx;
        }

        public async Task Respond(X.Config.IRequestContext request)
        {
            var context = request.HttpContext as HttpContext;
            if (request.ResponseType == ResponseType.JSON)
                context.Response.ContentType = "application/json";
            var json = new X.Helpers.JsonHelper();

            var settings = InitSerialization();

            var s = JsonConvert.SerializeObject(request.Response, settings);
            await context.Response.WriteAsync(s);
            request.Status = RequestStatus.ResponseSend;
        }
    }
}