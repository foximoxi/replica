using System;
using System.Collections.Generic;
using System.Text;
using X.Config;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace X.Services
{
    public class ReverseProxyService:IReverseProxyService
    {
        public ILogger Log { get; private set; }
    }
}