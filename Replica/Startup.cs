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
            services.AddSingleton<X.Services.IFileWatchService, X.Services.FileWatchService>();
            services.AddSingleton<X.Services.IRoutingTableService, X.Services.RoutingTableService>();
            services.AddSingleton<X.Services.IRequestResponseService, X.Services.RequestResponseService>();
            services.AddSingleton<X.Services.ICommandService, X.Services.CommandService>();
            services.AddSingleton<X.Services.IConfigurationUpdateService, X.Services.ConfigurationUpdateService>();
            services.AddRouting();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, X.Services.IRoutingTableService routingTableService, X.Services.ICommandService cmdService, X.Services.IRequestResponseService responseService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            string path=System.IO.Directory.GetCurrentDirectory();

            app.UseExtendedRouter(routingTableService, cmdService, responseService);
        }
    }
}
