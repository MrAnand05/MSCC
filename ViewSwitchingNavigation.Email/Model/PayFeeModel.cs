using Microsoft.Practices.Prism.Mvvm;
using System;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace ViewSwitchingNavigation.Email.Model
{
    public class PayFeeModel : BindableBase,IDataErrorInfo
    {
        //public event PropertyChangedEventHandler PropertyChanged;
        private Boolean checkboxtesting;
        private int? admino;
        private string name;
        private string currentClass;
        private string section;
        private int financialYear;
        private string fatherName;
        private string address;
        private BitmapSource image;
        private string description;
        private int requiredFee;
        private int paid;
        private int rebateGiven;
        private int balance;
        private int rebate;
        private int toPay;
        private DateTime dop;
        private string lastPaymentDate;
        private string rebateRemark;
        private bool siblingConcession;
        private int? feeSlipNo;
        
        public Boolean Checkboxtesting
        {
            get { return this.checkboxtesting; }
            set { SetProperty(ref this.checkboxtesting, value); }
        }
        public int? AdminNo
        {
            get { return this.admino; }
            set
            {
                SetProperty(ref this.admino, value);
                //this.OnPropertyChanged(() => this.FirstName);
            }
        }
        public string Name
        {
            get { return name; }
            set { SetProperty(ref this.name, value); }
        }
        public string RebateRemark
        {
            get { return rebateRemark; }
            set { SetProperty(ref this.rebateRemark, value); }
        }
        public string CurrentClass
        {
            get { return currentClass; }
            set { SetProperty(ref this.currentClass, value); }
        }
        public string Section
        {
            get { return section; }
            set { SetProperty(ref this.section, value); }
        }
        public int FinancialYear
        {
            get { return financialYear; }
            set { SetProperty(ref this.financialYear, value); }
        }
        public string FatherName
        {
            get { return fatherName; }
            set { SetProperty(ref this.fatherName, value); }
        }
        public string Address
        {
            get { return address; }
            set { SetProperty(ref this.address, value); }
        }
        public string Description
        {
            get { return description; }
            set { SetProperty(ref this.description, value); }
        }
        public int RequiredFee
        {
            get { return requiredFee; }
            set { SetProperty(ref this.requiredFee, value); }
        }
        public int Paid
        {
            get { return paid; }
            set { SetProperty(ref this.paid, value); }
        }
        public int RebateGiven
        {
            get { return rebateGiven; }
            set { SetProperty(ref this.rebateGiven, value); }
        }
        public int Balance
        {
            get { return balance; }
            set { SetProperty(ref this.balance, value); }
        }
        public int Rebate
        {
            get { return rebate; }
            set { SetProperty(ref this.rebate, value); }
        }

        public int ToPay
        {
            get { return toPay; }
            set { SetProperty(ref this.toPay, value); }
        }

        public DateTime DOP
        {
            get { return dop; }
            set { SetProperty(ref this.dop, value); }
        }
        public string LastPaymentDate
        {
            get { return lastPaymentDate; }
            set { SetProperty(ref this.lastPaymentDate, value); }
        }
        public bool SiblingConcession
        {
            get { return siblingConcession; }
            set { SetProperty(ref this.siblingConcession, value); }
        }
        public int? FeeSlipNo
        {
            get { return feeSlipNo; }
            set { SetProperty(ref this.feeSlipNo, value); }
        }
        public BitmapSource Image
        {
            get { return this.image; }
            set
            {
                SetProperty(ref this.image, value);
            }
        }
        public string this[string columnName]
        {
            get
            { 
                switch (columnName)
                {
                    case "ToPay":
                        if (this.ToPay < 0 || this.ToPay > (this.Balance-this.Rebate))
                            return "ToPay must be between 0 and Balance-Rebate";
                        break;
                    case "Rebate":
                        if (this.Rebate < 0 || this.Rebate > (this.Balance))
                            return "Rebate must be in 0 and >then Balance";
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
