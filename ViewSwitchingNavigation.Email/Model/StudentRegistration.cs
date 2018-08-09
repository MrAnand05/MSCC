using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Windows.Media.Imaging;
using ViewSwitchingNavigation.Infrastructure;

namespace ViewSwitchingNavigation.Email.Model
{
    public class StudentRegistration : BindableBase
    {
        //public event PropertyChangedEventHandler PropertyChanged;
        private int? admino;
        private string firstName;
        private string lastName;
        private DateTime dOJ;
        private string fName ;
        private string mName ;
        private DateTime dOB ;
        private Gender gender ;
        private string sections ;
        private string religion ;
        private string category ;
        private string pAddress ;
        private string pDistrict ;
        private Int64 pContact ;
        private string lAddress ;
        private string lDistrict ;
        private Int64 lContact ;
        private int? sib1AdminNo ;
        private string s1Name ;
        private string s1Class ;
        private char s1Section ;
        private int? sib2AdminNo ;
        private string sclass ;
        private string section;
        private string rollNo ;
        private string pSchoolName ;
        private string lClass ;
        private DateTime yOPLC ;
        private int lCMarks ;
        private string medium ;
        private int docID ;
        private bool siblingConcession ;
        private int? siblingLessAmount;   
        private bool hostelRequired ;
        private bool isOldStudentRegistration;
        //TransportDetails Start
        private bool isAvailingTransport;
        private int? startMonth;
        private int? routeNo;
        private string stop;
        // End
        private int rebate ;
        private string rebateRemark ;
        private string studentRemark ;
        BitmapSource image;
        private int? scholarNo;
        public int? AdminNo
        {
            get { return this.admino; }
            set
            {
                SetProperty(ref this.admino, value);
            }
        }
        public string FirstName
        {
            get { return this.firstName; }
            set
            {
                SetProperty(ref this.firstName, value);
            }
        }
        public string LastName
        {
            get { return this.lastName; }
            set
            {
                SetProperty(ref this.lastName, value);
            }
        }
        public DateTime DOJ
        {
            get { return this.dOJ; }
            set
            {
                SetProperty(ref this.dOJ, value);
            }
        }
        public string FName
        {
            get { return this.fName; }
            set
            {
                SetProperty(ref this.fName, value);
            }
        }
        public string MName
        {
            get { return this.mName; }
            set
            {
                SetProperty(ref this.mName, value);
            }
        }
        public DateTime DOB
        {
            get { return this.dOB; }
            set
            {
                SetProperty(ref this.dOB, value);
            }
        }
        public Gender Gender
        {
            get { return this.gender; }
            set
            {
                SetProperty(ref this.gender, value);
            }
        }
        public string Sections
        {
            get { return this.sections; }
            set
            {
                SetProperty(ref this.sections, value);
            }
        }
        public string Religion
        {
            get { return this.religion; }
            set
            {
                SetProperty(ref this.religion, value);
            }
        }
        public string Category
        {
            get { return this.category; }
            set
            {
                SetProperty(ref this.category, value);
            }
        }
        public string PAddress
        {
            get { return this.pAddress; }
            set
            {
                SetProperty(ref this.pAddress, value);
            }
        }
        public string PDistrict
        {
            get { return this.pDistrict; }
            set
            {
                SetProperty(ref this.pDistrict, value);
            }
        }
        public Int64 PContact
        {
            get { return this.pContact; }
            set
            {
                SetProperty(ref this.pContact, value);
            }
        }
        public string LAddress
        {
            get { return this.lAddress; }
            set
            {
                SetProperty(ref this.lAddress, value);
            }
        }
        public string LDistrict
        {
            get { return this.lDistrict; }
            set
            {
                SetProperty(ref this.lDistrict, value);
            }
        }
        public Int64 LContact
        {
            get { return this.lContact; }
            set
            {
                SetProperty(ref this.lContact, value);
            }
        }
        public int? Sib1AdminNo
        {
            get { return this.sib1AdminNo; }
            set
            {
                SetProperty(ref this.sib1AdminNo, value);
            }
        }
        public string S1Name
        {
            get { return this.s1Name; }
            set
            {
                SetProperty(ref this.s1Name, value);
            }
        }
        public string S1Class
        {
            get { return this.s1Class; }
            set
            {
                SetProperty(ref this.s1Class, value);
            }
        }
        public char S1Section
        {
            get { return this.s1Section; }
            set
            {
                SetProperty(ref this.s1Section, value);
            }
        }
        public int? Sib2AdminNo
        {
            get { return this.sib2AdminNo; }
            set
            {
                SetProperty(ref this.sib2AdminNo, value);
            }
        }
        public string Class
        {
            get { return this.sclass; }
            set
            {
                SetProperty(ref this.sclass, value);
            }
        }
        public string Section
        {
            get { return this.section; }
            set
            {
                SetProperty(ref this.section, value);
            }
        }
        public string RollNo
        {
            get { return this.rollNo; }
            set
            {
                SetProperty(ref this.rollNo, value);
            }
        }
        public string PSchoolName
        {
            get { return this.pSchoolName; }
            set
            {
                SetProperty(ref this.pSchoolName, value);
            }
        }
        public string LClass
        {
            get { return this.lClass; }
            set
            {
                SetProperty(ref this.lClass, value);
            }
        }
        public DateTime YOPLC
        {
            get { return this.yOPLC; }
            set
            {
                SetProperty(ref this.yOPLC, value);
            }
        }
        public int LCMarks
        {
            get { return this.lCMarks; }
            set
            {
                SetProperty(ref this.lCMarks, value);
            }
        }
        public string Medium
        {
            get { return this.medium; }
            set
            {
                SetProperty(ref this.medium, value);
            }
        }
        public int DocID
        {
            get { return this.docID; }
            set
            {
                SetProperty(ref this.docID, value);
            }
        }
        public bool SiblingConcession
        {
            get { return this.siblingConcession; }
            set
            {
                SetProperty(ref this.siblingConcession, value);
            }
        }

