using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using R.Config;
using R.Public;

namespace R.Services
{
    public class RoutingTableService : IRoutingTableService
    {
        private Dictionary<string, object> Parametrized { get; set; }
        private Dictionary<string, List<IRestEndPoint>> NonParametrized { get; set; } 
        public List<IEndPoint> EndPoints { get; set; }
        public ILogger Log { get; private set; }

        public void ReplaceEndPoints(ICollection<IEndPoint> endPoints)
        {
            ReleaseConfiguration();
            foreach (var e in endPoints)
            {
                if (e is IRestEndPoint)
                {
                    Register((IRestEndPoint)e);
                }
            }
        }

        void ReleaseConfiguration()
        {
            Parametrized  = new Dictionary<string, object>();
            NonParametrized  = new Dictionary<string, List<IRestEndPoint>>();
            EndPoints = new List<IEndPoint>();
        }

        public void CompleteUpdate()
        {
        }

        public void Register(IRestEndPoint endPoint)
        {
            if (!endPoint.Uri.Contains("{"))
            {
                if (!NonParametrized.ContainsKey(endPoint.Uri))
                    NonParametrized[endPoint.Uri] = new List<IRestEndPoint>();
                NonParametrized[endPoint.Uri].Add(endPoint);
            }
            else
            {
                var subPaths = endPoint.Uri.Split(new char[] { '/' });
                Register(endPoint, subPaths, 0, Parametrized);
            }
        }

        void Register(IRestEndPoint endPoint, string[] subPaths, int idx, Dictionary<string, object> point = null)
        {
            if (subPaths.Length > idx + 1)
            {
                if (!point.ContainsKey(subPaths[idx]))
                    point[subPaths[idx]] = new Dictionary<string, object>();
                Register(endPoint, subPaths, idx + 1, (Dictionary<string, object>)point[subPaths[idx]]);
            }
            else
            {
                if (point.ContainsKey(subPaths[idx])==false)
                    point[subPaths[idx]] = new Dictionary<string, IRestEndPoint>();
                var dict = point[subPaths[idx]] as Dictionary<string, IRestEndPoint>;
                dict[endPoint.Method.ToString()] = endPoint;
            }
        }

        public KeyValuePair<R.Config.IEndPoint, Dictionary<string, string>> Route(string uri, HttpMethod httpMethod)
        {
            if (NonParametrized.ContainsKey(uri))
            {
                var res = NonParametrized[uri].Where(x => x.Method == httpMethod).FirstOrDefault();
                return new KeyValuePair<IEndPoint, Dictionary<string, string>>(res, null);
            }
            else
            {
                string[] path = uri.Split(new char[] { '/' });
                var inputParameters = new Dictionary<string, string>();
                return new KeyValuePair<R.Config.IEndPoint, Dictionary<string, string>>(GetParametrized(path, 0, Parametrized, inputParameters, httpMethod), inputParameters);
            }
        }

        IEndPoint GetParametrized(string[] path, int idx, Dictionary<string, object> routeTable, Dictionary<string, string> inputParameters, HttpMethod httpMethod)
        {
            if (routeTable.ContainsKey(path[idx]))
                return GetParametrized(path, idx + 1, (Dictionary<string, object>)routeTable[path[idx]], inputParameters, httpMethod);
            else
            {
                var paramName = routeTable.Where(x => x.Key.StartsWith("{")).FirstOrDefault();
                if (paramName.Key != null)
                {
                    inputParameters[paramName.Key/*.Substring(1, paramName.Key.Length - 2)*/] = path[idx];
                    if (path.Length > idx + 1)
                        return GetParametrized(path, idx + 1, (Dictionary<string, object>)routeTable[paramName.Key], inputParameters, httpMethod);
                    else
                    {
                        var dict = routeTable[paramName.Key] as Dictionary<string, IRestEndPoint>;
                        return dict[httpMethod.ToString()];
                    }
                }
                else
                    return null;
            }
        }
    }
}
