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
    public partial class IDCard : UserControl
    {

        public IDCard()
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
            bool? IsVertical = chkIsV.IsChecked;
            if(IsVertical==false)
            {
                DataSet ds1 = GetData(0);
                ReportDataSource reportDataSource1 = new ReportDataSource("DataSet1", ds1.Tables[0]);
                ReportDataSource reportDataSource2 = new ReportDataSource("DataSet2", ds1.Tables[1]);
                reportViewer.LocalReport.DataSources.Add(reportDataSource1);
                reportViewer.LocalReport.DataSources.Add(reportDataSource2);
                reportViewer.LocalReport.ReportPath = "IDCard_H.rdlc";
                //reportViewer.LocalReport.ReportEmbeddedResource = "ViewSwitchingNavigation.Reports.Reports.IDCard_H.rdlc";
            }
            else
            {
                DataSet ds2 = GetData(1);
                ReportDataSource reportDataSource1 = new ReportDataSource("DataSet1", ds2.Tables[0]);
                ReportDataSource reportDataSource2 = new ReportDataSource("DataSet2", ds2.Tables[1]);
                ReportDataSource reportDataSource3 = new ReportDataSource("DataSet3", ds2.Tables[2]);
                reportViewer.LocalReport.DataSources.Add(reportDataSource1);
                reportViewer.LocalReport.DataSources.Add(reportDataSource2);
                reportViewer.LocalReport.DataSources.Add(reportDataSource3);
                reportViewer.LocalReport.ReportPath = "IDCard_V.rdlc";
                //reportViewer.LocalReport.ReportEmbeddedResource = "ViewSwitchingNavigation.Reports.Reports.IDCard_V.rdlc";
            }
            reportViewer.RefreshReport();
        }
        private DataSet GetData(int part)
        {
            DataSet ds = new DataSet();
            //DataTable dt = new DataTable();
            string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
            object SClass = cmbClass.SelectedValue;
            object SSec = cmbSec.SelectedValue;
            object FrmAdminNo = txtFromAdminNo.Text;
            object ToAdminNo = txtToAdminNo.Text;
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("Usp_IDCard", cn);
                cmd.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@Class", (String.IsNullOrWhiteSpace(SClass.ToString()) ? DBNull.Value : SClass));
                cmd.Parameters.AddWithValue("@Section", (String.IsNullOrWhiteSpace(SSec.ToString()) ? DBNull.Value : SSec));
                cmd.Parameters.AddWithValue("@FromAdminNo", (String.IsNullOrWhiteSpace(FrmAdminNo.ToString()) ? DBNull.Value : FrmAdminNo));
                cmd.Parameters.AddWithValue("@ToAdminNo", (String.IsNullOrWhiteSpace(ToAdminNo.ToString()) ? DBNull.Value : ToAdminNo));
                cmd.Parameters.AddWithValue("@Part", part);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);
            }
            return ds;
        }
    }
}
