using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System.Collections.Generic;

namespace ViewSwitchingNavigation.Email.Model
{
    public class SearchStudent:Confirmation
    {
        //public SearchStudent()
        //{
        //    this.Students = new List<StudentRegistration>();
        //    this.SelectedStudent = null;
        //}
        //public SearchStudent(IEnumerable<StudentRegistration> students)
        //    : this()
        //{
        //    foreach (StudentRegistration student in students)
        //    {
        //        this.Students.Add(student);
        //    }
        //}
        public IList<StudentRegistration> Students { get; private set; }

        public SearchStudentOutputModel SelectedStudent { get; set; }
        //public int Admino { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string FName { get; set; }
        //public string RollNo { get; set; }
        //public string Class { get; set; }
        //public string Section { get; set; }
        //public DateTime StartDate { get; set; }
        //public DateTime EndDate { get; set; }
    }
}
