using Microsoft.Practices.Prism.Mvvm;

namespace ViewSwitchingNavigation.Email.Model
{
    public class Sibling:BindableBase
    {
        private string sname;
        private string sclass;
        private string ssection;

        public string Ssection
        {
            get { return ssection; }
            set { SetProperty(ref this.ssection, value); }
        }

        public string Sclass
        {
            get { return sclass; }
            set { SetProperty(ref this.sclass, value); }
        }
        public string Sname
        {
            get { return this.sname; }
            set
            {
                SetProperty(ref this.sname, value);
                //this.OnPropertyChanged(() => this.FirstName);
            }
        }
        public int? Admino { get; set; }

        public void Reset()
        {
            Sname = string.Empty;
            Sclass = string.Empty;
            Ssection = string.Empty;
            Admino = null;
        }
    }
}
