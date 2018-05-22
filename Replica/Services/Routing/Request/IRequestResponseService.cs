using System;
using System.Collections.Generic;
using System.Text;
using X.Public;
using X.Config;
using System.Threading.Tasks;

namespace X.Services
{
    public interface IRequestResponseService : IService
    {
        IRequestContext PrepareRequest(object context, Dictionary<string, string> inputParams, HttpMethod method);
        Task Respond(IRequestContext request);
    }
}
