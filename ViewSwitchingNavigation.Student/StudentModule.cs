using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using ViewSwitchingNavigation.Student.Views;
using ViewSwitchingNavigation.Infrastructure;

namespace ViewSwitchingNavigation.Student
{

    [ModuleExport(typeof(StudentModule))]
    public class StudentModule:IModule
    {
        [Import]
        public IRegionManager regionManager;

        public void Initialize()
        {
            this.regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion, typeof(StudentNavigationItemView));
        }
    }
}
