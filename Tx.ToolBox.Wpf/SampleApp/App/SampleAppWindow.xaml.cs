using System.Windows;
using Tx.ToolBox.Wpf.SampleApp.List;
using Tx.ToolBox.Wpf.SampleApp.Log;
using Tx.ToolBox.Wpf.SampleApp.Sample;
using Tx.ToolBox.Wpf.ToolBar;

namespace Tx.ToolBox.Wpf.SampleApp.App
{
    /// <summary>
    /// Interaction logic for SampleAppWindow.xaml
    /// </summary>
    public partial class SampleAppWindow : Window
    {
        public SampleAppWindow()
        {
            InitializeComponent();
        }

        public ToolBarView ToolBar
        {
            set => ToolBarHost.Content = value;
        }

        public SelectedSampleView Sample
        {
            set => SampleHost.Content = value;
        }

        public SampleListView SampleList
        {
            set => SampleListHost.Content = value;
        }

        public EventLogView Events
        {
            set => LogHost.Content = value;
        }
    }
}
