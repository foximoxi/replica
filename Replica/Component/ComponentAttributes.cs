using System;
using System.Collections.Generic;

namespace R.Public
{
    //defines property field as subobject to fill during query
    public class SubViewAttribute : Attribute
    {
        public ResourceViewType ViewType { get; set; }
        public bool ValueRequired { get; set; }
        public SubViewAttribute(ResourceViewType viewType)
        {
            ViewType = viewType;
        }
    }

    public enum ResourceViewType
    {
        AsFullObject = 0,
        AsLocationUri = 1,
        AsId = 2
    }

    //defines class as RestView with Uri
    public interface IRestAttr
    {
        string Url { get; }
        string Method { get; }
    }
    public interface IRestAttrEx : IRestAttr
    {
        ResourceViewType ReturnViewType { get; }
    }

    public class RestGetAttribute : Attribute, IRestAttr
    {
        public string Url { get; private set; }
        public string Method { get; private set; }
        public RestGetAttribute(string url, string method = "GET")
        {
            Url = url;
            Method = method;
        }
    }

    public class RestPostAttribute : Attribute, IRestAttrEx
    {
        public string Url { get; private set; }
        public string Method { get; private set; }
        public ResourceViewType ReturnViewType { get; private set; }

        public RestPostAttribute(string url, ResourceViewType returnType, string method = "POST")
        {
            Url = url;
            ReturnViewType = returnType;
            Method = method;
        }
        public RestPostAttribute(string url, string method = "POST")
        {
            Url = url;
            Method = method;
        }
    }

    public class RestPutAttribute : Attribute, IRestAttrEx
    {
        public string Url { get; private set; }
        public string Method { get; private set; }
        public ResourceViewType ReturnViewType { get; private set; }

        public RestPutAttribute(string url, ResourceViewType returnType, string method = "PUT")
        {
            Url = url;
            ReturnViewType = returnType;
            Method = method;
        }

        public RestPutAttribute(string url, string method = "PUT")
        {
            Url = url;
            Method = method;
        }

    }

    public class RestDeleteAttribute : Attribute, IRestAttr
    {
        public string Url { get; private set; }
        public string Method { get; private set; }
        public RestDeleteAttribute(string url, string method = "DELETE")
        {
            Url = url;
            Method = method;
        }
    }

    public class RestPackAttribute : Attribute
    {
        public Op Operations { get; set; }
        public string Url { get; private set; }
        public string LocatorUrl { get; private set; }
        public string Method { get; private set; }
        public RestPackAttribute(string url)
        {
            Inner(url, Op.DELETE | Op.GET | Op.POST | Op.PUT | Op.GET_ONE);
        }
        public RestPackAttribute(string url, string locatorUrl = null, Op operations = Op.DELETE | Op.GET | Op.POST | Op.PUT | Op.GET_ONE)
        {
            Url = url;
            LocatorUrl = locatorUrl;
            Operations = operations;
        }

        public RestPackAttribute(string url, Op operations = Op.DELETE | Op.GET | Op.POST | Op.PUT | Op.GET_ONE)
        {
            Inner(url, operations);
        }

        void Inner(string url, Op operations)
        {
            Url = url;
            if (url.EndsWith("/"))
                LocatorUrl = url + "{id}";
            else
                LocatorUrl = url + "/{id}";
            Operations = operations;
        }

        public IEnumerable<string> LocatorParamNames()
        {
            var res = new List<string>();
            var startIdx = LocatorUrl.IndexOf("{");
            if (startIdx == -1)
                return res;

            while (startIdx != -1)
            {
                var endIdx = LocatorUrl.IndexOf("}", startIdx);
                if (endIdx != -1)
                    res.Add(LocatorUrl.Substring(startIdx + 1, endIdx - startIdx - 1));
                startIdx = LocatorUrl.IndexOf("{", endIdx);
            }
            return res;
        }
    }
    //returns path for single object location e.g. /objects/users/{id} or /objects/users/{name}
    public class RestLocatorAttribute : Attribute, IRestAttr
    {
        public string Url { get; private set; }
        public string Method { get; private set; }
        public RestLocatorAttribute(string url, string method = "GET")
        {
            Url = url;
            Method = method;
        }

        public IEnumerable<string> LocatorParamNames()
        {
            var res = new List<string>();
            var startIdx = Url.IndexOf("{");
            if (startIdx == -1)
                return res;

            while (startIdx != -1)
            {
                var endIdx = Url.IndexOf("}", startIdx);
                if (endIdx != -1)
                    res.Add(Url.Substring(startIdx + 1, endIdx - startIdx - 1));
                startIdx = Url.IndexOf("{", endIdx);
            }
            return res;
        }
    }

    [Flags]
    public enum Op
    {
        GET = 1, POST = 2, PUT = 4, DELETE = 8, GET_ONE = 16
    }

    public class DatabaseAttribute:Attribute
    {
        public string DatabaseName { get; private set; }
        public DatabaseAttribute(string name)
        {
            DatabaseName = name;
        }
    }
}