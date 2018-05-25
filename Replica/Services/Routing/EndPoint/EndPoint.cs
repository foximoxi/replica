using System;
using R.Public;
using R.Component;

namespace R.Config
{
    public class EndPoint:IEndPoint
    {
        public IComponent Component { get; set; }
        public bool IsParametrized { get; set; }
        public HttpMethod Method { get; set; }
        public R.Config.Filters.Security.IResourceAccessFilter Security { get; set; }
        string uri;
        public string Uri
        {
            get
            { return uri; }
            set
            {
                uri = value;
                IsParametrized = uri.Contains("{");
            }
        }
    }
}