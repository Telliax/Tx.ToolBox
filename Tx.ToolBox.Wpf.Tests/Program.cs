using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tx.ToolBox.Wpf.SampleApp;
using Tx.ToolBox.Wpf.Tests.Demo;
using Tx.ToolBox.Wpf.Tests.Demo.Tools;

namespace Tx.ToolBox.Wpf.Tests
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            using (var boot = new SampleBootstrap(new AppSettings()))
            {
                boot.AddSamples(new ToolBarSample(), new DummySample());
                boot.Run();
            }
        }
    }
}
