using System.ComponentModel.Composition;
using System.Windows.Controls;
using ViewSwitchingNavigation.Email.ViewModels;

namespace ViewSwitchingNavigation.Email.Views
{
    /// <summary>
    /// Interaction logic for RegisterStudentView.xaml
    /// </summary>
    public partial class PayFeeView : UserControl
    {
        public PayFeeView()
        {
            InitializeComponent();
        }
        [Import]
        public IPayFeeViewModel Model
        {
            get
            {
                return DataContext as IPayFeeViewModel;
            }
            set
            {
                DataContext = value;
            }
        }

    }
}
