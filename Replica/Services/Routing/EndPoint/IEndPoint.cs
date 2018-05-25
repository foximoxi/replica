using System;
using R.Public;
using R.Component;

namespace R.Config
{
    public interface IEndPoint
    {
         IComponent Component { get; set; }
         string Uri { get; }
         HttpMethod Method { get; set; }
         //IResourceAccessFilter Security { get; set; }
    }
}