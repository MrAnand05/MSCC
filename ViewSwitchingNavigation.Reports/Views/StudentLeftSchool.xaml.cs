using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using ViewSwitchingNavigation.Infrastructure;

namespace ViewSwitchingNavigation.Reports.Views
{
    /// <summary>
    /// Interaction logic for FeeBalance.xaml
    /// </summary>
    public partial class StudentLeftSchool : UserControl
    {

        public StudentLeftSchool()
        {
            InitializeComponent();
            List<string> LSFinancialYear = new List<string>() {"","15", "16", "17", "18", "19", "20" };
            List<string> LSType = new List<string>() { "12PassOut", "LeavingSchool" };
            cmbFinancialYear.ItemsSource = LSFinancialYear;
            //cmbFinancialYear.Items.Insert(0, 2);
            cmbType.ItemsSource = LSType;
            cmbFinancialYear.SelectedValue = AuthenticationContext.GlobalFinancialYear;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            reportViewer.Reset();
            DataTable dt = GetData();
            ReportDataSource reportDataSource = new ReportDataSource("DataSet1", dt);
            reportViewer.LocalReport.DataSources.Add(reportDataSource);
            reportViewer.LocalReport.ReportPath = "StudentLeftSchool.rdlc";
            //reportViewer.LocalReport.ReportEmbeddedResource = "ViewSwitchingNavigation.Reports.Reports.StudentLeftSchool.rdlc";
            reportViewer.RefreshReport();
        }
        private DataTable GetData()
        {
            DataTable dt = new DataTable();
            string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
            //object SClass = cmbClass.SelectedValue;
            //object SSec = cmbSec.SelectedValue;
            //object TMonth = cmbTMonth.SelectedValue;
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("Usp_StuLeftSchoolByAction", cn);
                cmd.Parameters.AddWithValue("@FinancialYear", ((object)cmbFinancialYear.SelectedValue ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@Action", cmbType.SelectedValue);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            return dt;
        }
    }
}
