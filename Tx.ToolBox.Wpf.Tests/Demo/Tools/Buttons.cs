using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using FontAwesome.WPF;
using Tx.ToolBox.Messaging;
using Tx.ToolBox.Wpf.SampleApp.App.Events;
using Tx.ToolBox.Wpf.Tools.Buttons;

namespace Tx.ToolBox.Wpf.Tests.Demo.Tools
{
    abstract class DemoButton : ButtonTool
    {
        public IMessenger Messenger { protected get; set; }

        protected override void Execute()
        {
            Messenger.PublishAsync(new LogMessage($"{ToolTip} clicked!"));
        }
    }

    class ImageButton : DemoButton
    {
        public ImageButton()
        {
            Image = ImageAwesome.CreateImageSource(FontAwesomeIcon.AddressBook, Brushes.Black);
            ToolTip = "Image Button";
        }
    }

    class TextButton : DemoButton
    {
        public TextButton()
        {
            ToolTip = "Text Button";
            Text = "Text";
        }
    }

    class ImageAndTextButton : DemoButton
    {
        public ImageAndTextButton()
        {
            Image = ImageAwesome.CreateImageSource(FontAwesomeIcon.FacebookSquare, Brushes.Black);
            ToolTip = "Image+Text Button";
            Text = "Image+Text";
        }
    }
}
