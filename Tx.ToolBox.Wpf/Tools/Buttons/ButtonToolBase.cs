using System.Windows.Media;
using Tx.ToolBox.Wpf.Helpers;

namespace Tx.ToolBox.Wpf.Tools.Buttons
{
    public abstract class ButtonToolBase : ToolBase
    {
        public string Text
        {
            get => _text;
            set => SetField(ref _text, value);
        }

        public ImageSource Image
        {
            get => _image;
            set => SetField(ref _image, value.ToFrozen());
        }

        protected virtual bool CanExecute()
        {
            return true;
        }

        private ImageSource _image;
        private string _text;
    }
}
