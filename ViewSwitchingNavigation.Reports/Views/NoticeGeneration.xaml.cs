using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using ViewSwitchingNavigation.Infrastructure;

namespace ViewSwitchingNavigation.Reports.Views
{
    /// <summary>
    /// Interaction logic for FeeBalance.xaml
    /// </summary>
    public partial class NoticeGeneration : UserControl
    {

        public NoticeGeneration()
        {
            InitializeComponent();
            List<string> LSClass = new List<string>() {"","000-Nursery","001-LKG","002-UKG","01","02","03","04","05","06","07","08","09","10","11","12" };
            List<string> LSSection = new List<string>() {"","A","B","C","D" };
            List<string> LTMonth = new List<string>() {"","Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "Jan", "Feb", "Mar" };
            cmbClass.ItemsSource = LSClass;
            cmbSec.ItemsSource = LSSection;
            cmbTMonth.ItemsSource = LTMonth;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            reportViewer.Reset();
            DataTable dt1 = GetData(0);
            DataTable dt2 = GetData(1);
            ReportDataSource reportDataSource1 = new ReportDataSource("DataSet1", dt1);
            ReportDataSource reportDataSource2 = new ReportDataSource("DataSet2", dt2);
            reportViewer.LocalReport.DataSources.Add(reportDataSource1);
            reportViewer.LocalReport.DataSources.Add(reportDataSource2);
            reportViewer.LocalReport.ReportPath = "Notice.rdlc";
            //reportViewer.LocalReport.ReportEmbeddedResource = "ViewSwitchingNavigation.Reports.Reports.Notice.rdlc";
            reportViewer.RefreshReport();
        }
        private DataTable GetData(int part)
        {
            DataTable dt = new DataTable();
            string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
            object SClass = cmbClass.SelectedValue;
            object SSec = cmbSec.SelectedValue;
            object TMonth = cmbTMonth.SelectedValue;
            object Message = new TextRange(rtxtMsg.Document.ContentStart, rtxtMsg.Document.ContentEnd).Text;
            DateTime LastDate = DateTime.Parse(dtPickerLastDt.Text);
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("Usp_Notice", cn);
                cmd.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@Class", (String.IsNullOrWhiteSpace(SClass.ToString()) ? DBNull.Value:SClass));
                cmd.Parameters.AddWithValue("@Section", (String.IsNullOrWhiteSpace(SSec.ToString()) ? DBNull.Value : SSec));
                cmd.Parameters.AddWithValue("@TillMonth", (String.IsNullOrWhiteSpace(TMonth.ToString()) ? DBNull.Value : TMonth));
                cmd.Parameters.AddWithValue("@LastDate", ((object)LastDate ?? DBNull.Value));
                //cmd.Parameters.AddWithValue("@ToDate", ((object)todt ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@Message", (String.IsNullOrWhiteSpace(Message.ToString()) ? DBNull.Value : Message));
                cmd.Parameters.AddWithValue("@DuesAmount", DBNull.Value);
                cmd.Parameters.AddWithValue("@Part", part);
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
