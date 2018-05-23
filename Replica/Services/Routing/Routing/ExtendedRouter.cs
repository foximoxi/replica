using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using R.Config;
using R.Public;

namespace Replica
{
    public static class ExtendedRouter
    {
        static R.Services.IRoutingTableService RoutingTable { get; set; }
        static R.Services.ICommandService CommandService { get; set; }
        static R.Services.IRequestResponseService RequestResponseService { get; set; }

        public static IApplicationBuilder UseExtendedRouter(this IApplicationBuilder app, R.Services.IRoutingTableService routingTable, R.Services.ICommandService commandService, R.Services.IRequestResponseService responseService)
        {
            CommandService = commandService;
            RoutingTable = routingTable;
            RequestResponseService = responseService;
            return Init(app);
        }

        static RequestDelegate DelegateHandler(HttpMethod mth)
        {
            RequestDelegate requestDelegate = async context =>
            {
                try
                {
                    var endPoint = RoutingTable.Route(ClearPath(context.Request.Path), mth);
                    if (endPoint.Key is RestEndPoint)
                    {
                        var r = endPoint.Key as IRestEndPoint;
                        var req = RequestResponseService.PrepareRequest(context, endPoint.Value, mth);
                        if (req.Status == RequestStatus.ReadyToProcess)
                        {
                            await r.Invoke(req);
                            if (req.Status == RequestStatus.ResponsePrepared)
                                await RequestResponseService.Respond(req);
                        }
                    }
                    else
                    {
                        if (InvokeCommand(context) == false)
                        {
                            context.Response.StatusCode = 404;
                            await context.Response.WriteAsync("404 NOT FOUND");
                        }
                    }
                }
                catch (Exception ex)
                {
#if DEBUG
                    await context.Response.WriteAsync(ex.ToString());
#endif
                }
            };
            return requestDelegate;
        }

        static IApplicationBuilder Init(IApplicationBuilder app)
        {
            var defaultRouteHandler = new RouteHandler(context =>
            {
                var routeValues = context.GetRouteData().Values;
                return context.Response.WriteAsync("");
            });

            var routeBuilder = new RouteBuilder(app, defaultRouteHandler);

            for (int i = 1; i < 7; i++)
                routeBuilder.MapGet(BuildPath(i), DelegateHandler(HttpMethod.GET));
            for (int i = 1; i < 7; i++)
                routeBuilder.MapPost(BuildPath(i), DelegateHandler(HttpMethod.POST));
            for (int i = 1; i < 7; i++)
                routeBuilder.MapPut(BuildPath(i), DelegateHandler(HttpMethod.PUT));
            for (int i = 1; i < 7; i++)
                routeBuilder.MapDelete(BuildPath(i), DelegateHandler(HttpMethod.DELETE));

            RoutingTable.CompleteUpdate();

            app.UseRouter(routeBuilder.Build());
            return app;
        }

        static void RegisterEndPoint(RestEndPoint endPoint)
        {
            RoutingTable.Register(endPoint);
        }

        static string ClearPath(string path)
        {
            var ret = path.ToString()?.Substring(1);
            if (ret.EndsWith("/"))
                return ret.Substring(0, ret.Length - 1);
            return ret;
        }

        static bool InvokeCommand(HttpContext ctx)
        {
            var path = ctx.Request.Path.ToString()?.Substring(1);
            return CommandService.InvokeCommand(path, ctx);
        }

        public static string BuildPath(int i)
        {
            string path = "";
            for (int x = 1; x <= i; x++)
            {
                path += "/{p" + x + "}";
            }
            return path.Substring(1);
        }
    }
}
