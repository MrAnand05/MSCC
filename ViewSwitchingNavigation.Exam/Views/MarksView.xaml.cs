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
    /// Interaction logic for MarksView.xaml
    /// </summary>
    public partial class MarksView : UserControl
    {

        public MarksView()
        {
            InitializeComponent();
            //List<string> LSClass = new List<string>() { "", "000-Nursery", "001-LKG", "002-UKG", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
            List<string> LSSection = new List<string>() {"","A","B","C","D" };
            List<string> TermL = new List<string>(){"1","2"};
            //cmbClass.ItemsSource = LSClass;
            cmbSec.ItemsSource = LSSection;
            cmbTerm.ItemsSource = TermL;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            reportViewer.Reset();
            if(cmbSub.SelectedIndex==-1)
            {
                MessageBox.Show("Please Select Subject");
            }
            else
            { 
                DataTable dt = GetData();
                ReportDataSource reportDataSource = new ReportDataSource("MarksReportDS", dt);
                reportViewer.LocalReport.DataSources.Add(reportDataSource);
                reportViewer.LocalReport.ReportEmbeddedResource = "ViewSwitchingNavigation.Exam.MarksReport.rdlc";
                reportViewer.RefreshReport();
            }
        }
        private DataTable GetData()
        {
            DataTable dt = new DataTable();
            string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
            //object SFinancialYr = (object)AuthenticationContext.GlobalFinancialYear;
            object SClass = cmbClass.SelectedValue;
            object SSec = cmbSec.SelectedValue;
            object SSub = cmbSub.SelectedValue;
            object STerm = cmbTerm.SelectedValue;
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("Usp_StudentMarksPrint", cn);
                cmd.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@Class", (String.IsNullOrWhiteSpace(SClass.ToString()) ? DBNull.Value : SClass));
                cmd.Parameters.AddWithValue("@Section", (String.IsNullOrWhiteSpace(SSec.ToString()) ? DBNull.Value : SSec));
                cmd.Parameters.AddWithValue("@Subject", (String.IsNullOrWhiteSpace(SSub.ToString()) ? DBNull.Value : SSub));
                cmd.Parameters.AddWithValue("@MD", (String.IsNullOrWhiteSpace(STerm.ToString()) ? DBNull.Value : STerm));
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
