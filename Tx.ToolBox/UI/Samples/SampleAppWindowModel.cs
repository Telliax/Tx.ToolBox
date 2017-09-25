using Tx.ToolBox.UI.Mvvm;

namespace Tx.ToolBox.UI.Samples
{
    public class SampleAppWindowModel : ViewModelBase
    {
        public SampleAppWindowModel(SampleManager samples, EventLog events)
        {
            Samples = samples;
            Events = events;
        }

        public SampleManager Samples { get; private set; }
        public EventLog Events { get; private set; }
    }
}
