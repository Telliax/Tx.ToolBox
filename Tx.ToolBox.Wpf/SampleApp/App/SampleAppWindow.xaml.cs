using System.Windows;
using Tx.ToolBox.Wpf.SampleApp.Log;
using Tx.ToolBox.Wpf.SampleApp.Samples;
using SampleListView = Tx.ToolBox.Wpf.SampleApp.Samples.SampleListView;

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
    }
}
