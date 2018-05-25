using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using R.Services;

namespace R.Services
{
    public class FileWatchService : IFileWatchService
    {
        System.IO.FileSystemWatcher watcher;
        public string WatchPath { get; private set; }
        IStatusServices StatusService { get; set; }
        R.Services.IConfigurationUpdateService UpdateService { get; set; }
        public ILogger Log { get; private set; }

        public FileWatchService(R.Services.IConfigurationUpdateService updateSvc, IStatusServices statusSvc)
        {
            UpdateService = updateSvc;
            StatusService = statusSvc;
            statusSvc.Status[typeof(FileWatchService).Name] = ServiceStatus.Initializing;
        }

        public void Start(string watchPath)
        {
            this.WatchPath = watchPath;
            watcher = new FileSystemWatcher() { Path = WatchPath, IncludeSubdirectories=true };
            watcher.Changed += (sender, e) => { NotifyChangesHandler(sender, e); };
            watcher.Deleted += (sender, e) => { NotifyChangesHandler(sender, e); };
            watcher.EnableRaisingEvents = true;
            NotifySettingsChanges();
            NotifyComponentChanges();
            StatusService.Status[typeof(FileWatchService).Name] = ServiceStatus.Running;
        }

        DateTime lastNotifyTime = DateTime.MinValue;
        string lastPathChanged = "";
        void NotifyChangesHandler(object sender, FileSystemEventArgs e)
        {
            if ((lastNotifyTime.Subtract(DateTime.Now).Ticks != 0) || (lastPathChanged != e.FullPath))
            {
                lastPathChanged = e.FullPath;
                lastNotifyTime = DateTime.Now;
                if (lastPathChanged.EndsWith("replica.cfg", StringComparison.CurrentCultureIgnoreCase))
                    NotifySettingsChanges();
                else
                    NotifyComponentChanges();
            }
        }

        void NotifySettingsChanges()
        {
            UpdateService.UpdateSettings(new string[] { lastPathChanged });
        }

        public void NotifyComponentChanges()
        {
            UpdateService.UpdateConfiguration(new Update.UpdatePackage(GetAllFiles()));
        }

        ICollection<string> GetAllFiles()
        {
            var files = Directory.GetFiles(WatchPath, "*.*", SearchOption.AllDirectories).ToList();
            return files;
        }

        DateTime lastRefreshDateTime = DateTime.MinValue;
        public List<string> GetChangedFiles(string pattern = "*.*")
        {
            var files = Directory.GetFiles(WatchPath, pattern, SearchOption.AllDirectories).ToList();
            var changedFiles = files.Where(p => File.GetLastWriteTime(p) != lastRefreshDateTime).ToList();
            lastRefreshDateTime = DateTime.Now;
            return changedFiles;
        }
    }
}
