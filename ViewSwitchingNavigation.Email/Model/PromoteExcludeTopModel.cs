using Microsoft.Practices.Prism.Mvvm;
using System;
using System.ComponentModel;

namespace ViewSwitchingNavigation.Email.Model
{
    public class PromoteExcludeTopModel : BindableBase,IDataErrorInfo
    {
        private bool selectAll;
        private int totalStudents;
        private int totalSelectedStudents;
        private bool promoteExclusion;
        private bool failLS;
        private bool pO12;
        private bool revert;
        private string selectedClass;
        private string selectedSection;
        private string selectedType;
        private int? adminNo;
        public bool SelectAll
        {
            get { return selectAll; }
            set { SetProperty(ref this.selectAll, value); }
        }
        public int TotalStudents
        {
            get { return totalStudents; }
            set { SetProperty(ref this.totalStudents, value); }
        }
        public int TotalSelectedStudents
        {
            get { return totalSelectedStudents; }
            set { SetProperty(ref this.totalSelectedStudents, value); }
        }
        public bool PromoteExclusion
        {
            get { return promoteExclusion; }
            set { SetProperty(ref this.promoteExclusion, value); }
        }
        public bool FailLS
        {
            get { return failLS; }
            set { SetProperty(ref this.failLS, value); }
        }
        public bool PO12
        {
            get { return pO12; }
            set { SetProperty(ref this.pO12, value); }
        }
        public bool Revert
        {
            get { return revert; }
            set { SetProperty(ref this.revert, value); }
        }
        public string SelectedClass
        {
            get { return selectedClass; }
            set { SetProperty(ref this.selectedClass, value); }
        }
        public string SelectedSection
        {
            get { return selectedSection; }
            set { SetProperty(ref this.selectedSection, value); }
        }
        public string SelectedType
        {
            get { return selectedType; }
            set { SetProperty(ref this.selectedType, value); }
        }
        public int? AdminNo
        {
            get { return adminNo; }
            set { SetProperty(ref this.adminNo, value); }
        }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "AdminNo":
                        if (this.AdminNo < 0 )
                            return "The Payment must be between 0 and Balance-Rebate";
                        break;
                }

                return string.Empty;
            }
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }
    }
}