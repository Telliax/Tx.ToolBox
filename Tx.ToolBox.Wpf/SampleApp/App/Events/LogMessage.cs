using System;
using Tx.ToolBox.Messaging;

namespace Tx.ToolBox.Wpf.SampleApp.App.Events
{
    class LogMessage : MessageBase
    {
        public LogMessage(string message, LogMessageType type = LogMessageType.Info)
        {
            Message = message;
            Type = type;
            TimeStamp = DateTime.Now;
        }

        public string Message { get; }
        public DateTime TimeStamp { get; }
        public LogMessageType Type { get; }
    }

    enum LogMessageType
    {
        Info,
        Error
    }
}
