using System;
using System.Windows;
using System.Windows.Threading;
using Tx.ToolBox.Messaging;
using Tx.ToolBox.Wpf.Mvvm;
using Tx.ToolBox.Wpf.SampleApp.Messages;

namespace Tx.ToolBox.Wpf.SampleApp.Sample
{
    class SelectedSampleViewModel : ViewModelBase, IListener<SampleLoadedMessage>
    {
        public SelectedSampleViewModel(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public SampleViewModel SelectedSample
        {
            get => _selectedSample;
            private set => SetField(ref _selectedSample, value);
        }

        void IListener<SampleLoadedMessage>.Handle(SampleLoadedMessage message)
        {
            _dispatcher.Invoke(() => SelectedSample = new SampleViewModel(message.Sample));
        }

        private readonly Dispatcher _dispatcher;
        private SampleViewModel _selectedSample;

        public class SampleViewModel : ViewModelBase
        {
            public SampleViewModel(ISample sample)
            {
                Name = sample.Name;
                View = sample.ResolveView();
            }

            public string Name { get; }
            public FrameworkElement View { get; }
        }
    }
}
