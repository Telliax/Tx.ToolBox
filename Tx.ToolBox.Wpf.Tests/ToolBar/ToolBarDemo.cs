using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using FontAwesome.WPF;
using NUnit.Framework;
using Tx.ToolBox.Wpf.Helpers;
using Tx.ToolBox.Wpf.Tools;
using Tx.ToolBox.Wpf.Tools.Buttons;

namespace Tx.ToolBox.Wpf.Tests.ToolBar
{
    [TestFixture, Apartment(ApartmentState.STA)]
    class ToolBarDemo
    {
        [Test]
        public void Run()
        {
            new TestApp().Run();
        }

        private class TestApp : Application
        {
            protected override void OnStartup(StartupEventArgs e)
            {
                base.OnStartup(e);
                var toolbar = new ToolBarViewModel();
                toolbar.Setup()
                       .Add(
                            new ImageButton(), 
                            new TextButton(),
                            new Button(),
                            new DisabledImageButton(),
                            new DisabledButton(),
                            new AsyncImageButton(),
                            new AsyncButton(),
                            new ToggleButton()
                            )
                       .Complete();
                MainWindow = new Window {Content = new ToolBarView {DataContext = toolbar}};
                MainWindow.ShowDialog();
            }
        }

        private class ImageButton : ButtonTool
        {
            public ImageButton()
            {
                Image = ImageAwesome.CreateImageSource(FontAwesomeIcon.AngleLeft, Brushes.Black);
                ToolTip = "Image only button";
            }

            protected override void Execute()
            {
                MessageBox.Show("Click!");
            }
        }

        private class TextButton : ButtonTool
        {
            public TextButton()
            {
                Text = "Text";
                ToolTip = "Text-only button";
            }

            protected override void Execute()
            {
                MessageBox.Show("Click!");
            }
        }

        private class Button : ButtonTool
        {
            public Button()
            {
                Text = "Text";
                Image = ImageAwesome.CreateImageSource(FontAwesomeIcon.Cab, Brushes.Black);
                ToolTip = "Button with image and text";
            }

            protected override void Execute()
            {
                MessageBox.Show("Click!");
            }
        }

        private class DisabledImageButton : ButtonTool
        {
            public DisabledImageButton()
            {
                Image = ImageAwesome.CreateImageSource(FontAwesomeIcon.Flag, Brushes.Black);
                ToolTip = "Disabled image button";
            }

            protected override void Execute()
            {
                throw new NotImplementedException();
            }

            protected override bool CanExecute()
            {
                return false;
            }
        }


        private class DisabledButton : ButtonTool
        {
            public DisabledButton()
            {
                Image = ImageAwesome.CreateImageSource(FontAwesomeIcon.Gamepad, Brushes.Black);
                ToolTip = "Disabled button with image and text";
                Text = "Text";
            }

            protected override void Execute()
            {
                throw new NotImplementedException();
            }

            protected override bool CanExecute()
            {
                return false;
            }
        }

        private class AsyncImageButton : AsyncButtonTool
        {
            public AsyncImageButton()
            {
                Image = ImageAwesome.CreateImageSource(FontAwesomeIcon.Android, Brushes.Black);
                ToolTip = "Async button";
            }

            protected override async Task ExecuteAsync(CancellationToken token)
            {
                await Task.Delay(3000, token);
                Application.Current.Dispatcher.BeginInvoke(() => MessageBox.Show("Done!"));
            }
        }

        private class AsyncButton : AsyncButtonTool
        {
            public AsyncButton()
            {
                Image = ImageAwesome.CreateImageSource(FontAwesomeIcon.HackerNews, Brushes.Black);
                Text = "Text";
                ToolTip = "Async button with text and image";
            }

            protected override async Task ExecuteAsync(CancellationToken token)
            {
                await Task.Delay(3000, token);
                Application.Current.Dispatcher.BeginInvoke(() => MessageBox.Show("Done!"));
            }
        }

        private class ToggleButton : ToggleButtonTool 
        {
            public ToggleButton()
            {
                Image = ImageAwesome.CreateImageSource(FontAwesomeIcon.Bomb, Brushes.Black);
                ToolTip = "Image only togglebutton";
            }

            protected override void OnIsChekedChanged()
            {
                MessageBox.Show(IsChecked ? "Enabled!" : "Disabled!");
            }
        }
    }
}
