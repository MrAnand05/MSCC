using Microsoft.Reporting.WinForms;
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
    /// Interaction logic for FeeBalance.xaml
    /// </summary>
    public partial class ReportCard : UserControl
    {

        public ReportCard()
        {
            InitializeComponent();
            List<string> LSClass = new List<string>() {"","000-Nursery","001-LKG","002-UKG","01","02","03","04","05","06","07","08","09","10","11","12" };
            List<string> LSSection = new List<string>() {"","A","B","C","D" };
            cmbClass.ItemsSource = LSClass;
            cmbSec.ItemsSource = LSSection;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            reportViewer.Reset();
            DataTable dt = GetData();
            ReportDataSource reportDataSource = new ReportDataSource("DataSet1", dt);
            reportViewer.LocalReport.DataSources.Add(reportDataSource);
            reportViewer.LocalReport.ReportEmbeddedResource = "ViewSwitchingNavigation.Exam.ReportCard.rdlc";
            reportViewer.RefreshReport();
        }
        private DataTable GetData()
        {
            DataTable dt = new DataTable();
            string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
            object SClass = cmbClass.SelectedValue;
            object SSec = cmbSec.SelectedValue;
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("Usp_ReportCard", cn);
                cmd.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@CClass", (String.IsNullOrWhiteSpace(SClass.ToString()) ? DBNull.Value:SClass));
                cmd.Parameters.AddWithValue("@Section", (String.IsNullOrWhiteSpace(SSec.ToString()) ? DBNull.Value : SSec));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            //DataColumn newColumn = new DataColumn("ExamType", typeof(String));
            //newColumn.DefaultValue = txtExam.Text.ToString();
            //dt.Columns.Add(newColumn);
            //dt.Columns.Add("ExamType", typeof(String),txtExam.Text.ToString());
            return dt;
        }
    }
}
