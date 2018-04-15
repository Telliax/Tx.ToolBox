using System.Xml.Serialization;
using Tx.ToolBox.Storage;
using Tx.ToolBox.Wpf.Mvvm;

namespace Tx.ToolBox.Wpf.Tools
{
    public abstract class ToolBase : ViewModel, ITool
    {
        protected ToolBase()
        {
            Id = GetType().FullName;
        }

        public string Id { get; protected set; }

        public string ToolTip
        {
            get => _toolTip;
            set => SetField(ref _toolTip, value);
        }

        public bool IsVisible
        {
            get => _isVisible;
            set => SetField(ref _isVisible, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetField(ref _isEnabled, value);
        }

        public virtual void SaveState(IStorage settingsStorage)
        {
            if (!UseSerialization) return;
            var state = new ToolState {Value = GetState()};
            settingsStorage.Set(state, Id);
        }

        public virtual void LoadState(IStorage settingsStorage)
        {
            if (!UseSerialization) return;
            if (settingsStorage.Contains<ToolState>(Id))
            {
                var state = settingsStorage.Get<ToolState>(Id);
                if (state.Value != null)
                {
                    SetState(state.Value);
                }
                else
                {
                    LoadDefaultState();
                }
            }
            else
            {
                LoadDefaultState();
            }
        }

        protected bool UseSerialization { get; set; }

        protected virtual void SetState(object state)
        {
        }

        protected virtual object GetState()
        {
            return -1;
        }

        protected virtual void LoadDefaultState()
        {
        }

        private string _toolTip;
        private bool _isVisible = true;
        private bool _isEnabled = true;
    }

    public class ToolState
    {
        [XmlElement("bool", typeof(bool))]
        [XmlElement("float", typeof(float))]
        [XmlElement("double", typeof(double))]
        [XmlElement("int", typeof(int))]
        [XmlElement("uint", typeof(uint))]
        [XmlElement("short", typeof(short))]
        [XmlElement("ushort", typeof(ushort))]
        [XmlElement("byte", typeof(byte))]
        [XmlElement("sbyte", typeof(sbyte))]
        [XmlElement("long", typeof(long))]
        [XmlElement("ulong", typeof(ulong))]
        [XmlElement("string", typeof(string))]
        public object Value { get; set; }
    }
}
