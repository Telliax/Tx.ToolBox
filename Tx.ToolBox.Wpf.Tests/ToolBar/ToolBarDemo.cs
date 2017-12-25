using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using FontAwesome.WPF;
using NUnit.Framework;
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
                       .Add(new Button(), new DisabledButton(), new AsyncButton())
                       .Complete();
                MainWindow = new Window {Content = new ToolBarView {DataContext = toolbar}};
                MainWindow.ShowDialog();
            }
        }

        private class Button : ButtonTool
        {
            public Button()
            {
                Image = ImageAwesome.CreateImageSource(FontAwesomeIcon.AngleLeft, Brushes.Black);
                ToolTip = "Button";
            }

            protected override void Execute()
            {
                MessageBox.Show("Click!");
            }
        }

        private class DisabledButton : ButtonTool
        {
            public DisabledButton()
            {
                Image = ImageAwesome.CreateImageSource(FontAwesomeIcon.Flag, Brushes.Black);
                ToolTip = "Disabled button";
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

        private class AsyncButton : AsyncButtonTool
        {
            public AsyncButton()
            {
                Image = ImageAwesome.CreateImageSource(FontAwesomeIcon.Android, Brushes.Black);
                ToolTip = "Async button";
            }

            protected override async Task ExecuteAsync(CancellationToken token)
            {
                await Task.Delay(3000, token);
                MessageBox.Show("Done!");
            }
        }
    }
}
