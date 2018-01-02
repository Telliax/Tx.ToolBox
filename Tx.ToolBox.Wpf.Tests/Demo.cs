using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tx.ToolBox.Wpf.Tests
{
    public class Demo
    {
        [STAThread]
        public static void Main()
        {
            //new Samples.SamplesDemo().Run();
            new ToolBar.ToolBarDemo().Run();
        }
    }
}
