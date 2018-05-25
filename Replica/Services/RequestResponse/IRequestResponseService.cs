using System;
using System.Collections.Generic;
using System.Text;
using R.Public;
using R.Config;
using System.Threading.Tasks;

namespace R.Services
{
    public interface IRequestResponseService : IService
    {
        IRequestContext PrepareRequest(object context, Dictionary<string, string> inputParams, HttpMethod method);
        Task Respond(IRequestContext request);
    }
}
