using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using R.Public;
using Microsoft.Extensions.Logging;

namespace R.Config
{
    public interface IComponent
    {
        ILogger Log { get; set; }
        Task Invoke(IRequestContext context);
        R.Config.EndPointUri CustomUri { get; }
        void Start();
    }
}