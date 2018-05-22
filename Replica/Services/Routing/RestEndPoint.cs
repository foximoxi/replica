using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using X.Public;

namespace X.Config
{
    public class RestEndPoint : EndPoint, IRestEndPoint
    {
        public async Task Invoke(IRequestContext context)
        {
            await Component.Invoke(context);
            return;
        }
    }
}