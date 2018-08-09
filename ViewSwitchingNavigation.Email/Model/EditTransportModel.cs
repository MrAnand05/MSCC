using Microsoft.Practices.Prism.Mvvm;

namespace ViewSwitchingNavigation.Email.Model
{
    public class EditTransportModel:BindableBase
    {
        
        private int? adminNo;
        private string name;
        private string classSec;
        private string address;
        private bool isAvailingTransport;
        private int routeNoCurrent;
        private string stopCurrent;
        private int startMonthCurrent;
        private string endMonthCurrent;
        private int? routeNoNew;
        private string stopNew;
        private string startMonthNew;
        private string endMonthNew;

        public int? AdminNo
        {
            get { return adminNo; }
            set { SetProperty(ref this.adminNo, value); }
        }
        public string Name
        {
            get { return name; }
            set { SetProperty(ref this.name, value); }
        }
        public string ClassSec
        {
            get { return classSec; }
            set { SetProperty(ref this.classSec, value); }
        }
        public string Address
        {
            get { return address; }
            set { SetProperty(ref this.address, value); }
        }
        public bool IsAvailingTransport
        {
            get { return isAvailingTransport; }
            set { SetProperty(ref this.isAvailingTransport, value); }
        }
        public int RouteNoCurrent
        {
            get { return routeNoCurrent; }
            set { SetProperty(ref this.routeNoCurrent, value); }
        }
        public string StopCurrent
        {
            get { return stopCurrent; }
            set { SetProperty(ref this.stopCurrent, value); }
        }
        public int StartMonthCurrent
        {
            get { return startMonthCurrent; }
            set { SetProperty(ref this.startMonthCurrent, value); }
        }
        public string EndMonthCurrent
        {
            get { return endMonthCurrent; }
            set { SetProperty(ref this.endMonthCurrent, value); }
        }
        public int? RouteNoNew
        {
            get { return routeNoNew; }
            set { SetProperty(ref this.routeNoNew, value); }
        }
        public string StopNew
        {
            get { return stopNew; }
            set { SetProperty(ref this.stopNew, value); }
        }
        public string StartMonthNew
        {
            get { return startMonthNew; }
            set { SetProperty(ref this.startMonthNew, value); }
        }
        public string EndMonthNew
        {
            get { return endMonthNew; }
            set { SetProperty(ref this.endMonthNew, value); }
        }
    }
}
