using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using System.ComponentModel.Composition;
using ViewSwitchingNavigation.ExpenseManagement.Views;
using ViewSwitchingNavigation.Infrastructure;

namespace ViewSwitchingNavigation.ExpenseManagement
{
    [ModuleExport(typeof(ExpenseManagementModule))]
    public class ExpenseManagementModule : IModule
    {
        [Import]
        public IRegionManager regionManager;

        public void Initialize()
        {
            this.regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion, typeof(ExpenseManagementNavigationItemView));
        }

    }
}
