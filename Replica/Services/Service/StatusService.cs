using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using X.Services;
using Microsoft.Extensions.Logging;

namespace X.Services
{   
    public class StatusService:IStatusServices
    {
        public Dictionary<string, ServiceStatus> Status { get; set; } = new Dictionary<string, ServiceStatus>();
        public ILogger Log { get; private set; }
    }
} 
    