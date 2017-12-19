using Tx.ToolBox.Messaging;

namespace Tx.ToolBox.Wpf.SampleApp.Messages
{
    class SampleLoadedMessage : MessageBase
    {
        public SampleLoadedMessage(ISample sample)
        {
            Sample = sample;
        }

        public ISample Sample { get; private set; }
    }
}
