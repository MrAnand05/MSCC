using Microsoft.Practices.Prism.Mvvm;

namespace ViewSwitchingNavigation.Configuration.Model
{
    public class ClassFeeConfigurationModel:BindableBase
    {
        private string studentClass;
        private int admissionFee;
        private int annualFee;
        private int formFee;
        private int iDCardFee;
        private int monthlyFee;
        private int computerFee;
        private int boardRegistrationFee;
        private int examFee;
        private int tieBeltCharges;
        private int otherCharges;
        public string StudentClass
        {
            get { return studentClass; }
            set { SetProperty(ref this.studentClass, value); }
        }
        public int AdmissionFee
        {
            get { return admissionFee; }
            set { SetProperty(ref this.admissionFee, value); }
        }
        public int AnnualFee
        {
            get { return annualFee; }
            set { SetProperty(ref this.annualFee, value); }
        }
        public int FormFee
        {
            get { return formFee; }
            set { SetProperty(ref this.formFee, value); }
        }
        public int IDCardFee
        {
            get { return iDCardFee; }
            set { SetProperty(ref this.iDCardFee, value); }
        }
        public int MonthlyFee
        {
            get { return monthlyFee; }
            set { SetProperty(ref this.monthlyFee, value); }
        }
        public int ComputerFee
        {
            get { return computerFee; }
            set { SetProperty(ref this.computerFee, value); }
        }
        public int BoardRegistrationFee
        {
            get { return boardRegistrationFee; }
            set { SetProperty(ref this.boardRegistrationFee, value); }
        }
        public int ExamFee
        {
            get { return examFee; }
            set { SetProperty(ref this.examFee, value); }
        }
        public int TieBeltCharges
        {
            get { return tieBeltCharges; }
            set { SetProperty(ref this.tieBeltCharges, value); }
        }
        public int OtherCharges
        {
            get { return otherCharges; }
            set { SetProperty(ref this.otherCharges, value); }
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
