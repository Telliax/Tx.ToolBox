using System.Collections.ObjectModel;
using System.Linq;
using Tx.ToolBox.Wpf.Mvvm;
using Tx.ToolBox.Wpf.SampleApp.Manager;

namespace Tx.ToolBox.Wpf.SampleApp.List
{
    class SampleListViewModel : ViewModelBase
    {
        private readonly ISampleManager _manager;

        public SampleListViewModel(ISampleManager manager)
        {
            _manager = manager;
            Samples = new ObservableCollection<SampleViewModel>(manager.Samples.Select(s => new SampleViewModel(s)));
        }

        public ObservableCollection<SampleViewModel> Samples { get; }

        public SampleViewModel SelectedSample
        {
            set => _manager.ChangeSampleAsync(value?.Sample);
        }

        public class SampleViewModel : ViewModelBase
        {
            public SampleViewModel(ISample sample)
            {
                _sample = sample;
            }

            public ISample Sample => _sample;
            public string Name => _sample.Name;
            public string Desc => _sample.Description;

            private readonly ISample _sample;
        }
    }
}
