using Microsoft.Practices.Prism.Mvvm;
using System;

namespace ViewSwitchingNavigation.ExpenseManagement.Model
{
    public class IncomeExpenseModel:BindableBase
    {
        private int sNo;
        private string iESource;
        private int income;
        private int expense;
        private int balance;
        private int runningbalance;
        private DateTime iEDate;
        private bool isDBData;
        private string isVisible;
        private string isChkVisible;
        private bool isChecked;

        public int SNo
        {
            get { return sNo; }
            set { SetProperty(ref this.sNo, value); }
        }
        public string IESource
        {
            get { return iESource; }
            set { SetProperty(ref this.iESource, value); }
        }
        public int Income
        {
            get { return income; }
            set { SetProperty(ref this.income, value); }
        }
        public int Expense
        {
            get { return expense; }
            set { SetProperty(ref this.expense, value); }
        }
        public int Balance
        {
            get { return balance; }
            set { SetProperty(ref this.balance, value); }
        }
        public int RunningBalance
        {
            get { return runningbalance; }
            set { SetProperty(ref this.runningbalance, value); }
        }
        public DateTime IEDate
        {
            get { return iEDate; }
            set { SetProperty(ref this.iEDate, value); }
        }
        public bool IsDBData
        {
            get { return isDBData; }
            set { SetProperty(ref this.isDBData, value); }
        }
        public string IsVisible
        {
            get { return isVisible; }
            set { SetProperty(ref this.isVisible, value); }
        }
        public string IsChkVisible
        {
            get { return isChkVisible; }
            set { SetProperty(ref this.isChkVisible, value); }
        }
        public bool IsChecked
        {
            get { return isChecked; }
            set { SetProperty(ref this.isChecked, value); }
        }
    }
}
