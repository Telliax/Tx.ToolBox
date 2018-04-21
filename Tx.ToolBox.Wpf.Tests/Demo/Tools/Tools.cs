using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Media;
using FontAwesome.WPF;
using Tx.ToolBox.Messaging;
using Tx.ToolBox.Wpf.SampleApp.App.Events;
using Tx.ToolBox.Wpf.Tools.Buttons;
using Tx.ToolBox.Wpf.Tools.Drop;
using Tx.ToolBox.Wpf.Tools.Text;
using Timer = System.Timers.Timer;

namespace Tx.ToolBox.Wpf.Tests.Demo.Tools
{
    abstract class DemoButton : ButtonTool
    {
        public IMessenger Messenger { protected get; set; }

        protected override void Execute()
        {
            Messenger.PublishAsync(new LogMessage($"'{ToolTip}' clicked!"));
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

    class DisabledButton : DemoButton
    {
        public DisabledButton()
        {
            Image = ImageAwesome.CreateImageSource(FontAwesomeIcon.Gears, Brushes.Black);
            ToolTip = "This button is disabled.";
            Text = "Disabled";
        }

        protected override bool CanExecute() => false;
    }

    class ToggleButton : ToggleButtonTool
    {
        public ToggleButton()
        {
            Image = ImageAwesome.CreateImageSource(FontAwesomeIcon.Table, Brushes.Black);
            ToolTip = "Image+Text ToggleButton";
            Text = "Toggle";
        }

        public IMessenger Messenger { protected get; set; }

        protected override void OnIsChekedChanged()
        {
            Messenger.PublishAsync(new LogMessage($"Toggle state changed: {IsChecked}!"));
        }
    }

    class AsyncButton : AsyncButtonTool
    {
        public AsyncButton()
        {
            ToolTip = "Async button";
            Text = "Async";
            Image = ImageAwesome.CreateImageSource(FontAwesomeIcon.Car, Brushes.Black);
        }

        public IMessenger Messenger { protected get; set; }

        protected override async Task ExecuteAsync(CancellationToken token)
        {
            await Messenger.PublishAsync(new LogMessage("Async action started!"));
            await Task.Delay(TimeSpan.FromSeconds(3), token);
            await Messenger.PublishAsync(new LogMessage("Async action completed!"));
        }
    }

    class Label : LabelTool, IDisposable
    {
        public Label()
        {
            ToolTip = "This is readonly text.";
            Update();
            _timer = new Timer();
            _timer.Interval = 2000;
            _timer.Elapsed += OnElapsed;
            _timer.Start();
        }

        public void Dispose()
        {
            _timer.Stop();
            _timer.Dispose();
        }

        private readonly Timer _timer;
        private uint _counter;

        private void Update()
        {
            Text = $"Timer: {++_counter}";
        }

        private void OnElapsed(object sender, ElapsedEventArgs e)
        {
            Update();
        }
    }

    class TextInput : TextTool<int>
    {
        public TextInput(IMessenger messenger)
        {
            _messenger = messenger;
            ToolTip = "Enter integer value and press 'Enter'.";
            Value = 100;
            Width = 50;
        }

        protected override (bool CanParse, int Value) ConvertBack(string text)
        {
            var res = int.TryParse(text, out var value);
            return (res, value);
        }

        protected override void OnValueChanged()
        {
            base.OnValueChanged();
            _messenger.PublishAsync(new LogMessage($"TextBox value has changed to {Value}!"));
        }

        private readonly IMessenger _messenger;
    }

    class IntComboBox : ComboBoxTool<int>
    {
        public IntComboBox(IMessenger messenger)
        {
            _messenger = messenger;
            ToolTip = "Enter integer value and press 'Enter'.";
            SetOptions(1,10,100);
            Width = 50;
        }

        protected override void OnSelectedItemChanged()
        {
            _messenger.PublishAsync(new LogMessage($"ComboBox value has changed to {SelectedItem}!"));
        }

        private readonly IMessenger _messenger;
    }

}
