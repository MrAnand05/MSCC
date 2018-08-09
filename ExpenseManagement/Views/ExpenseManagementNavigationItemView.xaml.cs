using Microsoft.Practices.Prism.Regions;
using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using ViewSwitchingNavigation.Infrastructure;

namespace ViewSwitchingNavigation.ExpenseManagement.Views
{
    /// <summary>
    /// Interaction logic for ExpenseManagementNavigationItemView.xaml
    /// </summary>
    [Export]
    [ViewSortHint("06")]
    public partial class ExpenseManagementNavigationItemView : UserControl, IPartImportsSatisfiedNotification
    {
        private static Uri ExpenseManagementViewUri = new Uri("/ExpenseManagementView", UriKind.Relative);
        [Import]
        public IRegionManager regionManager;
        public ExpenseManagementNavigationItemView()
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
            this.NavigateToExpenseManagementRadioButton.IsChecked = (uri == ExpenseManagementViewUri);
        }

        private void NavigateToExpenseManagementRadioButton_Click(object sender, RoutedEventArgs e)
        {
            this.regionManager.RequestNavigate(RegionNames.MainContentRegion, ExpenseManagementViewUri);
        }
    }
}