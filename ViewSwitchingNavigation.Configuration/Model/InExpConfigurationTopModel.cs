using Microsoft.Practices.Prism.Mvvm;
using System;
using System.ComponentModel;

namespace ViewSwitchingNavigation.Configuration.Model
{
    public class InExpConfigurationTopModel : BindableBase, IDataErrorInfo
    {
        private int? sNo;
        private string incomeExpenseSource;
        private bool isEnableSaveButton;
        private bool isEnableEditButton;
        private bool isReadOnly;

        public int? SNo
        {
            get { return sNo; }
            set { SetProperty(ref this.sNo, value); }
        }
        public string IncomeExpenseSource
        {
            get { return incomeExpenseSource; }
            set { SetProperty(ref this.incomeExpenseSource, value); }
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
            IncomeExpenseSource = null;
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
                    case "IncomeExpenseSource":
                        if (string.IsNullOrWhiteSpace(this.IncomeExpenseSource))
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
