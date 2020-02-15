using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using System.ComponentModel.Composition;
using ViewSwitchingNavigation.Exam.Views;
using ViewSwitchingNavigation.Infrastructure;

namespace ViewSwitchingNavigation.Exam
{
    [ModuleExport(typeof(ExamModule))]
    public class ExamModule:IModule
    {
        [Import]
        public IRegionManager regionManager;

        public void Initialize()
        {
            this.regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion, typeof(ExamNavigationItemView));
        }
    }
}
