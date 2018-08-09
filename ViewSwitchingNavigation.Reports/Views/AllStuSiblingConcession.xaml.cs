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
    /// Interaction logic for FeeBalance.xaml
    /// </summary>
    public partial class AllStuSiblingConcession : UserControl
    {
        public AllStuSiblingConcession()
        {
            InitializeComponent();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
                reportViewer.Reset();
                DataTable dt = GetData();
                ReportDataSource reportDataSource = new ReportDataSource("DataSet1", dt);
                reportViewer.LocalReport.DataSources.Add(reportDataSource);
                reportViewer.LocalReport.ReportPath = "AllStuSibConReport.rdlc";
                //reportViewer.LocalReport.ReportEmbeddedResource = "ViewSwitchingNavigation.Reports.Reports.AllStuSibConReport.rdlc";
                //reportViewer.LocalReport.ReportEmbeddedResource = "ViewSwitchingNavigation.Reports.Reports.FeeBalance.rdlc";
                reportViewer.RefreshReport();
        }
        private DataTable GetData()
        {
            DataTable dt = new DataTable();
            string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("Usp_AllSiblingConcessionStudent", cn);
                cmd.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            return dt;
        }
    }
}
