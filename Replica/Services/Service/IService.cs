using System;
using System.Collections.Generic;
using System.Text;
using R.Config;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace R.Services
{
    public interface IService
    {
        ILogger Log { get; }
    }
}