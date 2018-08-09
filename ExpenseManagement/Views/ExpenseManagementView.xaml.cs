using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace ViewSwitchingNavigation.ExpenseManagement.Views
{
    /// <summary>
    /// Interaction logic for ExpenseManagementView.xaml
    /// </summary>
    [Export("ExpenseManagementView")]
    public partial class ExpenseManagementView : UserControl
    {
        public ExpenseManagementView()
        {
            InitializeComponent();
        }
    }
}
