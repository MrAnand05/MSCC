// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.Prism.Regions;
using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using ViewSwitchingNavigation.Infrastructure;

namespace ViewSwitchingNavigation.Exam.Views
{
    [Export]
    [ViewSortHint("07")]
    public partial class ExamNavigationItemView : UserControl, IPartImportsSatisfiedNotification
    {
        private static Uri examViewUri = new Uri("/ExamView", UriKind.Relative);

        [Import]
        public IRegionManager regionManager;

        public ExamNavigationItemView()
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
            this.NavigateToExamRadioButton.IsChecked = (uri == examViewUri);
        }

        private void NavigateToExamRadioButton_Click(object sender, RoutedEventArgs e)
        {
            this.regionManager.RequestNavigate(RegionNames.MainContentRegion, examViewUri);
        }
    }
}
