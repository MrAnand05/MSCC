using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
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
    public partial class AllFeeSlipIndividual : UserControl
    {
        public SearchStudentOutputModel returnedStudent { get; set; }
        public AllFeeSlipIndividual()
        {
            InitializeComponent();
            List<int> LSFinancialYear= new List<int>() { 15,16,17,18,19,20};
            cmbFinancialYear.ItemsSource = LSFinancialYear;
            cmbFinancialYear.SelectedValue = AuthenticationContext.GlobalFinancialYear;
        }

        
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            SearchStudentViewTest vm = new SearchStudentViewTest();
            vm.ShowDialog();
            returnedStudent = vm.SelectedItem;
            if (returnedStudent != null)
            {
                reportViewer.Reset();
                DataTable dt = GetData(returnedStudent.Admino);
                ReportDataSource reportDataSource = new ReportDataSource("DataSet1", dt);
                reportViewer.LocalReport.DataSources.Add(reportDataSource);
                reportViewer.LocalReport.ReportPath = "AllFeeSlipIndividual.rdlc";
                //reportViewer.LocalReport.ReportEmbeddedResource = "ViewSwitchingNavigation.Reports.Reports.AllFeeSlipIndividual.rdlc";
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
                SqlCommand cmd = new SqlCommand("Usp_AllPaymentByAdminNo", cn);
                cmd.Parameters.AddWithValue("@FinancialYear", ((object)cmbFinancialYear.SelectedValue ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@AdminNo", ((object)AdminNo ?? DBNull.Value));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            return dt;
        }
    }
}
