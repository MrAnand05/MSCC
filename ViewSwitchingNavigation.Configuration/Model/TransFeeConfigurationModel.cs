using Microsoft.Practices.Prism.Mvvm;

namespace ViewSwitchingNavigation.Configuration.Model
{
    public class TransFeeConfigurationModel:BindableBase
    {
        private int routeId;
        private int routeNo;
        private string routeDescription;
        private string stop;
        private int transportFee;
        private bool isChecked;
        public int RouteId
        {
            get { return routeId; }
            set { SetProperty(ref this.routeId, value); }
        }
        public int RouteNo
        {
            get { return routeNo; }
            set { SetProperty(ref this.routeNo, value); }
        }
        public string RouteDescription
        {
            get { return routeDescription; }
            set { SetProperty(ref this.routeDescription, value); }
        }
        public string Stop
        {
            get { return stop; }
            set { SetProperty(ref this.stop, value); }
        }
        public int TransportFee
        {
            get { return transportFee; }
            set { SetProperty(ref this.transportFee, value); }
        }
        public bool IsChecked
        {
            get { return isChecked; }
            set { SetProperty(ref this.isChecked, value); }
        }
    }
}
