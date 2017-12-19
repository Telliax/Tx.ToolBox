using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Castle.Windsor;
using Tx.ToolBox.Messaging;
using Tx.ToolBox.Wpf.SampleApp.Messages;

namespace Tx.ToolBox.Wpf.SampleApp.Manager
{
    class SampleManager : ISampleManager, IDisposable
    {
        public SampleManager(IWindsorContainer container, ISample[] samples, IMessenger messenger)
        {
            _container = container;
            _messenger = messenger;
            Samples = samples;
        }

        public ISample SelectedSample { get; private set; }
        public IReadOnlyList<ISample> Samples { get; }

        public Task ChangeSampleAsync(ISample newSample)
        {
            lock (_lock)
            {
                if (!_disposed)
                {
                    _taskQueue = _taskQueue.ContinueWith(t =>
                    {
                        if (ReferenceEquals(SelectedSample, newSample)) return;
                        Unload();
                        Load(newSample);
                    });
                }
                return _taskQueue;
            }
        }

        public void Dispose()
        {
            lock (_taskQueue)
            {
                if (_disposed) return;
                _taskQueue.ContinueWith(t => Unload()).Wait();
                _disposed = true;
            }
        }

        private readonly IWindsorContainer _container;
        private readonly IMessenger _messenger;
        private Task _taskQueue = Task.CompletedTask;
        private readonly object _lock = new object();
        private bool _disposed;

        private void Unload()
        {
            if (SelectedSample == null) return;

            try
            {
                SelectedSample.Unload();
            }
            catch (Exception ex)
            {
                _messenger.PublishAsync(new LogMessage($"Failed to unload \"{SelectedSample.Name}\":\n{ex}", LogMessageType.Error));
            }
        }

        private void Load(ISample sample)
        {
            if (sample != null)
            {
                try
                {
                    sample.Load(_container);
                    _messenger.PublishAsync(new LogMessage($"\"{sample.Name}\" loaded."));
                }
                catch (Exception ex)
                {
                    _messenger.PublishAsync(new LogMessage($"Failed to load \"{sample.Name}\":\n{ex}", LogMessageType.Error));
                    sample = null;
                }
            }
            SelectedSample = sample;
            _messenger.PublishAsync(new SampleLoadedMessage(sample)).Wait();
        }
    }
}
