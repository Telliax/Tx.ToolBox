using System.Windows;

namespace Tx.ToolBox.Wpf.SampleApp.App
{
    class SampleApplication : Application
    {
        public SampleApplication(SampleAppWindow window)
        {
            _window = window;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow = _window;
            MainWindow.ShowDialog();
        }
        private readonly SampleAppWindow _window;
    }
}
