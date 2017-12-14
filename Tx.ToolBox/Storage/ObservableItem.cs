using System;
using System.Collections.Generic;
using Tx.ToolBox.Helpers;

namespace Tx.ToolBox.Storage
{
    public class ObservableItem<TItem> : IItemContainer, IObservable<TItem>
        where TItem : class, new()
    {
        public ObservableItem(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Null, empty string or whitpaces cannot be used as Id");
            Id = id;
        }

        public ObservableItem(TItem item, string id) : this(id)
        {
            Item = item;
        }

        public string Id { get; }
        public TItem Item { get; private set; }
        object IItemContainer.Item => Item;
        public Type ItemType => typeof(TItem);

        public void Set(object item)
        {
            Item = (TItem)item;
            lock (_observers)
            {
                _observers.ForEach(o => o.OnNext(Item));
            }
        }

        public IDisposable Subscribe(IObserver<TItem> observer)
        {
            observer.OnNext(Item);
            lock (_observers)
            {
                _observers.Add(observer);
                return new Action(() =>
                {
                    lock (_observers)
                    {
                        _observers.Remove(observer);
                    }
                }).AsDisposable();
            }
        }

        public void CopyTo(IDataStorage otherStorage)
        {
            otherStorage.Set(Item, Id);
        }

        public void Dispose()
        {
            lock (_observers)
            {
                _observers.ForEach(o => o.OnCompleted());
            }
        }

        private readonly List<IObserver<TItem>> _observers = new List<IObserver<TItem>>();
    }
}
