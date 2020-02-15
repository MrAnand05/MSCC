using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using System.ComponentModel.Composition;
using ViewSwitchingNavigation.Configuration.Views;
using ViewSwitchingNavigation.Infrastructure;

namespace ViewSwitchingNavigation.Configuration
{
    [ModuleExport(typeof(ConfigurationModule))]
    public class ConfigurationModule:IModule
    {
        [Import]
        public IRegionManager regionManager;

        public void Initialize()
        {
            this.regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion, typeof(ConfigurationNavigationItemView));
        }
    }
}
