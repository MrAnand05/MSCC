using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using ViewSwitchingNavigation.Email.Model;
using ViewSwitchingNavigation.Email.Views;
using ViewSwitchingNavigation.Infrastructure;

namespace ViewSwitchingNavigation.Reports.Views
{
    /// <summary>
    /// Interaction logic for FeeBalance.xaml
    /// </summary>
    public partial class TC : UserControl
    {
        public SearchStudentOutputModel returnedStudent { get; set; }
        public SearchTCOutputModel returnedTC { get; set; }
        bool IsGenerate = false;
        int? SNo = null;
        public TC()
        {
            InitializeComponent();
        }
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            IsGenerate = false;
            SNo = null;
            btnGenerate.IsEnabled = false;
            SearchStudentViewTest vm = new SearchStudentViewTest();
            vm.ShowDialog();
            returnedStudent = vm.SelectedItem;
            if (returnedStudent != null)
            {
                reportViewer.Reset();
                DataTable dt = GetData(returnedStudent.Admino);
                ReportDataSource reportDataSource = new ReportDataSource("DataSet1", dt);
                reportViewer.LocalReport.DataSources.Add(reportDataSource);
                reportViewer.LocalReport.ReportPath = "TC.rdlc";
                //reportViewer.LocalReport.ReportEmbeddedResource = "ViewSwitchingNavigation.Reports.Reports.TC.rdlc";
                //reportViewer.LocalReport.ReportEmbeddedResource = "ViewSwitchingNavigation.Reports.Reports.FeeBalance.rdlc";
                reportViewer.RefreshReport();
                btnGenerate.IsEnabled = true;
            }
            else
                MessageBox.Show("Please double click Student to Select");
            if (returnedStudent.Admino == null)
                btnGenerate.IsEnabled = false;
        }
        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            btnGenerate.IsEnabled = false;
            SNo = null;
            IsGenerate = true;
            reportViewer.Reset();
            DataTable dt = GetData(returnedStudent.Admino);
            ReportDataSource reportDataSource = new ReportDataSource("DataSet1", dt);
            reportViewer.LocalReport.DataSources.Add(reportDataSource);
            reportViewer.LocalReport.ReportEmbeddedResource = "ViewSwitchingNavigation.Reports.Reports.TC.rdlc";
            //reportViewer.LocalReport.ReportEmbeddedResource = "ViewSwitchingNavigation.Reports.Reports.FeeBalance.rdlc";
            reportViewer.RefreshReport();
            //btnGenerate.IsEnabled = true;
        }
        private void SearchTC_Click(object sender, RoutedEventArgs e)
        {
            btnGenerate.IsEnabled = false;
            SearchTCView vm = new SearchTCView();
            vm.ShowDialog();
            returnedTC = vm.SelectedItem;
            if (returnedTC != null)
            {
                IsGenerate = false;
                SNo = returnedTC.SNo;
                reportViewer.Reset();
                DataTable dt = GetData(returnedTC.Admino);
                ReportDataSource reportDataSource = new ReportDataSource("DataSet1", dt);
                reportViewer.LocalReport.DataSources.Add(reportDataSource);
                reportViewer.LocalReport.ReportEmbeddedResource = "ViewSwitchingNavigation.Reports.Reports.TC.rdlc";
                //reportViewer.LocalReport.ReportEmbeddedResource = "ViewSwitchingNavigation.Reports.Reports.FeeBalance.rdlc";
                reportViewer.RefreshReport();
            }
            else
                MessageBox.Show("Please double click Student to Select");

        }
        private DataTable GetData(int? AdminNo)
        {
            DataTable dt = new DataTable();
            string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("Usp_TC", cn);
                cmd.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@SNo", ((object)SNo ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@AdminNo", ((object)AdminNo ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@NoWorkingDay", ((object)txtWrkDay.Text ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@PresentDay", ((object)txtPrsntDay.Text ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@DOA", ((object)dtPickerAppDt.SelectedDate ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@DOI", ((object)dtPickerIssueDt.SelectedDate ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@ISGenerate", ((object)IsGenerate ?? DBNull.Value));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            return dt;
        }
    }
}
