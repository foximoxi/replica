using System;
using System.Collections.Generic;
using System.Linq;
using R.Config;
using R.Public;
using Microsoft.Extensions.Logging;

namespace R.Services
{
    public class ComponentService : IComponentService
    {
        R.Services.IRoutingTableService RoutingTableService { get; set; }
        public ILogger Log { get; private set; }

        public List<R.Component.CustomRestComponent> Rest { get; set; }
        public List<R.Component.StaticResourceComponent> Static { get; set; }

        public ComponentService(IRoutingTableService routingTableService)
        {
            this.RoutingTableService = routingTableService;
        }

        public void Update(R.Config.Update.IUpdatePackage pkg)
        {
            foreach (var p in pkg.PackageFiles)
            {

            }
        }

        public void CompleteUpdate()
        {
        }

        public void ReleaseConfiguration()
        {
        }

        private R.Config.RestEndPoint CompleteCustomComponent(CustomDefinition v)
        {
            var endPoint = new R.Config.RestEndPoint() { Method = (HttpMethod)(int)v.Operation, Uri = v.Uri, Component = v.Component };
            if (!String.IsNullOrEmpty(v.Component.CustomUri))
            {
                endPoint.Uri = v.Component.CustomUri;
            }
            return endPoint;
        }
    }
}