        public int? SiblingLessAmount
        {
            get { return this.siblingLessAmount; }
            set
            {
                SetProperty(ref this.siblingLessAmount, value);
            }
        }
        public bool HostelRequired
        {
            get { return this.hostelRequired; }
            set
            {
                SetProperty(ref this.hostelRequired, value);
            }
        }
        public bool IsOldStudentRegistration
        {
            get { return this.isOldStudentRegistration; }
            set
            {
                SetProperty(ref this.isOldStudentRegistration, value);
            }
        }
        public bool IsAvailingTransport
        {
            get { return this.isAvailingTransport; }
            set { SetProperty(ref this.isAvailingTransport, value); }
        }
        public int? StartMonth
        {
            get { return startMonth; }
            set { SetProperty(ref this.startMonth, value); }
        }
        public int? RouteNo
        {
            get { return routeNo; }
            set { SetProperty(ref this.routeNo, value); }
        }
        public string Stop
        {
            get { return stop; }
            set { SetProperty(ref this.stop, value); }
        }
        public int Rebate
        {
            get { return this.rebate; }
            set
            {
                SetProperty(ref this.rebate, value);
            }
        }
        public string RebateRemark
        {
            get { return this.rebateRemark; }
            set
            {
                SetProperty(ref this.rebateRemark, value);
            }
        }
        public string StudentRemark
        {
            get { return this.studentRemark; }
            set
            {
                SetProperty(ref this.studentRemark, value);
            }
        }
        public BitmapSource Image
        {
            get { return this.image; }
            set
            {
                SetProperty(ref this.image, value);
            }
        }
        public int? ScholarNo
        {
            get { return scholarNo; }
            set { SetProperty(ref this.scholarNo, value); }
        }
        
        public void Reset()
        {
            AdminNo = null;
            FirstName = string.Empty;
            LastName = string.Empty;
            DOJ = DateTime.Today;
            FName = string.Empty;
            MName = string.Empty;
            DOB =DateTime.Today;
            Religion = null;
            Category = null;
            PSchoolName = string.Empty;
            LClass = null;
            YOPLC = DateTime.Today;
            LCMarks = 0;
            Medium = null;
            DocID = 0;
            PAddress = string.Empty;
            PDistrict = string.Empty;
            PContact = 0;
            LAddress = string.Empty;
            LDistrict = string.Empty;
            LContact = 0;
            Sib1AdminNo = null;
            S1Class = null;
            S1Section = '\0';
            S1Name = string.Empty;
            Sib2AdminNo = null;
            //Sclass = null;
            //SSection = '\0';
            //SName = string.Empty;
            Image = null;
            Class =null;
            Section = null;
            IsAvailingTransport = false;
            SiblingLessAmount = 0;
            IsOldStudentRegistration = false;
            SiblingConcession = false;
            SiblingLessAmount = null;
            IsAvailingTransport = false;
            StartMonth = null;
            RouteNo = null;
            Stop = null;
            ScholarNo = null;
        }

    }
}
