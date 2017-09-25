using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Tx.ToolBox.UI.Samples
{
    class SampleApp : Application
    {
        public SampleApp(SampleAppWindow window)
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
