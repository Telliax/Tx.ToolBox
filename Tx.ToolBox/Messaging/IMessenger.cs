using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tx.ToolBox.Messaging
{
    public interface IMessenger
    {
        IDisposable Subscribe(object listener);
        IDisposable Subscribe<TMessage>(Action<TMessage> handler)
            where TMessage : IMessage;

        void Publish(IMessage message);
        Task PublishAsync(IMessage message);
    }
}
