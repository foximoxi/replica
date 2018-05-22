using System;
using System.Collections.Generic;

namespace X.Config
{
    public class EndPointUri
    {
        string _uri;
        string _method="";
        public EndPointUri(string value)
        {
            if (value.StartsWith("/"))
                this._uri = value.Substring(1);
            else
                this._uri = value;
        }

        public EndPointUri(string value,string method)
        {
            if (value.StartsWith("/"))
                this._uri = value.Substring(1);
            else
                this._uri = value;
            _method = method;
        }
        public override string ToString()
        {
            return _uri;
        }

        public static implicit operator string(EndPointUri uri)
        {
            if (uri!=null)
                return uri.ToString();
            return null;
        }

        public static implicit operator EndPointUri(string d)
        {
            return new EndPointUri(d);
        }

        public string Address
        {
            get { return _uri; }
        }
        public string Method
        {
            get { return _method; }
        }
    }
}