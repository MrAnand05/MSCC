using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Input;
using ViewSwitchingNavigation.Email.Model;
using ViewSwitchingNavigation.Infrastructure;

namespace ViewSwitchingNavigation.Email.ViewModels
{
    [Export]
    public class EditPromoteExcludeViewModel : BindableBase
    {
        public IEnumerable<string> SectionList { get; private set; }
        public List<string> Classes1 { get; private set; }
        public IEnumerable<string> TypeList { get; private set; }
        public PromoteExcludeTopModel PromoteExcludeTop { get; set; }
        public ObservableCollection<PromoteExcludeModel> SearchedStudents { get; set; }
        public DelegateCommand<object> ckhCheckedCommand { get; private set; }
        public DelegateCommand<object> RevertCommand { get; private set; }
        public DelegateCommand<object> chkSelectAllCommand { get; private set; } 
        public static string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
        SqlConnection con = new SqlConnection(connectionString);
        DataSet ds = new DataSet();
        public ICommand SearchStudentCommand { get; private set; }
        public EditPromoteExcludeViewModel()
        {
            this.SectionList = new[] { DBNull.Value.ToString(), "A", "B", "C", "D" };
            this.TypeList = new[] { DBNull.Value.ToString(), "LeavingSchool","Promoted" , "12PassOut", "Failed" };
            this.PromoteExcludeTop = new PromoteExcludeTopModel();
            PromoteExcludeTop.SelectAll = false;
            PromoteExcludeTop.TotalStudents = 0;
            PromoteExcludeTop.TotalSelectedStudents = 0;
            this.Classes1 = new List<string>();
            this.SearchedStudents = new ObservableCollection<PromoteExcludeModel>();
            this.SearchStudentCommand = new DelegateCommand<object>(this.Search);
            this.ckhCheckedCommand = new DelegateCommand<object>(OnCheckBoxSelectedExecuted);
            this.RevertCommand = new DelegateCommand<object>(OnRevertExecuted);
            this.chkSelectAllCommand = new DelegateCommand<object>(OnChkAllExecuted);
            SqlCommand command = new SqlCommand("Usp_ClassFeeRelation", con);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                da.SelectCommand = command;
                da.Fill(ds);
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
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Classes1.Add(dr["Class"].ToString());
            }
        }
        public void Search(object parameters)
        {
            string Type = "";
            Type = (((object[])parameters)[2].ToString());
            if (Type == "Failed")
                Type = "Detained";
            SearchedStudents.Clear();
            PromoteExcludeTop.TotalSelectedStudents = 0;
            PromoteExcludeTop.SelectAll = false;
            PromoteExcludeTop.Revert = false;
            PromoteExcludeTop.PromoteExclusion = false;
            DataSet ds = new DataSet();
            SqlCommand command = new SqlCommand("Usp_FindStudentEditPromoteExclude", con);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                command.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                command.Parameters.AddWithValue("@CurrentClass", (String.IsNullOrWhiteSpace(((object[])parameters)[0].ToString()) ? DBNull.Value : ((object[])parameters)[0]));
                command.Parameters.AddWithValue("@Section", (String.IsNullOrWhiteSpace(((object[])parameters)[1].ToString()) ? DBNull.Value : ((object[])parameters)[1]));
                command.Parameters.AddWithValue("@Type", (String.IsNullOrWhiteSpace(Type) ? DBNull.Value : (object)Type));
                
                da.SelectCommand = command;
                da.Fill(ds);
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    var obj = new PromoteExcludeModel();
                    obj.SNo = Convert.ToInt32(row["S.No"].ToString());
                    obj.AdminNo = (int)row["AdminNo"];
                    obj.StudentName = row["Name"].ToString();
                    obj.FName = row["FName"].ToString();
                    obj.Address = row["Address"].ToString();
                    obj.CurrentClass = row["CurrentClass"].ToString();
                    obj.Section = row["Section"].ToString();
                    obj.Action = row["Action"].ToString();
                    SearchedStudents.Add(obj);
                }
                PromoteExcludeTop.TotalStudents = ds.Tables[0].Rows.Count;
            }
            catch
            { 

            }
            finally
            {
                command.Dispose();
            }
        }
        void OnRevertExecuted(object parameters)
        {
            var ToUpdateObject = SearchedStudents.Where(c => c.Chk_Student == true);
            try
            {
                
                con.Open();
                foreach (PromoteExcludeModel SelectedStudent in ToUpdateObject)
                {
                    string Action = SelectedStudent.Action;
                    if (Action == "Failed")
                        Action = "Detained";
                    SqlCommand cmd1 = new SqlCommand("Usp_RevertPromoteExclusion", con);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    //cmd1.Parameters.AddWithValue("@UpdatedStatus", ((object)SelectedStudent.AdminNo ?? DBNull.Value));
                    cmd1.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                    cmd1.Parameters.AddWithValue("@AdminNo", ((object)SelectedStudent.AdminNo ?? DBNull.Value));
                    cmd1.Parameters.AddWithValue("@CurrentClass", ((object)SelectedStudent.CurrentClass ?? DBNull.Value));
                    cmd1.Parameters.AddWithValue("@Action", ((object)Action ?? DBNull.Value));
                    cmd1.Parameters.AddWithValue("@UpdatedBy", ((object)"SystemRevert" ?? DBNull.Value));
                    cmd1.ExecuteNonQuery();
                }
                object[] input = {PromoteExcludeTop.SelectedClass,PromoteExcludeTop.SelectedSection,PromoteExcludeTop.SelectedType};
                SearchStudentCommand.Execute(input);
            }
            catch (SqlException ex)
            {
                con.Close();
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
        void OnChkAllExecuted(object parameters)
        {
            SearchedStudents.Select(c => { c.Chk_Student = (bool)((object[])(parameters))[0]; return c; }).ToList();
            PromoteExcludeTop.TotalSelectedStudents = (bool)((object[])(parameters))[0] ? SearchedStudents.Count : 0;
            //PromoteExcludeTop.PromoteExclusion = PromoteExcludeTop.TotalSelectedStudents > 0 ? true : false;
            //PromoteExcludeModel p = SearchedStudents.Where(c => (c.CurrentClass.Trim() == "12" && c.Chk_Student == true)).FirstOrDefault();
            //PromoteExcludeModel np = SearchedStudents.Where(c => (c.CurrentClass.Trim() != "12" && c.Chk_Student == true)).FirstOrDefault();
            //PromoteExcludeTop.PromoteExclusion = (PromoteExcludeTop.TotalSelectedStudents > 0 && p == null) ? true : false;
            PromoteExcludeTop.Revert = PromoteExcludeTop.TotalSelectedStudents > 0 ? true : false;
            //PromoteExcludeTop.PO12 = (PromoteExcludeTop.TotalSelectedStudents > 0 && np == null) ? true : false;
        }
        void OnCheckBoxSelectedExecuted(object parameters)
        {
            //PromoteExcludeModel p = SearchedStudents.Where(c => (c.CurrentClass.Trim() == "12" && c.Chk_Student == true)).FirstOrDefault();
            //PromoteExcludeModel np = SearchedStudents.Where(c => (c.CurrentClass.Trim() != "12" && c.Chk_Student == true)).FirstOrDefault();
            PromoteExcludeTop.TotalSelectedStudents = (bool)((object[])(parameters))[1] ? PromoteExcludeTop.TotalSelectedStudents + 1 : PromoteExcludeTop.TotalSelectedStudents - 1;
            PromoteExcludeTop.Revert = (PromoteExcludeTop.TotalSelectedStudents > 0 ) ? true : false;

        }
    }
}

