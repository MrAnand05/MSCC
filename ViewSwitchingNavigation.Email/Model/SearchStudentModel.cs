using Microsoft.Practices.Prism.Mvvm;
using System;

namespace ViewSwitchingNavigation.Email.Model
{
    public class SearchStudentModel:BindableBase,ICloneable
    {
        object ICloneable.Clone()
        {
            return this.Clone();
        }
        public SearchStudentModel Clone()
        {
            return (SearchStudentModel)this.MemberwiseClone();
        }
        public int? Admino { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FName { get; set; }
        public string RollNo { get; set; }
        public string Class { get; set; }
        public string Section { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
