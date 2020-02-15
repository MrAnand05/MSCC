using Microsoft.Practices.Prism.Mvvm;

namespace ViewSwitchingNavigation.Configuration.Model
{
    public class ExamFeeConfigurationModel:BindableBase
    {
        private string studentClass;
        private int? apr;
        private int? may;
        private int? jun;
        private int? jul;
        private int? aug;
        private int? sep;
        private int? oct;
        private int? nov;
        private int? dec;
        private int? jan;
        private int? feb;
        private int? mar;

        public string StudentClass
        {
            get { return studentClass; }
            set { SetProperty(ref this.studentClass, value); }
        }
        public int? Apr
        {
            get { return apr; }
            set { SetProperty(ref this.apr, value); }
        }
        public int? May
        {
            get { return may; }
            set { SetProperty(ref this.may, value); }
        }
        public int? Jun
        {
            get { return jun; }
            set { SetProperty(ref this.jun, value); }
        }
        public int? Jul
        {
            get { return jul; }
            set { SetProperty(ref this.jul, value); }
        }
        public int? Aug
        {
            get { return aug; }
            set { SetProperty(ref this.aug, value); }
        }
        public int? Sep
        {
            get { return sep; }
            set { SetProperty(ref this.sep, value); }
        }
        public int? Oct
        {
            get { return oct; }
            set { SetProperty(ref this.oct, value); }
        }
        public int? Nov
        {
            get { return nov; }
            set { SetProperty(ref this.nov, value); }
        }
        public int? Dec
        {
            get { return dec; }
            set { SetProperty(ref this.dec, value); }
        }
        public int? Jan
        {
            get { return jan; }
            set { SetProperty(ref this.jan, value); }
        }
        public int? Feb
        {
            get { return feb; }
            set { SetProperty(ref this.feb, value); }
        }
        public int? Mar
        {
            get { return mar; }
            set { SetProperty(ref this.mar, value); }
        }
    }
}
