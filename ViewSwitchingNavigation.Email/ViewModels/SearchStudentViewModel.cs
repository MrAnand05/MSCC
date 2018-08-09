using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Input;
using ViewSwitchingNavigation.Email.Model;
using ViewSwitchingNavigation.Infrastructure;

namespace ViewSwitchingNavigation.Email.ViewModels
{
    [Export]
    public class SearchStudentViewModel:BindableBase,IInteractionRequestAware
    {
        public string RadiobuttonParameter { get; set; }
        public ObservableCollection<SearchStudentOutputModel> SearchedStudents { get;  set; }
        public List<string> Classes1 { get; private set; }
        public IEnumerable<string> SectionList { get; private set; }
        public SearchStudentModel CopySte { get; set; }
        DateTime now = DateTime.Now;
        public SearchStudent Student{get;set;}
        public SearchStudentModel Ste { get; set; }
        public SearchStudentViewModel()
        {
            int FinancialYear = AuthenticationContext.GlobalFinancialYear;
            DataSet dc = new DataSet();
            Ste = new SearchStudentModel();
            this.Classes1 = new List<string>();
            Student = new SearchStudent();
            Ste.StartDate = now.AddDays(-15);
            Ste.EndDate = now;
            this.SectionList = new[] {DBNull.Value.ToString(), "A", "B", "C", "D" };
            this.SearchStudentCommand = new DelegateCommand<object>(this.Search);
            this.SelectItemCommand = new DelegateCommand(this.AcceptSelectedItem);
            this.CancelCommand = new DelegateCommand(this.CancelInteraction);
            this.SearchedStudents = new ObservableCollection<SearchStudentOutputModel>();
            SearchRadioBtnCommand = new DelegateCommand<string>(SetParameters);
            SqlCommand command = new SqlCommand("Usp_ClassFeeRelation", con);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@FinancialYear", ((object)FinancialYear ?? DBNull.Value));
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                da.SelectCommand = command;
                da.Fill(dc);
            }
            catch
            {

            }
            finally
            {
                command.Dispose();
            }

            List<string> list = new List<string>();
            Classes1.Add("");
            foreach (DataRow dr in dc.Tables[0].Rows)
            {
                Classes1.Add(dr["Class"].ToString());
            }
        }
        public Action FinishInteraction { get; set; }
        public INotification Notification
        {
            get
            {
                return this.Student;
            }
            set
            {
                if (SearchedStudents != null)
                {
                    SearchedStudents.Clear();
                }
                if (value is SearchStudent)
                {
                    // To keep the code simple, this is the only property where we are raising the PropertyChanged event,
                    // as it's required to update the bindings when this property is populated.
                    // Usually you would want to raise this event for other properties too.
                    this.Student = value as SearchStudent;
                    this.OnPropertyChanged(() => this.Notification);
                }
            }
        }
        public SearchStudentOutputModel SelectedItem { get; set; }
        public ICommand SearchStudentCommand { get; private set; }
        public ICommand SelectItemCommand { get; private set; }
        //public ICommand SearchRadioBtnCommand { get; private set; }
        public DelegateCommand<string> SearchRadioBtnCommand { get; private set; }

        public ICommand CancelCommand { get; private set; }
        public static string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
        SqlConnection con = new SqlConnection(connectionString);
        
        public DataTable dt{get;set;} 
        
