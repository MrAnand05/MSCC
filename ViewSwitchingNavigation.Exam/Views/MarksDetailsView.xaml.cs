using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using ViewSwitchingNavigation.Infrastructure;

namespace ViewSwitchingNavigation.Exam.Views
{
    /// <summary>
    /// Interaction logic for ClassFeeConfigurationView.xaml
    /// </summary>

    public class ComboBoxPairs
    {
        public int _Key { get; set; }
        public string _Value { get; set; }
        public ComboBoxPairs(int _key, string _value)
        {
            _Key = _key;
            _Value = _value;
        }
    }
    public partial class MarksDetailsView : UserControl
    {
        public static string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
        public List<string> Classes1 { get; private set; }
        public List<string> TermL = new List<string>(){"1","2"};
        public List<string> SectionL = new List<string>() { "", "A","B","C","D" };

        List<ComboBoxPairs> cbp = new List<ComboBoxPairs>();
        DataSet ds1 = new DataSet();
        DataTable dt3 = new DataTable();
        public MarksDetailsView()
        {
            InitializeComponent();
            cbp.Add(new ComboBoxPairs(1, "Oral(MM-30)"));
            cbp.Add(new ComboBoxPairs(2, "Written(MM-70)"));
            DataSet ds = new DataSet();
            this.Classes1 = new List<string>();
            SqlConnection con2 = new SqlConnection(connectionString);
            SqlCommand command1 = new SqlCommand("Usp_ClassFeeRelation", con2);
            command1.CommandType = CommandType.StoredProcedure;
            command1.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                da.SelectCommand = command1;
                da.Fill(ds);
            }
            catch
            {

            }
            finally
            {
                command1.Dispose();
                con2.Close();
            }
            Classes1.Add("");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Classes1.Add(dr["Class"].ToString());
            }
            CmbClass.ItemsSource = Classes1;
            CmbTerm.ItemsSource = TermL;
            CmbSection.ItemsSource = SectionL;
            CmbWrOr.DisplayMemberPath = "_Value"; 
            CmbWrOr.SelectedValuePath = "_Key";
            CmbWrOr.ItemsSource = cbp;
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (CmbClass.SelectedIndex == 0 )
            {
                MessageBox.Show("Please select class");
            }
            else
            {
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand("Usp_StudentMarks", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                command.Parameters.AddWithValue("@Class", CmbClass.SelectedValue.ToString());
                command.Parameters.AddWithValue("@Section", (String.IsNullOrWhiteSpace(CmbSection.SelectedValue.ToString()) ? DBNull.Value : CmbSection.SelectedValue)); 
                command.Parameters.AddWithValue("@Term", CmbTerm.SelectedValue.ToString());
                command.Parameters.AddWithValue("@MD", CmbWrOr.SelectedValue.ToString());

                SqlDataAdapter da1 = new SqlDataAdapter();
                try
                {
                    ds1.Tables.Clear();
                    dt3.Clear();
                    da1.SelectCommand = command;
                    da1.Fill(ds1);

                }
                catch
                {

                }
                finally
                {
                    command.Dispose();
                }
                if (ds1.Tables.Count != 0)
                {
                    ds1.Tables[0].Columns[0].ReadOnly = true;
                    ds1.Tables[0].Columns[1].ReadOnly = true;
                    ds1.Tables[0].Columns[2].ReadOnly = true;
                    grd.ItemsSource = ds1.Tables[0].AsDataView();
                    dt3 = ds1.Tables[0].Copy();
                    grd.CanUserSortColumns = false;
                    grd.CanUserReorderColumns = false;
                    //grd.Columns[1].IsReadOnly = true;
                    grd.CanUserAddRows = false;
                }
                else
                {
                    grd.ItemsSource = "";
                    MessageBox.Show("No Subject Assigned to class");
                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string adminNo;
            int colCount;
            string subId;
            string marks;
            string term;
            string MD;
            string cClass;
            DataTable dt = new DataTable();
            dt.Clear();
            if (grd.ItemsSource == "")
            {
            }
            else
            {
                dt = ((DataView)grd.ItemsSource).ToTable();
                //dt1 = dt3;
                colCount = dt.Columns.Count - 3;
                term = dt.Rows[0][colCount + 1].ToString();
                MD = dt.Rows[0][colCount + 2].ToString();//Last Row or retrieved table
                cClass = dt.Rows[0][colCount].ToString();
                foreach (DataRow dr in dt.Rows)
                {
                    adminNo = dr[0].ToString();
                    int index = dt.Rows.IndexOf(dr);
                    SqlConnection con = new SqlConnection(connectionString);
                    try
                    {
                        con.Open();
                        for (int i = 3; i < colCount; i++)
                        {
                            if (dr[i].ToString() != dt3.Rows[index][i].ToString())
                            {
                                subId = dt.Columns[i].Caption.Substring(0, 2);
                                if (dr[i].ToString() != "")
                                {
                                    marks = dr[i].ToString();
                                    SqlCommand cmd = new SqlCommand("Usp_SaveUpdateStudentMarks", con);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    //cmd.Parameters.AddWithValue("@IsDatafromDB", ((object)IsDatafromDB ?? DBNull.Value));
                                    cmd.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                                    cmd.Parameters.AddWithValue("@AdminNo", adminNo);
                                    cmd.Parameters.AddWithValue("@Class", cClass);
                                    cmd.Parameters.AddWithValue("@SubId", subId);
                                    cmd.Parameters.AddWithValue("@Marks", marks);
                                    cmd.Parameters.AddWithValue("@Term", term);
                                    cmd.Parameters.AddWithValue("@MD", MD);
                                    cmd.ExecuteScalar();
                                }
                            }
                        }
                    }
                    catch (SqlException ex)
                    {

                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
        }
        private void grd_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "AdminNo" || e.Column.Header.ToString() == "cClass" || e.Column.Header.ToString() == "Term" || e.Column.Header.ToString() == "MD")
            {
                e.Cancel = true;
            }
        }
    }
}