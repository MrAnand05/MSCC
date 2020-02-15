// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using System.ComponentModel.Composition;
using ViewSwitchingNavigation.Infrastructure;
using ViewSwitchingNavigation.Reports.Views;

namespace ViewSwitchingNavigation.Reports
{
    [ModuleExport(typeof(ReportsModule))]
    public class ReportsModule : IModule
    {
        [Import]
        public IRegionManager regionManager;

        public void Initialize()
        {
            this.regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion, typeof(ReportsNavigationItemView));
        }
    }
}
