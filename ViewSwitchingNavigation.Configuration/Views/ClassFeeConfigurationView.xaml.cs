using System.Windows;
using System.Windows.Controls;

namespace ViewSwitchingNavigation.Configuration.Views
{
    /// <summary>
    /// Interaction logic for ClassFeeConfigurationView.xaml
    /// </summary>
    public partial class ClassFeeConfigurationView : UserControl
    {
        public ClassFeeConfigurationView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            //PrintDialog dialog = new PrintDialog();
            //dialog.PrintVisual(ConfigurationList, "My Canvas");

            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog().GetValueOrDefault(false))
            {
                printDialog.PrintVisual(this, "Test");
            }
        }
    }
}
