using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R.Services.Plugins
{
    public interface DbConnector
    {
        void Close(bool dispose = true);
        void CreatePool(string connectionString);
        R.Services.Plugins.Db.IEngineServices Services { get; }
        R.Config.IConnectionPool Pool {get;}
    }
}
