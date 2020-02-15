using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace ViewSwitchingNavigation.ExpenseManagement.Views
{
    /// <summary>
    /// Interaction logic for IncomeExpenseSourcewiseReport.xaml
    /// </summary>
    public partial class IncomeExpenseSourcewiseReport : UserControl
    {
        public static string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
        SqlConnection con = new SqlConnection(connectionString);
        DataSet ds1 = new DataSet();
        public IncomeExpenseSourcewiseReport()
        {
            InitializeComponent();
            List<string> cmbIESourceLst = new List<string>();
            cmbIESourceLst.Add("");
            SqlCommand command = new SqlCommand("Usp_InExpSourceList", con);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                da.SelectCommand = command;
                da.Fill(ds1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                command.Dispose();
            }
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                cmbIESourceLst.Add((string)dr["IncomeExpenseSource"]);
            }
            cmbIESource.ItemsSource = cmbIESourceLst;
            cmbIESource.SelectedIndex = 0;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            reportViewer.Reset();
            DateTime frmdt = DateTime.Parse(dtPickerFrmDt.Text);
            DateTime todt = DateTime.Parse(dtPickerToDt.Text);
            DataTable dt = GetData(frmdt,todt);
            ReportDataSource reportDataSource = new ReportDataSource("DataSet1", dt);
            reportViewer.LocalReport.DataSources.Add(reportDataSource);
            reportViewer.LocalReport.ReportEmbeddedResource = "ViewSwitchingNavigation.ExpenseManagement.ItemWiseExpence.rdlc";   
            reportViewer.RefreshReport();
        }
        private DataTable GetData(DateTime FrmDate, DateTime ToDate)
        {
            DataTable dt = new DataTable();
            string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
            object IESource = cmbIESource.SelectedValue;
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("Usp_GetIncomeExpenseByDateItem", cn);
                cmd.Parameters.AddWithValue("@IEDate", (object)ToDate);
                cmd.Parameters.AddWithValue("@FrmIEDate", (object)FrmDate);
                cmd.Parameters.AddWithValue("@IESource", (String.IsNullOrWhiteSpace(IESource.ToString()) ? DBNull.Value : IESource));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            return dt;
        }
    }
}
