using System;
using X.Public;

namespace X.Config
{
    public interface IEndPoint
    {
         IComponent Component { get; set; }
         string Uri { get; }
         HttpMethod Method { get; set; }
         //IResourceAccessFilter Security { get; set; }
    }
}