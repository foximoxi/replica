using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R.Services
{
    public interface IStatusServices : IService
    {
        Dictionary<string, ServiceStatus> Status { get; set; }
    }
}