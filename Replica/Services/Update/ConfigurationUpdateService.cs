using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using R.Config;
using R.Component;

namespace R.Services
{
    public class ConfigurationUpdateService : IConfigurationUpdateService
    {
        public ILogger Log { get; private set; }
        private IStatusServices StatusService { get; set; }
        private IRoutingTableService RoutingTableService { get; set; }

        public DateTime LastUpdateTime { get; private set; } = DateTime.MinValue;
        public ConfigurationUpdateService(ILoggerFactory logger, IStatusServices statusSvc, IRoutingTableService routingTableService)
        {
            StatusService = statusSvc;
            RoutingTableService = routingTableService;
            statusSvc.Status[typeof(ConfigurationUpdateService).Name] = ServiceStatus.Running;
            Log = logger.CreateLogger(nameof(ConfigurationUpdateService));
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
                if (pkg.Unpack(this.LastUpdateTime))
                {
                    RoutingTableService.ReleaseConfiguration();
                    RoutingTableService.ReplaceEndPoints(EndPoints(pkg));
                    Log.LogInformation("Configuration update completed. No errors.");
                    LastUpdateTime = DateTime.Now;
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

        List<IEndPoint> EndPoints(R.Config.Update.IUpdatePackage pkg)
        {
            ComponentFactory factory = new ComponentFactory();
            List<IEndPoint> res = new List<IEndPoint>();
            foreach (var p in pkg.PackageFiles)
            {
                new RestEndPoint() { Uri = p.Config.Uri, Component = factory.Create(p.Config), IsParametrized = true, Method = Public.HttpMethod.GET };
            }
            return null;
        }

        bool IsParametrized(PackageFile p)
        {
            return p.Config.Uri.Contains("{");
        }
    }
}