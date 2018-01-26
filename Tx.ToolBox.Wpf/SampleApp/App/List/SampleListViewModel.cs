using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Castle.Windsor;
using Tx.ToolBox.Helpers;
using Tx.ToolBox.Messaging;
using Tx.ToolBox.Threading;
using Tx.ToolBox.Wpf.Mvvm;
using Tx.ToolBox.Wpf.SampleApp.App.Events;

namespace Tx.ToolBox.Wpf.SampleApp.App.List
{
    class SampleListViewModel : ViewModel, IDisposable
    {
        public SampleListViewModel(IWindsorContainer container, ISample[] samples, IMessenger messenger)
        {
            _container = container;
            _messenger = messenger;
            Samples = new ObservableCollection<SampleViewModel>(samples.Select(s => new SampleViewModel(s)));
            _queue = new ProcessLatestQueue();
        }

        public bool IsLoading => _loadingFlag.IsSet; 

        public IReadOnlyList<SampleViewModel> Samples { get; }

        public SampleViewModel SelectedSample
        {
            get { return _selectedSample; }
            set
            {
                var handle = _loadingFlag.SetTemporary();
                OnPropertyChanged(nameof(IsLoading));       
                _queue.Post(t => ChangeSample(value, t))
                      .ContinueWith(t =>
                                    {
                                        handle.Dispose();
                                        OnPropertyChanged(nameof(IsLoading));
                                    }, 
                                    TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        public void Dispose()
        {
            _queue.Dispose();
            SelectedSample?.Sample.Unload();
        }

        private readonly IWindsorContainer _container;
        private readonly IMessenger _messenger;
        private readonly Flag _loadingFlag = new Flag();
        private readonly ProcessLatestQueue _queue;
        private SampleViewModel _selectedSample;

        private void ChangeSample(SampleViewModel sample, CancellationToken cancellationToken)
        {
            Unload(cancellationToken);
            Load(sample, cancellationToken);
        }

        private void Unload(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return;
            var oldSample = _selectedSample?.Sample;
            if (oldSample == null) return;
            _selectedSample = null;
            try
            {
                oldSample.Unload();
            }
            catch (Exception ex)
            {
                _messenger.PublishAsync(new LogMessage($"Failed to unload \"{oldSample.Name}\":\n{ex}", LogMessageType.Error));
            }
        }

        private void Load(SampleViewModel sample, CancellationToken cancellationToken)
        {
            if (sample != null && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    sample.Sample.Load(_container);
                    _messenger.PublishAsync(new LogMessage($"\"{sample.Name}\" loaded."));
                }
                catch (Exception ex)
                {
                    _messenger.PublishAsync(new LogMessage($"Failed to load \"{sample.Name}\":\n{ex}", LogMessageType.Error));
                }
            }
            SetField(ref _selectedSample, sample, nameof(SelectedSample));
        }
    }
}
