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
            _scheduler = new RunLatestScheduler();
        }

        public bool IsLoading => _loadingFlag.IsSet; 

        public IReadOnlyList<SampleViewModel> Samples { get; }

        public SampleViewModel SelectedSample
        {
            get => _selectedSample;
            set => ChangeSampleAsync(value);
        }

        public void Dispose()
        {
            _scheduler.Dispose();
            SelectedSample?.Sample.Unload();
        }

        private readonly IWindsorContainer _container;
        private readonly IMessenger _messenger;
        private readonly Flag _loadingFlag = new Flag();
        private readonly RunLatestScheduler _scheduler;
        private SampleViewModel _selectedSample;

        private async void ChangeSampleAsync(SampleViewModel sample)
        {
            using (_loadingFlag.Set())
            {
                OnPropertyChanged(nameof(IsLoading));
                await Task.Factory.StartNew(ChangeSample, CancellationToken.None, TaskCreationOptions.None, _scheduler);
            }
            OnPropertyChanged(nameof(IsLoading));

            void ChangeSample()
            {
                Unload();
                Load(sample);
            }
        }

        private void Unload()
        {
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

        private void Load(SampleViewModel sample)
        {
            if (sample != null)
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
