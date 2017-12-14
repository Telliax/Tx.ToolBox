using System;

namespace Tx.ToolBox.Storage
{
    public interface IItemContainer : IDisposable
    {
        string Id { get; }
        object Item { get; }
        Type ItemType { get; }
        void Set(object item);
        void CopyTo(IDataStorage otherStorage);
    }
}
