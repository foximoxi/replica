using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using X.Public;
using Microsoft.Extensions.Logging;

namespace X.Config
{
    public interface IComponentBase
    {
        IConnectionPool ConnectionPool { get; set; }
        ILogger Log { get; set; }
    }
}