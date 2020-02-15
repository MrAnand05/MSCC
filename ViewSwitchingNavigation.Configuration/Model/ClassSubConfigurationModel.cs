using Microsoft.Practices.Prism.Mvvm;

namespace ViewSwitchingNavigation.Configuration.Model
{
    public class ClassSubConfigurationModel : BindableBase
    {
        private int id;
        private string sclass;
        private int? subId;
        private string subject;
        private int mm;
        private int term;
        private int mmd;
        private int mm1;
        private int mm2;
        private int mm3;
        private bool isChecked;

        public int Id
        {
            get { return id; }
            set { SetProperty(ref this.id, value); }
        }
        public string Sclass
        {
            get { return sclass; }
            set { SetProperty(ref this.sclass, value); }
        }
        public int? SubId
        {
            get { return subId; }
            set { SetProperty(ref this.subId, value); }
        }
        public string Subject
        {
            get { return subject; }
            set { SetProperty(ref this.subject, value); }
        }
        public int Mm
        {
            get { return mm; }
            set { SetProperty(ref this.mm, value); }
        }
        public int Term
        {
            get { return term; }
            set { SetProperty(ref this.term, value); }
        }
        public int Mmd
        {
            get { return mmd; }
            set { SetProperty(ref this.mmd, value); }
        }
        public int Mm1
        {
            get { return mm1; }
            set { SetProperty(ref this.mm1, value); }
        }
        public int Mm2
        {
            get { return mm2; }
            set { SetProperty(ref this.mm2, value); }
        }
        public int Mm3
        {
            get { return mm3; }
            set { SetProperty(ref this.mm3, value); }
        }
        public bool IsChecked
        {
            get { return isChecked; }
            set { SetProperty(ref this.isChecked, value); }
        }
    }
}
