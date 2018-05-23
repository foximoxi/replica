using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using R.Public;
using Microsoft.Extensions.Logging;

namespace R.Config
{
    public interface IComponentBase
    {
        ILogger Log { get; set; }
    }
}