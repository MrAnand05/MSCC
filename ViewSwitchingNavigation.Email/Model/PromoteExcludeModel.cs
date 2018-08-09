using Microsoft.Practices.Prism.Mvvm;

namespace ViewSwitchingNavigation.Email.Model
{
    public class PromoteExcludeModel : BindableBase
    {
        private int sNo;
        private bool chk_Student;
        private int adminNo;
        private string studentName;
        private string fName;
        private string address;
        private string currentClass;
        private string section;
        private int marksObtained;
        private int feeBalance;
        private string action;
        public int SNo
        {
            get { return sNo; }
            set { SetProperty(ref this.sNo, value); }
        }
        public bool Chk_Student
        {
            get { return chk_Student; }
            set { SetProperty(ref this.chk_Student, value); }
        }
        public int AdminNo
        {
            get { return adminNo; }
            set { SetProperty(ref this.adminNo, value); }
        }
        public string StudentName
        {
            get { return studentName; }
            set { SetProperty(ref this.studentName, value); }
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
        public int MarksObtained
        {
            get { return marksObtained; }
            set { SetProperty(ref this.marksObtained, value); }
        }
        public int FeeBalance
        {
            get { return feeBalance; }
            set { SetProperty(ref this.feeBalance, value); }
        }
        public string Action
        {
            get { return action; }
            set { SetProperty(ref this.action, value); }
        }
        //public string this[string columnName]
        //{
        //    get
        //    {
        //        switch (columnName)
        //        {
        //        }

        //        return string.Empty;
        //    }
        //}

        //public string Error
        //{
        //    get { throw new NotImplementedException(); }
        //}
    }
}
