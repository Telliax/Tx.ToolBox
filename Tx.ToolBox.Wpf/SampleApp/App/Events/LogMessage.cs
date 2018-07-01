using System;
using Tx.ToolBox.Messaging;

namespace Tx.ToolBox.Wpf.SampleApp.App.Events
{
    public class LogMessage : MessageBase
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

        public static implicit operator LogMessage(string message)
        {
            return new LogMessage(message);
        }
    }

    public enum LogMessageType
    {
        Info,
        Error
    }
}
