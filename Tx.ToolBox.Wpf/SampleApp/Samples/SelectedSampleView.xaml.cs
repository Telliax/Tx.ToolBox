using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tx.ToolBox.Wpf.ToolBar;

namespace Tx.ToolBox.Wpf.SampleApp.Samples
{
    /// <summary>
    /// Interaction logic for SelectedSampleView.xaml
    /// </summary>
    public partial class SelectedSampleView : UserControl
    {
        public SelectedSampleView()
        {
            InitializeComponent();
        }

        public ToolBarView ToolBar
        {
            set => ToolBarHost.Content = value;
        }
    }
}
