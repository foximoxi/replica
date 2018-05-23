using System;
using System.Collections.Generic;
using System.Text;
using R.Public;
using R.Config;

namespace R.Services
{
    public interface IRoutingTableService : IService
    {
        void Register(R.Config.IRestEndPoint endPoint);
        KeyValuePair<R.Config.IEndPoint, Dictionary<string, string>> Route(string uri, HttpMethod restMethod);
        void CompleteUpdate();
        void ReleaseConfiguration();
        void ReplaceEndPoints(ICollection<IEndPoint> endPoint);

        List<IEndPoint> EndPoints { get; set; }
    }
}
