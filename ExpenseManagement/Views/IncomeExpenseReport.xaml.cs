using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace ViewSwitchingNavigation.ExpenseManagement.Views
{
    /// <summary>
    /// Interaction logic for IncomeExpenseReport.xaml
    /// </summary>
    public partial class IncomeExpenseReport : UserControl
    {
        public IncomeExpenseReport()
        {
            InitializeComponent();
            //reportViewer.Reset();
            //DateTime frmdt = DateTime.Parse(dtPickerFrmDt.Text);
            //DateTime todt = DateTime.Parse(dtPickerToDt.Text);
            //DataTable dt = GetData(frmdt, todt);
            //DataColumn dc = new DataColumn("RunningBalance");
            //dt.Columns.Add(dc);
            //if (dt.Rows.Count != 0)
            //{
            //    int runningTotal = 0;
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        runningTotal = runningTotal + (int)dr["Income"] - (int)dr["Expense"];
            //        dr["RunningBalance"] = runningTotal;
            //    }
            //}
            //ReportDataSource reportDataSource = new ReportDataSource("DataSet", dt);
            //reportViewer.LocalReport.DataSources.Add(reportDataSource);
            //reportViewer.LocalReport.ReportEmbeddedResource = "ViewSwitchingNavigation.Reports.Reports.FeeTransactionReport.rdlc";
            //reportViewer.RefreshReport();
            
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            reportViewer.Reset();
            DateTime frmdt = DateTime.Parse(dtPickerFrmDt.Text);
            DateTime todt = DateTime.Parse(dtPickerToDt.Text);
            DataTable dt = GetData(frmdt,todt);
            DataColumn dc=new DataColumn("RunningBalance");
            dt.Columns.Add(dc);
            if (dt.Rows.Count != 0)
            {
                int runningTotal = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    runningTotal = runningTotal + (int)dr["Income"] - (int)dr["Expense"];
                    dr["RunningBalance"] = runningTotal;
                }
            }
            ReportDataSource reportDataSource = new ReportDataSource("DataSet1", dt);
            reportViewer.LocalReport.DataSources.Add(reportDataSource);
            reportViewer.LocalReport.ReportEmbeddedResource = "ViewSwitchingNavigation.ExpenseManagement.FromToIncomeExpenceReport.rdlc";   
            reportViewer.RefreshReport();
        }
        private DataTable GetData(DateTime FrmDate, DateTime ToDate)
        {
            DataTable dt = new DataTable();
            string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("Usp_GetIncomeExpenseByDate", cn);
                cmd.Parameters.AddWithValue("@IEDate", (object)ToDate);
                cmd.Parameters.AddWithValue("@FrmIEDate", (object)FrmDate);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            return dt;
        }
    }
}
