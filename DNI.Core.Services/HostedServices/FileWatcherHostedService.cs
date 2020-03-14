using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Services.HostedServices
{
    public class FileWatcherHostedService : IHostedService
    {
        private readonly FileSystemWatcher fileSystemWatcher;
        private readonly Timer watcherTimer;
        public FileWatcherHostedService()
        {
            fileSystemWatcher = new FileSystemWatcher();
            watcherTimer = new Timer(timerCallback)
        }

        private void timerCallback(object state)
        {
            fileSystemWatcher.WaitForChanged(WatcherChangeTypes.Created | WatcherChangeTypes.Deleted | WatcherChangeTypes.Changed);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
