using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.Loader;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace R.Services
{
    public class CommandService : ICommandService
    {
        IFileWatchService FileWatchService { get; set; }
        IRoutingTableService RoutingTableService { get; set; }
        IStatusServices StatusServices { get; set; }
        public ILogger Log { get; private set; }
        public CommandService(IFileWatchService fileWatchService, IRoutingTableService routingTableService, IStatusServices statusServices)
        {
            FileWatchService = fileWatchService;
            RoutingTableService = routingTableService;
            StatusServices = statusServices;
            Init();
        }

        List<Command> Commands { get; set; }
        void Init()
        {
            Commands = new List<Command>();
            Commands.Add(new Command() { Name = "show.views", Func = ShowLoadedViews });
            Commands.Add(new Command() { Name = "show.endpoints", Func = ShowEndPoints });
            Commands.Add(new Command() { Name = "show.apprests", Func = AppRests });
            Commands.Add(new Command() { Name = "config.restart", Func = ConfigRestart });
            Commands.Add(new Command() { Name = "show.endpoints.csharp", RespondFunc = ShowEndPointsInCSharp });
        }

        public object ShowLoadedViews()
        {
            var res = new List<object>();
            return res;
        }

        public object ShowLoadedOperations()
        {
            var res = new List<object>();
            //res.AddRange(ComponentService.TypedInserts.Items.Values);
            return res;
        }
        public object ShowEndPoints()
        {
            return RoutingTableService.EndPoints.Select(x => new { uri = x.Uri, Method = x.Method.ToString() });
        }

        public void ShowEndPointsInCSharp(object ctx)
        {
            /*var context = ctx as HttpContext;
            var gen = new X.Helpers.RestMapCSharpGenerator();            
            var res=gen.Generate(ComponentService.AllOperations.Select(x => x.Uri));
            context.Response.WriteAsync(res);*/
        }


        object AppRests()
        {
            //var extractor = new X.Helpers.DefinitionExtractor();
            Dictionary<string, List<ExposedRest>> res = new Dictionary<string, List<ExposedRest>>();
            //ExtractCollectionRests(res, this.ComponentService.TypedViews.GetEnumerator());
            //ExtractCollectionRests(res, this.ComponentService.TypedInserts.GetEnumerator());
            //ExtractCollectionRests(res, this.ComponentService.TypedUpdates.GetEnumerator());
            //ExtractCollectionRests(res, this.ComponentService.TypedDeletes.GetEnumerator());
            //ExtractCollectionRests(res, this.ComponentService.CustomComponents.GetEnumerator());
            return res;
        }

        string GetAssemblyName(R.Config.IOperationDefinition definition)
        {
            var assemblyName = definition.ExposedType?.AssemblyQualifiedName;
            if (assemblyName == null)
            {
                if (definition is R.Config.CustomDefinition)
                {
                    var component = (R.Config.CustomDefinition)definition;
                    assemblyName = component.ComponentType.AssemblyQualifiedName;
                }
            }
            return assemblyName;
        }

        void ExtractCollectionRests(Dictionary<string, List<ExposedRest>> res, System.Collections.IEnumerator enu)
        {
            while (enu.MoveNext())
            {
                var c=enu.Current as R.Config.IOperationDefinition;
                var assemblyName = GetAssemblyName(c);
                if (res.ContainsKey(assemblyName) == false)
                    res[assemblyName] = new List<ExposedRest>();
                res[assemblyName].Add(new ExposedRest() { Uri = c.Uri, Op =c.Operation });
            }
        }

        void Respond(object context, object obj)
        {
            var ctx = context as HttpContext;
            R.Helpers.JsonHelper js = new Helpers.JsonHelper();
            ctx.Response.WriteAsync(js.SerializeObject(obj));
        }

        void ShowCmdMenu(object context)
        {            
            var ctx = context as HttpContext;
            var res = new StringBuilder();
            ctx.Response.ContentType = "text/html; charset=utf-8";            
            res.Append("<html><head><link href='https://www.w3schools.com/w3css/4/w3.css' rel='stylesheet'></head><body><div class='w3-white w3-padding notranslate'><table border='0'>");
            res.Append("<tr><th>Command</th><th>Description</td></tr>");
            foreach (var c in Commands)
            {
                res.Append("<tr><td><a href='http://"+ctx.Request.Host+"/_cmd/" + c.Name + "'>"+c.Name+"</a></td><td>"+c.Description+"</td>");
            }
            res.Append("</table></div></body></html>");
            ctx.Response.WriteAsync(res.ToString());
        }

        Assembly LoadAssembly(string assemblyPath)
        {
            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);
            return assembly;
        }

        object ConfigRestart()
        {
            FileWatchService.NotifyComponentChanges();return null;
        }

        
        public bool InvokeCommand(string path, object ctx)
        {
            var p = path.Replace("_cmd", "").Replace("/","").ToLower();
            var cmd=Commands.Where(x => x.Name.ToLower() == p).FirstOrDefault();
            if (cmd != null)
            {
                if (cmd.Func!=null)
                    Respond(ctx, cmd.Func.Invoke());                
                else
                    cmd.RespondFunc(ctx);
                return true;
            }
            else
            {
                if (path.ToLower().Replace("/", "").EndsWith("_cmd"))
                {
                    ShowCmdMenu(ctx);
                    return true;
                }
            }
            return false;
        }
    }

    public class Command
    {
        public string Name { get; set; }
        public Func<object> Func { get; set; }
        public Action<object> RespondFunc { get; set; }
        public string Description { get; set; }
    }

    class ExposedRest
    {
        public string Uri { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public R.Public.Op Op { get; set; }
    }
}