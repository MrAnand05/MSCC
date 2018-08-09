using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using ViewSwitchingNavigation.Email.Model;
using ViewSwitchingNavigation.Infrastructure;

namespace ViewSwitchingNavigation.Email.ViewModels
{
    public class ObservableSearchedStudentOutput
    {
        public ObservableCollection<SearchStudentOutputModel> SearchedStudents { get; private set; }
        public ObservableSearchedStudentOutput(SearchStudentModel ste)
        {
            this.SearchedStudents = new ObservableCollection<SearchStudentOutputModel>();
            PopulateStudents(ste);
        }

        public static string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
        SqlConnection con = new SqlConnection(connectionString);
        DataSet ds = new DataSet();
        private void PopulateStudents(SearchStudentModel Ste)
        {
            SqlCommand command = new SqlCommand("Usp_FindStudent", con);
            command.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                command.Parameters.AddWithValue("@AdminNo", ((object)Ste.Admino ?? DBNull.Value));
                command.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                command.Parameters.AddWithValue("@FirstName", ((object)Ste.FirstName ?? DBNull.Value));
                command.Parameters.AddWithValue("@LastName", ((object)Ste.LastName ?? DBNull.Value));
                command.Parameters.AddWithValue("@RollNo", ((object)Ste.RollNo ?? DBNull.Value));
                command.Parameters.AddWithValue("@Class", ((object)Ste.Class ?? DBNull.Value));
                command.Parameters.AddWithValue("@Section", ((object)Ste.Section ?? DBNull.Value));
                command.Parameters.AddWithValue("@StartDate", ((object)Ste.StartDate ?? DBNull.Value));
                command.Parameters.AddWithValue("@EndDate", ((object)Ste.EndDate ?? DBNull.Value));
                da.SelectCommand = command;
                da.Fill(ds);
                foreach(DataRow row in ds.Tables[0].Rows)
                {
                    var obj = new SearchStudentOutputModel()
                    {
                        Admino=(int)row["AdminNo"],
                        FirstName=row["FirstName"].ToString()
                    };
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
    }
}