        public void SetParameters(string RadiobtnParameter)
        {
            RadiobuttonParameter = RadiobtnParameter;
            
        }
        public void Search(object stu)
        {
            CopySte = (SearchStudentModel)Ste.Clone();
            if (RadiobuttonParameter == "Radio1"||RadiobuttonParameter == null )
            {
                CopySte.StartDate = null;
                CopySte.EndDate = null;
                CopySte.FirstName = null;
                CopySte.LastName = null;
                CopySte.FName = null;
                CopySte.Class = null;
                CopySte.Section = null;
                CopySte.RollNo = null;
            }
            if (RadiobuttonParameter == "Radio2")
            {
                CopySte.Admino = null;
                CopySte.FirstName = null;
                CopySte.LastName = null;
                CopySte.FName = null;
                CopySte.Class = null;
                CopySte.Section = null;
                CopySte.RollNo = null;
            }
            if (RadiobuttonParameter == "Radio3")
            {
                CopySte.Admino = null;
                CopySte.StartDate = null;
                CopySte.EndDate = null;
                CopySte.FirstName = null;
                CopySte.LastName = null;
                CopySte.FName = null;
                CopySte.Class = null;
                CopySte.Section = null;
                CopySte.RollNo = null;
            }
            if (RadiobuttonParameter == "Radio4")
            {
                CopySte.Admino = null;
                CopySte.StartDate = null;
                CopySte.EndDate = null;
            }
            SearchedStudents.Clear();
            //this.SearchedStudents = new ObservableCollection<SearchStudentOutputModel>();
            DataSet ds = new DataSet();
            SqlCommand command = new SqlCommand("Usp_FindStudent", con);
            command.CommandType = CommandType.StoredProcedure;
            
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                command.Parameters.AddWithValue("@AdminNo", ((object)CopySte.Admino ?? DBNull.Value));
                command.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                command.Parameters.AddWithValue("@FirstName", ((object)CopySte.FirstName ?? DBNull.Value));
                command.Parameters.AddWithValue("@LastName", ((object)CopySte.LastName ?? DBNull.Value));
                command.Parameters.AddWithValue("@FName", ((object)CopySte.FName ?? DBNull.Value));
                command.Parameters.AddWithValue("@RollNo", ((object)CopySte.RollNo ?? DBNull.Value));
                command.Parameters.AddWithValue("@Class", String.IsNullOrWhiteSpace(CopySte.Class) ? (object)DBNull.Value : (object)CopySte.Class);
                //command.Parameters.AddWithValue("@Class", ((object)CopySte.Class ?? DBNull.Value));
                command.Parameters.AddWithValue("@Section", String.IsNullOrWhiteSpace(CopySte.Section) ? (object)DBNull.Value : (object)CopySte.Section);
                //command.Parameters.AddWithValue("@Section", ((object)CopySte.Section ?? DBNull.Value));
                command.Parameters.AddWithValue("@StartDate", ((object)CopySte.StartDate ?? DBNull.Value));
                command.Parameters.AddWithValue("@EndDate", ((object)CopySte.EndDate ?? DBNull.Value));
                da.SelectCommand = command;
                da.Fill(ds);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    
                    var obj = new SearchStudentOutputModel();
                    obj.Admino = (int?)row["AdminNo"];
                    obj.DOJ=((DateTime)row["DOJ"]).ToShortDateString();
                    obj.FirstName = row["FirstName"].ToString();
                    obj.LastName= row["LastName"].ToString();
                    obj.FName=row["FName"].ToString();
                    obj.MName=row["MName"].ToString();
                    obj.DOB=((DateTime)row["DOB"]).ToShortDateString();
                    if(row["Gender"].ToString()=="Male")
                    {
                        obj.Gender = Gender.Male;
                    }
                    else
                        obj.Gender=Gender.Female;
                    obj.Religion = row["Religion"].ToString();
                    obj.Category = row["Category"].ToString();
                    obj.PSchoolName = row["PSchoolName"].ToString();
                    obj.LClass = row["LClass"].ToString();
                    obj.YOPLC = ((DateTime)row["YOPLC"]).ToShortDateString();
                    obj.LCMarks = (int)row["LCMarks"];
                    if (row["Medium"].ToString()=="E")
                    {
                        obj.Medium = "English";
                    }
                    else if (row["Medium"].ToString() == "H")
                    {
                        obj.Medium = "Hindi";
                    }
                    else
                    {
                        obj.Medium = "Others";
                    }
                    obj.DocID = (int)row["DocID"];
                    obj.PAddress=row["PAddress"].ToString();
                    obj.PDistrict= row["PDistrict"].ToString();
                    obj.PContact = (Int64)row["PContact"];
                    obj.LAddress = row["LAddress"].ToString();
                    obj.LDistrict = row["LDistrict"].ToString();
                    obj.LContact = (Int64)row["LContact"];
                    obj.Class = row["CurrentClass"].ToString();
                    obj.Section=row["Section"].ToString();
                    obj.RollNo = row["RollNo"].ToString();
                    obj.SiblingConcession = (bool)row["SiblingConcession"];
                    if(obj.SiblingConcession)
                    {
                        obj.SiblingLessAmount = (int)row["SiblingLessAmount"];
                    }
                    obj.IsAvailingTransport = (bool)row["IsAvailingTransport"];
                    if ((bool)row["IsAvailingTransport"])
                    {
                        obj.RountNo = (int)row["RouteNo"];
                        obj.Stop = row["Stop"].ToString();
                        obj.StartMonth = (int)row["startMonth"];
                    }
                    obj.FinancialYear=(int)row["FinancialYear"];
                    obj.CurrentFinancialYear = (int)row["CurrentFinancialYear"];
                    obj.ScholarNo = (int)row["ScholarNo"];
                    SearchedStudents.Add(obj);
                }
            }
            catch
            {

            }
            finally
            {
                command.Dispose();
            }
        }
        public void AcceptSelectedItem()
        {
            if (this.Student != null)
            {
                this.Student.SelectedStudent = this.SelectedItem;
                this.Student.Confirmed = true;
            }

            this.FinishInteraction();
        }
        public void CancelInteraction()
        {
            if (this.Student != null)
            {
                this.Student.SelectedStudent = null;
                this.Student.Confirmed = false;
            }

            this.FinishInteraction();
        }

    }
}
