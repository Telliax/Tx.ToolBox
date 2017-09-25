using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Tx.ToolBox.UI.Samples
{
    public interface ISample
    {
        string Name { get; }
        string Description { get; }
        Visual View { get; }
        void Load(IWindsorContainer appContainer);
        void Unload();
    }
}
