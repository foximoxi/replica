using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Replica
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<R.Services.IStatusServices, R.Services.StatusService>();
            services.AddSingleton<R.Services.ICommandService, R.Services.CommandService>();
            services.AddSingleton<R.Services.IFileWatchService, R.Services.FileWatchService>();
            services.AddSingleton<R.Services.IRoutingTableService, R.Services.RoutingTableService>();
            services.AddSingleton<R.Services.IConfigurationService, R.Services.ConfigurationService>();
            services.AddSingleton<R.Services.IRequestResponseService, R.Services.RequestResponseService>();
            services.AddSingleton<R.Services.IConfigurationUpdateService, R.Services.ConfigurationUpdateService>();
            services.AddRouting();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, R.Services.IRoutingTableService routingTableService, R.Services.ICommandService cmdService, R.Services.IRequestResponseService responseService)
        {
            //app.ApplicationServices.GetService<X.Services.ISettingsService>().Start(cfg["configPath"]);
            app.ApplicationServices.GetService<R.Services.IFileWatchService>().Start("c:\\rest");
            app.UseExtendedRouter(routingTableService, cmdService, responseService);
        }
    }
}
