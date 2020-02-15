using Microsoft.Practices.Prism.Mvvm;

namespace ViewSwitchingNavigation.Configuration.Model
{
    public class InExpConfigurationModel : BindableBase
    {
        private int sNo;
        private string incomeExpenseSource;
        private bool isChecked;
        public int SNo
        {
            get { return sNo; }
            set { SetProperty(ref this.sNo, value); }
        }
        public string IncomeExpenseSource
        {
            get { return incomeExpenseSource; }
            set { SetProperty(ref this.incomeExpenseSource, value); }
        }
        public bool IsChecked
        {
            get { return isChecked; }
            set { SetProperty(ref this.isChecked, value); }
        }
    }
}
