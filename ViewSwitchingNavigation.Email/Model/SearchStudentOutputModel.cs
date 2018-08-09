using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Drawing;
using ViewSwitchingNavigation.Infrastructure;

namespace ViewSwitchingNavigation.Email.Model
{
    public class SearchStudentOutputModel:BindableBase
    {
        public int? Admino { get; set; }
        public string DOJ { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FName { get; set; }
        public string MName { get; set; }
        public string DOB { get; set; }
        public Gender Gender { get; set; }
        public string Religion { get; set; }
        public string Category { get; set; }
        public string PAddress { get; set; }
        public string PDistrict { get; set; }
        public Int64 PContact { get; set; }
        public string LAddress { get; set; }
        public string LDistrict { get; set; }
        public Int64 LContact { get; set; }
        public int? Sib1AdminNo { get; set; }
        public int? Sib2AdminNo { get; set; }
        public bool SiblingConcession { get; set; }
        public int? SiblingLessAmount { get; set; }
        public string Class { get; set; }
        public string Section { get; set; }
        public string RollNo { get; set; }
        public string PSchoolName { get; set; }
        public string LClass { get; set; }
        public string YOPLC { get; set; }
        public int LCMarks { get; set; }
        public string Medium { get; set; }
        public int DocID { get; set; }
        public int CurrentMonthFeeBalance { get; set; }
        public bool IsAvailingTransport { get; set; }
        public int RountNo { get; set; }
        public string Stop { get; set; }
        public int StartMonth { get; set; }
        public int EndMonth { get; set; }
        public int FinancialYear { get; set; }
        public int CurrentFinancialYear { get; set; }
        public Bitmap StuImage { get; set; }
        public int? ScholarNo { get; set; }
    }
}
