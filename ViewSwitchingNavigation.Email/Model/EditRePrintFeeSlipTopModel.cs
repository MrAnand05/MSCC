using Microsoft.Practices.Prism.Mvvm;
using System;
using System.ComponentModel;

namespace ViewSwitchingNavigation.Email.Model
{
    public class EditRePrintFeeSlipTopModel:BindableBase,IDataErrorInfo
    {
        private int paySlipNo;
        private int adminNo;
        private DateTime dateOfPayment;
        private string fName;
        private string firstName;
        private string lastName;
        private string selectedClass;
        private string selectedSection;
        private int totalBalance;
        private int totalRequired;
        private int totalRebate;
        private int totalToPay;
        private int feeSlipNo; 
        private bool isEnableUpdateFeeSlip=false;
        private bool isEnablePrintFeeSlip=false;
        private bool isCheckBoxSelected = false;
        public int PaySlipNo
        {
            get { return paySlipNo; }
            set { SetProperty(ref this.paySlipNo, value); }
        }
        public int AdminNo
        {
            get { return adminNo; }
            set { SetProperty(ref this.adminNo, value); }
        }    
        public DateTime DateOfPayment
        {
            get { return dateOfPayment; }
            set { SetProperty(ref this.dateOfPayment, value); }
        }
        public string FName
        {
            get { return fName; }
            set { SetProperty(ref this.fName, value); }
        }
        public string FirstName
        {
            get { return firstName; }
            set { SetProperty(ref this.firstName, value); }
        }
        public string LastName
        {
            get { return lastName; }
            set { SetProperty(ref this.lastName, value); }
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

        public int TotalBalance
        {
            get { return totalBalance; }
            set { SetProperty(ref this.totalBalance, value); }
        }
        public int TotalRequired
        {
            get { return totalRequired; }
            set { SetProperty(ref this.totalRequired, value); }
        }
        public int TotalRebate
        {
            get { return totalRebate; }
            set { SetProperty(ref this.totalRebate, value); }
        }
        public int TotalToPay
        {
            get { return totalToPay; }
            set { SetProperty(ref this.totalToPay, value); }
        }
        public int FeeSlipNo
        {
            get { return feeSlipNo; }
            set { SetProperty(ref this.feeSlipNo, value); }
        }
        public bool IsEnableUpdateFeeSlip
        {
            get { return isEnableUpdateFeeSlip; }
            set { SetProperty(ref this.isEnableUpdateFeeSlip, value); }
        }
        public bool IsEnablePrintFeeSlip
        {
            get { return isEnablePrintFeeSlip; }
            set { SetProperty(ref this.isEnablePrintFeeSlip, value); }
        }
        public bool IsCheckBoxSelected
        {
            get { return isCheckBoxSelected; }
            set { SetProperty(ref this.isCheckBoxSelected, value); }
        }
        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "PaySlipNo":
                        if (this.PaySlipNo < 0)
                            return "The Payment Slip Should be Positive and Integer";
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
