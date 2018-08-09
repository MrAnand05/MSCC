using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace ViewSwitchingNavigation.ExpenseManagement.Views
{
    /// <summary>
    /// Description for IncomeExpensePrint.
    /// </summary>
    public partial class IncomeExpensePrint : Window
    {
        /// <summary>
        /// Initializes a new instance of the IncomeExpensePrint class.
        /// </summary>
        public IncomeExpensePrint(object parameters)
        {
            InitializeComponent();
            reportViewer.Reset();
            DateTime SelectedDate =(DateTime)parameters;
            DataTable dt = GetData(SelectedDate);
            ReportDataSource reportDataSource = new ReportDataSource("DataSet1", dt);
            reportViewer.LocalReport.DataSources.Add(reportDataSource);
            reportViewer.LocalReport.ReportEmbeddedResource = "ViewSwitchingNavigation.ExpenseManagement.IncomeExpenceReport.rdlc";
            reportViewer.RefreshReport();
        }
        private DataTable GetData(DateTime SelectedDate)
        {
            DataTable dt = new DataTable();
            string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("Usp_GetIncomeExpenseByDate", cn);
                cmd.Parameters.AddWithValue("@IEDate", (object)SelectedDate);
                cmd.Parameters.AddWithValue("@FrmIEDate", ((object)SelectedDate ?? DBNull.Value));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            return dt;
        }
    }
}