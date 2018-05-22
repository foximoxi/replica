using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Services.Plugins
{
    public interface DbConnector
    {
        void Close(bool dispose = true);
        void CreatePool(string connectionString);
        X.Services.Plugins.Db.IEngineServices Services { get; }
        X.Config.IConnectionPool Pool {get;}
    }
}
