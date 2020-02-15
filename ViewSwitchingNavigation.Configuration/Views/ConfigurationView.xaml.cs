using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace ViewSwitchingNavigation.Configuration.Views
{
    /// <summary>
    /// Interaction logic for ConfigurationView.xaml
    /// </summary>
    [Export("ConfigurationView")]
    public partial class ConfigurationView : UserControl
    {
        public ConfigurationView()
        {
            InitializeComponent();
        }
    }
}
