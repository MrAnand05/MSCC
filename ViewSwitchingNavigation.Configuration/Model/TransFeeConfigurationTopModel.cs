using Microsoft.Practices.Prism.Mvvm;
using System;
using System.ComponentModel;

namespace ViewSwitchingNavigation.Configuration.Model
{
    public class TransFeeConfigurationTopModel : BindableBase, IDataErrorInfo
    {
        private int? routeId;
        private int? routeNo;
        private string routeDescription;
        private string stop;
        private int? transportFee;
        private bool isEnableSaveButton;
        private bool isEnableEditButton;
        private bool isReadOnly;

        public int? RouteId
        {
            get { return routeId; }
            set { SetProperty(ref this.routeId, value); }
        }
        public int? RouteNo
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
        public int? TransportFee
        {
            get { return transportFee; }
            set { SetProperty(ref this.transportFee, value); }
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
            RouteId = null;
            RouteNo = null;
            Stop = null;
            TransportFee = null;
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
                    case "RouteNo":
                        if (this.RouteNo <= 0 || this.TransportFee <= 0 || this.RouteNo == null || this.TransportFee == null || string.IsNullOrWhiteSpace(this.Stop) || string.IsNullOrWhiteSpace(this.RouteDescription))
                        {
                            IsEnableSaveButton=false;
                            return "";
                         }
                        break;
                    case "TransportFee":
                        if (this.TransportFee <= 0 || this.RouteNo <= 0 || this.RouteNo == null || this.TransportFee == null || string.IsNullOrWhiteSpace(this.Stop) || string.IsNullOrWhiteSpace(this.RouteDescription))
                        {
                            IsEnableSaveButton = false;
                            return "";
                        }
                        break;
                    case "Stop":
                        if (this.TransportFee <= 0 || this.RouteNo <= 0 || this.RouteNo == null || this.TransportFee == null || string.IsNullOrWhiteSpace(this.Stop) || string.IsNullOrWhiteSpace(this.RouteDescription))
                        {
                            IsEnableSaveButton = false;
                            return "";
                        }
                        break;
                    case "RouteDescription":
                        if (this.TransportFee <= 0 || this.RouteNo <= 0 || this.RouteNo == null || this.TransportFee == null || string.IsNullOrWhiteSpace(this.Stop) || string.IsNullOrWhiteSpace(this.RouteDescription))
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
