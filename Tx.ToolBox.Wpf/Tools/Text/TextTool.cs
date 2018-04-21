using System;
using Tx.ToolBox.Helpers;
using Tx.ToolBox.Wpf.Templates;

namespace Tx.ToolBox.Wpf.Tools.Text
{
    [Template(typeof(TextToolView))]
    public abstract class TextTool : ToolBase
    {
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged();
                try
                {
                    OnTextChanged();
                }
                catch (Exception e)
                {
                    new AggregateException(e).RethrowOnThreadPool();
                }
            }
        }

        public double Width
        {
            get => _width;
            set => SetField(ref _width, value);
        }

        private string _text;
        private double _width = 100;

        protected abstract void OnTextChanged();

        protected override void SetState(object state)
        {
            Text = (string)state;
        }

        protected override object GetState()
        {
            return Text;
        }
    }

    public abstract class TextTool<TValue> : TextTool
    {
        public TValue Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged();

                OnValueChanged();
            }
        }

        protected virtual string Convert(TValue value)
        {
            return value.ToString();
        }

        protected abstract (bool CanParse, TValue Value) ConvertBack(string text);

        protected sealed override void OnTextChanged()
        {
            if (_updatingValue.IsSet) return;
  
            var result = ConvertBack(Text);
            if (result.CanParse)
            {
                using (_updatingValue.Set())
                {
                    Value = result.Value;
                }
            }
        }

        protected virtual void OnValueChanged()
        {
            if (_updatingValue.IsSet) return;

            using (_updatingValue.Set())
            {
                Text = Convert(Value);
            }
        }

        private readonly Flag _updatingValue = new Flag();
        private TValue _value;
    }

}
