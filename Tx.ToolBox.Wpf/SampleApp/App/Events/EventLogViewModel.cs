using System.Collections.ObjectModel;
using System.Windows.Threading;
using Tx.ToolBox.Messaging;
using Tx.ToolBox.Wpf.Mvvm;

namespace Tx.ToolBox.Wpf.SampleApp.App.Log
{
    class EventLogViewModel : ViewModelBase, IListener<LogMessage>
    {
        public EventLogViewModel(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public ObservableCollection<MessageViewModel> Events { get; } = new ObservableCollection<MessageViewModel>();

        void IListener<LogMessage>.Handle(LogMessage message)
        {
            var vm = new MessageViewModel(message);
            _dispatcher.BeginInvoke(() =>
            {
                Events.Add(vm);
                if (Events.Count > MaxSize)
                {
                    Events.RemoveAt(0);
                }
            });
        }

        private const int MaxSize = 30;
        private readonly Dispatcher _dispatcher;

        public class MessageViewModel : ViewModelBase
        {
            public MessageViewModel(LogMessage message)
            {
                _message = message;
            }

            public bool IsError => _message.Type == LogMessageType.Error;

            public override string ToString()
            {
                return $"[{_message.TimeStamp:T}][{_message.Type}] {_message.Message}";
            }

            private readonly LogMessage _message;
        }
    }
}
