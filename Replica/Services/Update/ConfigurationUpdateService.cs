using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using R.Services;

namespace R.Services
{
    public class ConfigurationUpdateService : IConfigurationUpdateService
    {
        public ILogger Log { get; private set; }
        private IStatusServices StatusService { get; set; }
        private IRoutingTableService RoutingTableService { get; set; }

        public ConfigurationUpdateService(ILoggerFactory logger, IStatusServices statusSvc, IRoutingTableService routingTableService)
        {
            StatusService = statusSvc;
            RoutingTableService = routingTableService;

            statusSvc.Status[typeof(ComponentService).Name] = ServiceStatus.Running;
            Log = logger.CreateLogger(nameof(ComponentService));
        }

        #region publicMethods
        public void UpdateConfiguration(R.Config.Update.IUpdatePackage pkg)
        {
            try
            {
                ApplyConfiguration(pkg);
            }
            catch (Exception ex)
            {
                Log.LogWarning("----------------------Configuration update failed: {msg}", ex.Message);
#if DEBUG
                throw ex;
#endif
            }
        }

        public void ReleaseConfiguration()
        {
            //this.ComponentService.ReleaseConfiguration();
            //this.DataServices.ReleaseConfiguration();
            this.RoutingTableService.ReleaseConfiguration();
            System.GC.Collect();
        }
        #endregion

        object lockObj = new object();
        void ApplyConfiguration(R.Config.Update.IUpdatePackage pkg)
        {
            lock (lockObj)
            {
                Log.LogInformation("Begin of configuration update: " + DateTime.Now);
                if (pkg.Unpack())
                {
                    this.RoutingTableService.CompleteUpdate();
                    Log.LogInformation("Configuration update completed. No errors.");
                }
                else
                {
                    string msg = "";
                    Log.LogInformation($"Configuration update failed:{msg}");
                }
            }
        }

        public void UpdateSettings(ICollection<string> paths)
        {
            //SettingsService.Update(paths);
        }
    }
}