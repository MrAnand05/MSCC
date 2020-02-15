using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using ViewSwitchingNavigation.Infrastructure;

namespace ViewSwitchingNavigation.Reports.Views
{
    /// <summary>
    /// Interaction logic for TransactionsReportR.xaml
    /// </summary>
    public partial class TransactionsReportR : UserControl
    {
        public TransactionsReportR()
        {
            InitializeComponent();
            
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            reportViewer.Reset();
            DataTable dt = GetData();
            ReportDataSource reportDataSource = new ReportDataSource("DataSet2", dt);
            //Added By Anand For second Tablix Start
            DataTable dt2 = GetData2();
            ReportDataSource reportDataSource2 = new ReportDataSource("DataSet1", dt2);
            reportViewer.LocalReport.DataSources.Add(reportDataSource2);
            //End
            reportViewer.LocalReport.DataSources.Add(reportDataSource);
            reportViewer.LocalReport.ReportPath = "FeeTransactionReport.rdlc";
            //reportViewer.LocalReport.ReportEmbeddedResource = "ViewSwitchingNavigation.Reports.Reports.FeeTransactionReport.rdlc";   
            reportViewer.RefreshReport();
        }
        private DataTable GetData()
        {
            DataTable dt = new DataTable();
            string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
            DateTime frmdt = DateTime.Parse(dtPickerFrmDt.Text);
            DateTime todt = DateTime.Parse(dtPickerToDt.Text);
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("Usp_GetFeeTransaction", cn);
                cmd.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@FrmDate",((object)frmdt ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@ToDate", ((object)todt ?? DBNull.Value));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            return dt;
        }
        private DataTable GetData2()
        {
            DataTable dt = new DataTable();
            string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
            DateTime frmdt = DateTime.Parse(dtPickerFrmDt.Text);
            DateTime todt = DateTime.Parse(dtPickerToDt.Text);
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("Usp_GetFeeTransactionForUpdates", cn);
                cmd.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@FrmDate", ((object)frmdt ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@ToDate", ((object)todt ?? DBNull.Value));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            return dt;
        }
    }
}
