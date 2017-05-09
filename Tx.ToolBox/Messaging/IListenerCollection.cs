using System;

namespace Tx.ToolBox.Messaging
{
    interface IListenerCollection
    {
        IDisposable Add(object listener);
        void Handle(object message);
    }
}
