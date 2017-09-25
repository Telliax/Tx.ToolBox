using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using Tx.ToolBox.UI.Mvvm;
using Tx.ToolBox.Helpers;

namespace Tx.ToolBox.UI.Samples
{
    public class EventLog : ViewModelBase, IEventLog
    {
        public EventLog(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public ObservableCollection<Event> Events { get; } = new ObservableCollection<Event>();

        public void Post(string message, MessageType type)
        {
            var ev = new Event(message, type);
            _dispatcher.BeginInvoke(() =>
            {
                Events.Add(ev);
                if (Events.Count > MaxSize)
                {
                    Events.RemoveAt(0);
                }
            });
        }

        public void Clear()
        {
            _dispatcher.BeginInvoke(Events.Clear);
        }

        private readonly Dispatcher _dispatcher;
        private const int MaxSize = 30;

        public class Event : ViewModelBase
        {
            public Event(string message, MessageType type)
            {
                Message = message;
                Type = type;
                TimeStamp = DateTime.Now;
            }

            public string Message { get; private set; }
            public DateTime TimeStamp { get; private set; }
            public MessageType Type { get; private set; }
        }
    }

}
