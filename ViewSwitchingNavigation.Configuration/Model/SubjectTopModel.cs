using Microsoft.Practices.Prism.Mvvm;
using System;
using System.ComponentModel;

namespace ViewSwitchingNavigation.Configuration.Model
{
    public class SubjectTopModel : BindableBase, IDataErrorInfo
    {
        private int? sNo;
        private string subjectName;
        private bool isEnableSaveButton;
        private bool isEnableEditButton;
        private bool isReadOnly;

        public int? SNo
        {
            get { return sNo; }
            set { SetProperty(ref this.sNo, value); }
        }
        public string SubjectName
        {
            get { return subjectName; }
            set { SetProperty(ref this.subjectName, value); }
        }
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
        public void Reset()
        {
            SNo = null;
            subjectName = null;
            IsEnableSaveButton = false;
            IsEnableEditButton = false;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }
        public string this[string columnName]
        {
            get
            {
               switch (columnName)
                {
                    case "SubjectName":
                        if (string.IsNullOrWhiteSpace(this.SubjectName))
                        {
                            IsEnableSaveButton = false;
                            return "";
                        }
                        break;
                }

                IsEnableSaveButton = true ;
                return "";
            }
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }
    }
}
