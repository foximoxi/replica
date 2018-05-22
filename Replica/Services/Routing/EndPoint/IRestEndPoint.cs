using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.Public;

namespace X.Config
{
    public interface IRestEndPoint:IEndPoint
    {
        Task Invoke(IRequestContext context);
    }
}