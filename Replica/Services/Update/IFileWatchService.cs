using System;

namespace R.Services
{
    //basic storage plugin for various implementations/database connectors
    public interface IFileWatchService : IService
    {
        string WatchPath { get; }
        void Start(string watchPath);
        void NotifyConfigurationChange();
    }
}