using System;
using X.Services;

namespace X.Config
{
    public interface IDataConnection
    {
        X.Services.Plugins.DbConnector DataSource { get; set; }
        X.Config.DataSource Config { get; set; }
        X.Config.DataSource NewConfig { get; set; }
        bool IsDefault { get; set; }
        void Initialize();

        X.Services.IModelManager ModelManager { get; set; }
        X.Services.ITestDataGenerator TestDataGenerator { get; set; }
    }
}