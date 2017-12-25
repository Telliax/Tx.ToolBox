using Tx.ToolBox.Wpf.Mvvm;

namespace Tx.ToolBox.Wpf.SampleApp.App.List
{
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