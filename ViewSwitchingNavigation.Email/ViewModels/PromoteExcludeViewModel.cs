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
    public class PromoteExcludeViewModel : BindableBase
    {
        public IEnumerable<string> SectionList { get; private set; }
        public List<string> Classes1 { get; private set; }
        public PromoteExcludeTopModel PromoteExcludeTop { get; set; }
        public ObservableCollection<PromoteExcludeModel> SearchedStudents { get; set; }
        public DelegateCommand<object> ckhCheckedCommand { get; private set; }
        public DelegateCommand<object> PromoteStudentsCommand { get; private set; }
        public DelegateCommand<object> chkSelectAllCommand { get; private set; } 
        public static string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
        SqlConnection con = new SqlConnection(connectionString);
        DataSet ds = new DataSet();
        public ICommand SearchStudentCommand { get; private set; }
        public PromoteExcludeViewModel()
        {
            this.SectionList = new[] { DBNull.Value.ToString(), "A", "B", "C", "D" };
            this.PromoteExcludeTop = new PromoteExcludeTopModel();
            PromoteExcludeTop.SelectAll = false;
            PromoteExcludeTop.TotalStudents = 0;
            PromoteExcludeTop.TotalSelectedStudents = 0;
            this.Classes1 = new List<string>();
            this.SearchedStudents = new ObservableCollection<PromoteExcludeModel>();
            this.SearchStudentCommand = new DelegateCommand<object>(this.Search);
            this.ckhCheckedCommand = new DelegateCommand<object>(OnCheckBoxSelectedExecuted);
            this.PromoteStudentsCommand = new DelegateCommand<object>(OnPromoteExecuted);
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
            //string CurrentClass = (string)((object[])parameters)[0];
            //string Section = (string)((object[])parameters)[1];
            //int AdminNo= (int)((object[])parameters)[2];
            SearchedStudents.Clear();
            PromoteExcludeTop.TotalSelectedStudents = 0;
            PromoteExcludeTop.SelectAll = false;
            PromoteExcludeTop.Revert = false;
            PromoteExcludeTop.PromoteExclusion = false;
            DataSet ds = new DataSet();
            SqlCommand command = new SqlCommand("Usp_FindStudentForPromoteExclude", con);
            command.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                command.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                command.Parameters.AddWithValue("@CurrentClass", (String.IsNullOrWhiteSpace(((object[])parameters)[0].ToString()) ? DBNull.Value : ((object[])parameters)[0]));
                command.Parameters.AddWithValue("@Section", (String.IsNullOrWhiteSpace(((object[])parameters)[1].ToString()) ? DBNull.Value : ((object[])parameters)[1]));
                if (((object[])parameters)[2] == null)
                {
                    command.Parameters.AddWithValue("@AdminNo", ((object[])parameters)[2] ?? DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@AdminNo", (String.IsNullOrWhiteSpace(((object[])parameters)[2].ToString()) ? (object)DBNull.Value : ((object[])parameters)[2]));
                }
                da.SelectCommand = command;
                da.Fill(ds);
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    var obj = new PromoteExcludeModel();
                    //{
                    //    SNo = (int)row["S.No"],
                    //    AdminNo =(int)row["AdminNo"],
                    //    StudentName= row["Name"].ToString(),
                    //    FName = row["FName"].ToString(),
                    //    Address = row["Address"].ToString(),
                    //    CurrentClass = row["CurrentClass"].ToString(),
                    //    Section = row["Section"].ToString()
                    //};
                    obj.SNo = Convert.ToInt32(row["S.No"].ToString());
                    obj.AdminNo = (int)row["AdminNo"];
                    obj.StudentName = row["Name"].ToString();
                    obj.FName = row["FName"].ToString();
                    obj.Address = row["Address"].ToString();
                    obj.CurrentClass = row["CurrentClass"].ToString();
                    obj.Section = row["Section"].ToString();
                    SearchedStudents.Add(obj);
                }
                PromoteExcludeTop.TotalStudents = ds.Tables[0].Rows.Count;
                PromoteExcludeTop.Revert = PromoteExcludeTop.TotalStudents > 0 ? true : false;
            }
            catch
            { 

            }
            finally
            {
                command.Dispose();
            }
        }
        void OnPromoteExecuted(object parameters)
        {
            //ObservableCollection<PromoteExcludeModel> ToUpdateObject = new ObservableCollection<PromoteExcludeModel>();
            //ToUpdateObject = (ObservableCollection<PromoteExcludeModel>)SearchedStudents.Where(c => c.Chk_Student == true);
            var ToUpdateObject = SearchedStudents.Where(c => c.Chk_Student == true);
            try
            {
                con.Open();
                foreach (PromoteExcludeModel SelectedStudent in ToUpdateObject)
                {
                    SqlCommand cmd1 = new SqlCommand("Usp_PromoteStudents", con);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@UpdateStatus", ((object)parameters.ToString() ?? DBNull.Value));
                    cmd1.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                    cmd1.Parameters.AddWithValue("@AdminNo", ((object)SelectedStudent.AdminNo ?? DBNull.Value));
                    cmd1.Parameters.AddWithValue("@CurrentClass", ((object)SelectedStudent.CurrentClass ?? DBNull.Value));
                    //cmd1.Parameters.AddWithValue("@EnteredBy", ((object)"" ?? DBNull.Value));
                    cmd1.Parameters.AddWithValue("@UpdatedBy", ((object)("System" + parameters.ToString()) ?? DBNull.Value));
                    cmd1.ExecuteNonQuery();
                }
                object[] input = {PromoteExcludeTop.SelectedClass,PromoteExcludeTop.SelectedSection,PromoteExcludeTop.AdminNo};
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
            PromoteExcludeModel p = SearchedStudents.Where(c => (c.CurrentClass.Trim() == "12" && c.Chk_Student == true)).FirstOrDefault();
            PromoteExcludeModel np = SearchedStudents.Where(c => (c.CurrentClass.Trim() != "12" && c.Chk_Student == true)).FirstOrDefault();
            PromoteExcludeTop.PromoteExclusion = (PromoteExcludeTop.TotalSelectedStudents > 0 && p == null) ? true : false;
            PromoteExcludeTop.FailLS = PromoteExcludeTop.TotalSelectedStudents > 0 ? true : false;
            PromoteExcludeTop.PO12 = (PromoteExcludeTop.TotalSelectedStudents > 0 && np == null) ? true : false;
        }
        void OnCheckBoxSelectedExecuted(object parameters)
        {
            PromoteExcludeModel p = SearchedStudents.Where(c => (c.CurrentClass.Trim() == "12" && c.Chk_Student == true)).FirstOrDefault();
            PromoteExcludeModel np = SearchedStudents.Where(c => (c.CurrentClass.Trim() != "12" && c.Chk_Student == true)).FirstOrDefault();
            PromoteExcludeTop.TotalSelectedStudents = (bool)((object[])(parameters))[1] ? PromoteExcludeTop.TotalSelectedStudents + 1 : PromoteExcludeTop.TotalSelectedStudents - 1;
            PromoteExcludeTop.PromoteExclusion = (PromoteExcludeTop.TotalSelectedStudents > 0 && p == null) ? true : false;
            PromoteExcludeTop.FailLS = PromoteExcludeTop.TotalSelectedStudents > 0 ? true : false;
            PromoteExcludeTop.PO12 = (PromoteExcludeTop.TotalSelectedStudents > 0 && np == null) ? true : false;

        }
    }
}

