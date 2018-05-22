using System;
using X.Services;
using System.Threading.Tasks;
using System.Data;

namespace X.Config
{
    public interface IConnectionPool
    {
        IDbConnection AcquireConnection();
    }
}