using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Services
{
    public enum ServiceStatus
    {
        Initializing,
        Stopped,
        Running,
        Failed,
        Restarting
    }
}