using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using ViewSwitchingNavigation.Email.Model;
using ViewSwitchingNavigation.Infrastructure;

namespace ViewSwitchingNavigation.Email.Views
{
    /// <summary>
    /// Interaction logic for SearchTCView.xaml
    /// </summary>
    public partial class SearchTCView : Window
    {
        public static string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
        SqlConnection con = new SqlConnection(connectionString);
        public SearchTCOutputModel SelectedItem { get; set; }
        public List<string> Classes1 { get; private set; }
        //public IEnumerable<string> SectionList { get; private set; }
        public ObservableCollection<SearchTCOutputModel> SearchedStudents { get; set; }
        public SearchTCView()
        {
            InitializeComponent();
            this.Classes1 = new List<string>();
            //this.SectionList = new[] { DBNull.Value.ToString(), "A", "B", "C", "D" };
            this.SelectedItem = new SearchTCOutputModel();
            this.SearchedStudents = new ObservableCollection<SearchTCOutputModel>();
            int FinancialYear = AuthenticationContext.GlobalFinancialYear;
            DataSet dc = new DataSet();

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
            cmbClass.ItemsSource = Classes1;
            //cmbSection.ItemsSource = SectionList;
        }
        protected void HandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectedItem = (SearchTCOutputModel)lstViewSearchedStudent.SelectedItem;
            this.Close();
        }        
        private void btn_SearchClick(object sender, RoutedEventArgs e)
        {
            string SelectedClass = "";
            if (cmbClass.SelectedItem != null)
                SelectedClass = cmbClass.SelectedItem.ToString();
            SearchedStudents.Clear();
            DataSet ds = new DataSet();
            SqlCommand command = new SqlCommand("Usp_FindTC", con);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                command.Parameters.AddWithValue("@AdminNo",String.IsNullOrWhiteSpace(txtAdminNo.Text) ? (object)DBNull.Value : (object)txtAdminNo.Text);
                command.Parameters.AddWithValue("@SNo", String.IsNullOrWhiteSpace(txtTcSNo.Text) ? (object)DBNull.Value : (object)txtTcSNo.Text);
                command.Parameters.AddWithValue("@SName", String.IsNullOrWhiteSpace(txtSName.Text) ? (object)DBNull.Value : (object)txtSName.Text);
                command.Parameters.AddWithValue("@FName", String.IsNullOrWhiteSpace(txtFatherName.Text) ? (object)DBNull.Value : (object)txtFatherName.Text);
                command.Parameters.AddWithValue("@Class", String.IsNullOrWhiteSpace(SelectedClass.ToString()) ? (object)DBNull.Value : (object)SelectedClass.ToString());
                da.SelectCommand = command;
                da.Fill(ds);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var obj = new SearchTCOutputModel();
                    obj.Admino = (int)row["AdminNo"];
                    obj.SNo=(int)row["SNo"];
                    obj.Name = row["Name"] == DBNull.Value ? "" : (string)row["Name"];
                    obj.FName = row["FName"] == DBNull.Value ? "" : (string)row["FName"];
                    obj.MName = row["MName"] == DBNull.Value ? "" : (string)row["MName"];
                    obj.Class = row["CurrentClass"] == DBNull.Value ? "" : (string)row["CurrentClass"];
                    SearchedStudents.Add(obj);
                }
                lstViewSearchedStudent.ItemsSource = SearchedStudents;          
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
