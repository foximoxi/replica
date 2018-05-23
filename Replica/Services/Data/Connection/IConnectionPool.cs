using System;
using R.Services;
using System.Threading.Tasks;
using System.Data;

namespace R.Config
{
    public interface IConnectionPool
    {
        IDbConnection AcquireConnection();
    }
}