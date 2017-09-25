namespace Tx.ToolBox.UI.Samples
{
    public interface IEventLog
    {
        void Post(string message, MessageType type = MessageType.Info);
        void Clear();
    }

    public enum MessageType
    {
        Info,
        Error
    }
}
