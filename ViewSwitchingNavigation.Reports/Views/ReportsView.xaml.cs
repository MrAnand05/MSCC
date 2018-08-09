using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace ViewSwitchingNavigation.Reports.Views
{
    /// <summary>
    /// Interaction logic for ReportsView.xaml
    /// </summary>
    [Export("ReportsView")]
    public partial class ReportsView : UserControl
    {
        public ReportsView()
        {
            InitializeComponent();
        }
    }
}
