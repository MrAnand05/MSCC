using Microsoft.Practices.Prism.Mvvm;
using System;

namespace ViewSwitchingNavigation.Email.Model
{
    public class EditRePrintFeeSlipModel : BindableBase
    {
        private int paySlipNo;
        private DateTime paymentDate;
        private int totalAmount;
        private int totalRebate;
        private int adminNo;
        private string name;
        private string classSection;
        private string fName;
        private string address;
        public int PaySlipNo
        {
            get { return paySlipNo; }
            set { SetProperty(ref this.paySlipNo, value); }
        }
        public DateTime PaymentDate
        {
            get { return paymentDate; }
            set { SetProperty(ref this.paymentDate, value); }
        }
        public int TotalAmount
        {
            get { return totalAmount; }
            set { SetProperty(ref this.totalAmount, value); }
        }
        public int TotalRebate
        {
            get { return totalRebate; }
            set { SetProperty(ref this.totalRebate, value); }
        }
        public int AdminNo
        {
            get { return adminNo; }
            set { SetProperty(ref this.adminNo, value); }
        }
        public string Name
        {
            get { return name; }
            set { SetProperty(ref this.name, value); }
        }
        public string ClassSection
        {
            get { return classSection; }
            set { SetProperty(ref this.classSection, value); }
        }
        public string FName
        {
            get { return fName; }
            set { SetProperty(ref this.fName, value); }
        }
        public string Address
        {
            get { return address; }
            set { SetProperty(ref this.address, value); }
        }
    }
}
