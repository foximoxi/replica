using System;
using System.Collections.Generic;
using System.Text;
using X.Public;
using X.Config;

namespace X.Services
{
    public interface IRoutingTableService : IService
    {
        void Register(X.Config.IRestEndPoint endPoint);
        KeyValuePair<X.Config.IEndPoint, Dictionary<string, string>> Route(string uri, HttpMethod restMethod);
        void CompleteUpdate();
        void ReleaseConfiguration();
        void ReplaceEndPoints(ICollection<IEndPoint> endPoint);

        List<IEndPoint> EndPoints { get; set; }
    }
}
