using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tx.ToolBox.Wpf.SampleApp.Manager
{
    interface ISampleManager
    {
        ISample SelectedSample { get; }
        IReadOnlyList<ISample> Samples { get; }

        Task ChangeSampleAsync(ISample newSample);
    }
}
