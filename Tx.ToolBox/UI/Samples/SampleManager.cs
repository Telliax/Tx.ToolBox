using Castle.Windsor;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Tx.ToolBox.UI.Mvvm;

namespace Tx.ToolBox.UI.Samples
{
    public class SampleManager : ViewModelBase
    {
        public SampleManager(IWindsorContainer container, ISample[] samples, IEventLog log)
        {
            _container = container;
            _log = log;
            Samples = new ObservableCollection<ISample>(samples);
            SelectedSample = samples.FirstOrDefault();
        }

        public ObservableCollection<ISample> Samples { get; private set; }

        public ISample SelectedSample
        {
            get { return _sample; }
            set
            {
                if (_sample == value) return;
                _log.Clear();
                if (_sample != null)
                {
                    try
                    {
                        _sample.Unload();
                    }
                    catch(Exception ex)
                    {
                        _log.Post($"Failed to unload \"{_sample.Name}\":\n{ex}", MessageType.Error);
                    }
                }
                _sample = value;
                if (_sample != null)
                {
                    try
                    {
                        _sample.Load(_container);
                        _log.Post($"\"{_sample.Name}\" loaded.");
                    }
                    catch (Exception ex)
                    {
                        _log.Post($"Failed to load \"{_sample.Name}\":\n{ex}", MessageType.Error);
                    }
                }
                OnPropertyChanged();
            }
        }

        private ISample _sample;
        private readonly IWindsorContainer _container;
        private readonly IEventLog _log;
    }
}
