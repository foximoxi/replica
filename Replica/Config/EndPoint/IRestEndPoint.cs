using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using R.Public;

namespace R.Config
{
    public interface IRestEndPoint:IEndPoint
    {
        Task Invoke(IRequestContext context);
    }
}