using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Regions;
using System.Windows.Controls;
using System;
using ViewSwitchingNavigation.Infrastructure;
using System.Windows;

namespace ViewSwitchingNavigation.Configuration.Views
{
    /// <summary>
    /// Interaction logic for ConfigurationNavigationItemView.xaml
    /// </summary>
    [Export]
    [ViewSortHint("05")]
    public partial class ConfigurationNavigationItemView : UserControl, IPartImportsSatisfiedNotification
    {
        private static Uri configurationViewUri = new Uri("/ConfigurationView", UriKind.Relative);
        [Import]
        public IRegionManager regionManager;
        public ConfigurationNavigationItemView()
        {
            InitializeComponent();
        }
        void IPartImportsSatisfiedNotification.OnImportsSatisfied()
        {
            IRegion mainContentRegion = this.regionManager.Regions[RegionNames.MainContentRegion];
            if (mainContentRegion != null && mainContentRegion.NavigationService != null)
            {
                mainContentRegion.NavigationService.Navigated += this.MainContentRegion_Navigated;
            }
        }
        public void MainContentRegion_Navigated(object sender, RegionNavigationEventArgs e)
        {
            this.UpdateNavigationButtonState(e.Uri);
        }
        private void UpdateNavigationButtonState(Uri uri)
        {
            this.NavigateToConfigurationRadioButton.IsChecked = (uri == configurationViewUri);
        }

        private void NavigateToConfigurationRadioButton_Click(object sender, RoutedEventArgs e)
        {
            this.regionManager.RequestNavigate(RegionNames.MainContentRegion, configurationViewUri);
        }
    }
}
