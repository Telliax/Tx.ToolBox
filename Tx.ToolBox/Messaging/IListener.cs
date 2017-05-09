using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tx.ToolBox.Messaging
{
    public interface IListener<in TMessage> where TMessage : IMessage
    {
        void Handle(TMessage message);
    }
}
