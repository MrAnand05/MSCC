using Microsoft.Practices.Prism.Mvvm;

namespace ViewSwitchingNavigation.Configuration.Model
{
    public class SubjectModel : BindableBase
    {
        private int sNo;
        private string subjectName;
        private bool isChecked;
        public int SNo
        {
            get { return sNo; }
            set { SetProperty(ref this.sNo, value); }
        }
        public string SubjectName
        {
            get { return subjectName; }
            set { SetProperty(ref this.subjectName, value); }
        }
        public bool IsChecked
        {
            get { return isChecked; }
            set { SetProperty(ref this.isChecked, value); }
        }
    }
}
