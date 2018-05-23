using System;
using R.Services;

namespace R.Config
{
    public interface IDataConnection
    {
        R.Services.Plugins.DbConnector DataSource { get; set; }
        R.Config.DataSource Config { get; set; }
        R.Config.DataSource NewConfig { get; set; }
        bool IsDefault { get; set; }
        void Initialize();

        R.Services.IModelManager ModelManager { get; set; }
        R.Services.ITestDataGenerator TestDataGenerator { get; set; }
    }
}