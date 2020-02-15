using Microsoft.Practices.Prism.Mvvm;
using System;
using System.ComponentModel;

namespace ViewSwitchingNavigation.Email.Model
{
    public class PayMonthlyFeeModel:BindableBase,IDataErrorInfo
    {
        private Boolean monthlyCheckBox;
        private string monthName;
        private int requiredMonthlyFee;
        private int monthlypaid;
        private int monthlyRebateGiven;
        private int monthlyBalance;
        private int monthlyRebate;
        private int monthlyToPay;
        private DateTime monthlyDOP;
        private string montlyLastPaymentDate;
        private string monthlyRebateRemark;
        private int yearlyMonthlyRequiredTotal;
        private int yearlyMonthlyRebateTotal;
        private int yearlyMonthlyToPayTotal;
        private int yearlyMonthlyBalance;
        private int? feeSlipNo;
        public Boolean MonthlyCheckBox
        {
            get { return this.monthlyCheckBox; }
            set { SetProperty(ref this.monthlyCheckBox, value); }
        }
        public string MonthName
        {
            get { return monthName; }
            set { SetProperty(ref this.monthName, value); }
        }
        public int RequiredMonthlyFee
        {
            get { return requiredMonthlyFee; }
            set { SetProperty(ref this.requiredMonthlyFee, value); }
        }
        public int Monthlypaid
        {
            get { return monthlypaid; }
            set { SetProperty(ref this.monthlypaid, value); }
        }
        public int MonthlyRebateGiven
        {
            get { return monthlyRebateGiven; }
            set { SetProperty(ref this.monthlyRebateGiven, value); }
        }
        public int MonthlyBalance
        {
            get { return monthlyBalance; }
            set { SetProperty(ref this.monthlyBalance, value); }
        }
        public int MonthlyRebate
        {
            get { return monthlyRebate; }
            set { SetProperty(ref this.monthlyRebate, value); }
        }
        public int MonthlyToPay
        {
            get { return monthlyToPay; }
            set { SetProperty(ref this.monthlyToPay, value); }
        }

        public DateTime MonthlyDOP
        {
            get { return monthlyDOP; }
            set { SetProperty(ref this.monthlyDOP, value); }
        }
        public string MontlyLastPaymentDate
        {
            get { return montlyLastPaymentDate; }
            set { SetProperty(ref this.montlyLastPaymentDate, value); }
        }
        public string MonthlyRebateRemark
        {
            get { return monthlyRebateRemark; }
            set { SetProperty(ref this.monthlyRebateRemark, value); }
        }
        public int YearlyMonthlyRequiredTotal
        {
            get { return yearlyMonthlyRequiredTotal; }
            set { SetProperty(ref this.yearlyMonthlyRequiredTotal, value); }
        }
        public int YearlyMonthlyRebateTotal
        {
            get { return yearlyMonthlyRebateTotal; }
            set { SetProperty(ref this.yearlyMonthlyRebateTotal, value); }
        }
        public int YearlyMonthlyToPayTotal
        {
            get { return yearlyMonthlyToPayTotal; }
            set { SetProperty(ref this.yearlyMonthlyToPayTotal, value); }
        }
        public int YearlyMonthlyBalance
        {
            get { return yearlyMonthlyBalance; }
            set { SetProperty(ref this.yearlyMonthlyBalance, value); }
        }
        public int? FeeSlipNo
        {
            get { return feeSlipNo; }
            set { SetProperty(ref this.feeSlipNo, value); }
        }
        
        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "MonthlyToPay":
                        if (this.MonthlyToPay < 0 || this.MonthlyToPay > (this.MonthlyBalance- this.MonthlyRebate))
                            return "The Payment must be between 0 and Balance-Rebate";
                        break;
                    case "MonthlyRebate":
                        if (this.MonthlyRebate < 0 || this.MonthlyRebate > (this.MonthlyBalance))
                            return "Rebate must be in between 0 and Balance";
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
