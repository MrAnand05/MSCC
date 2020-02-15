using Microsoft.Practices.Prism.Mvvm;
using System;

namespace ViewSwitchingNavigation.Email.Model
{
    public class StudentRegistrationTopModel:BindableBase
    {
        private Boolean isNewRegistration;
        private Boolean isSaveBtnEnable;
        private Boolean isEditBtnEnable=true;
        private Boolean isUpdateBtnEnable;
        private Boolean isBSTranClassFieldEnable;
        private Boolean isClassEnable=true;

        public Boolean IsNewRegistration
        {
            get { return this.isNewRegistration; }
            set
            {
                SetProperty(ref this.isNewRegistration, value);
            }
        }
        public Boolean IsSaveBtnEnable
        {
            get { return this.isSaveBtnEnable; }
            set
            {
                SetProperty(ref this.isSaveBtnEnable, value);
            }
        }
        public Boolean IsEditBtnEnable
        {
            get { return this.isEditBtnEnable; }
            set
            {
                SetProperty(ref this.isEditBtnEnable, value);
            }
        }
        public Boolean IsUpdateBtnEnable
        {
            get { return this.isUpdateBtnEnable; }
            set
            {
                SetProperty(ref this.isUpdateBtnEnable, value);
            }
        }
        public Boolean IsBSTranClassFieldEnable
        {
            get { return this.isBSTranClassFieldEnable; }
            set
            {
                SetProperty(ref this.isBSTranClassFieldEnable, value);
            }
        }
        public Boolean IsClassEnable
        {
            get { return this.isClassEnable; }
            set
            {
                SetProperty(ref this.isClassEnable, value);
            }
        }
    }
}
