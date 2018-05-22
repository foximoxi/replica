﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using X.Services;

namespace X.Services
{
    public class FileWatchService : IFileWatchService
    {
        System.IO.FileSystemWatcher watcher;
        public string WatchPath { get; private set; }
        IStatusServices statusService;
        X.Services.IConfigurationUpdateService updateService;
        public ILogger Log { get; private set; }

        public FileWatchService(X.Services.IConfigurationUpdateService updateSvc, IStatusServices statusSvc)
        {
            updateService = updateSvc;
            statusService = statusSvc;
            statusSvc.Status[typeof(FileWatchService).Name] = ServiceStatus.Initializing;
        }

        public void Start(string watchPath)
        {
            this.WatchPath = watchPath;
            watcher = new FileSystemWatcher() { Path = WatchPath, IncludeSubdirectories=true };
            watcher.Changed += (sender, e) => { NotifyChangesHandler(sender, e); };
            watcher.Deleted += (sender, e) => { NotifyChangesHandler(sender, e); };
            watcher.EnableRaisingEvents = true;
            statusService.Status[typeof(FileWatchService).Name] = ServiceStatus.Running;
            NotifyAllSettingsChanges();
            NotifyConfigurationChange();
        }

        DateTime lastNotifyTime = DateTime.MinValue;
        string lastPathChanged = "";
        void NotifyChangesHandler(object sender, FileSystemEventArgs e)
        {
            if ((lastNotifyTime.Subtract(DateTime.Now).Ticks != 0) || (lastPathChanged != e.FullPath))
            {
                lastPathChanged = e.FullPath;
                lastNotifyTime = DateTime.Now;
                if (lastPathChanged.EndsWith(".settings", StringComparison.CurrentCultureIgnoreCase))
                    NotifySettingsChanges();
                else
                    NotifyConfigurationChange();
            }
        }

        public void NotifyConfigurationChange()
        {
            var pkg = new Update.UpdatePackage() { AllFiles = GetAllFiles() };
            pkg.RecognizedFiles = RecognizedFiles;
            updateService.UpdateConfiguration(pkg);
            RecognizedFiles = pkg.RecognizedFiles;
        }

        void NotifyAllSettingsChanges()
        {
            var settings=GetPackageFiles("*.settings");
            if (settings!=null)
                updateService.UpdateSettings(settings);
        }

        void NotifySettingsChanges()
        {
            updateService.UpdateSettings(new string[] { lastPathChanged });
        }

        Dictionary<string, X.Config.Update.FileType> RecognizedFiles { get; set; } = new Dictionary<string, Config.Update.FileType>();

        

        ICollection<string> GetAllFiles()
        {
            var files = Directory.GetFiles(WatchPath, "*.*", SearchOption.AllDirectories).ToList();
            return files;
        }

        public List<string> GetPackageFiles(string pattern)
        {
            var dataSourcesChanged = GetChangedFiles(pattern);
            if (dataSourcesChanged.Count > 0)
                return Directory.GetFiles(WatchPath, pattern, SearchOption.AllDirectories).ToList();
            return null;
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
