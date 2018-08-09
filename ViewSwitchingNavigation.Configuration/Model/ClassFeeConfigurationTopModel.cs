using Microsoft.Practices.Prism.Mvvm;

namespace ViewSwitchingNavigation.Configuration.Model
{
    public class ClassFeeConfigurationTopModel : BindableBase
    {
        private bool isEnableSaveButton;
        private bool isEnableEditButton;
        private bool isReadOnly;
        public bool IsEnableSaveButton
        {
            get { return isEnableSaveButton; }
            set { SetProperty(ref this.isEnableSaveButton, value); }
        }
        public bool IsEnableEditButton
        {
            get { return isEnableEditButton; }
            set { SetProperty(ref this.isEnableEditButton, value); }
        }
        public bool IsReadOnly
        {
            get { return isReadOnly; }
            set { SetProperty(ref this.isReadOnly, value); }
        }
    }
}
