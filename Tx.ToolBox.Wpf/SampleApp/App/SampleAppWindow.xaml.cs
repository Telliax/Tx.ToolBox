using System.Windows;
using Tx.ToolBox.Wpf.Tools;
using Tx.ToolBox.Wpf.SampleApp.App.Log;
using Tx.ToolBox.Wpf.SampleApp.App.Sample;
using Tx.ToolBox.Wpf.SampleApp.App.List;

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

        public ToolBarView ToolBar
        {
            set => ToolBarHost.Content = value;
        }

        public LoadingScreenView LoadingScreen
        {
            set => LoadingScreenHost.Content = value;
        }
    }
}
