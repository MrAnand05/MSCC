using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using ViewSwitchingNavigation.Infrastructure;

namespace ViewSwitchingNavigation.Reports.Views
{
    /// <summary>
    /// Description for IncomeExpensePrint.
    /// </summary>
    public partial class GenerateIdCard : Window
    {
        /// <summary>
        /// Initializes a new instance of the IncomeExpensePrint class.
        /// </summary>
        public GenerateIdCard(string selclass,string selsec,string selectedAdminNo, bool? IsChkV)
        {
            InitializeComponent();
            reportViewer.Reset();
            string SClass = selclass;
            string SSection = selsec;
            if(IsChkV==false)
            {
                DataSet ds1= GetData(0, SClass, SSection, selectedAdminNo);
                ReportDataSource reportDataSource1 = new ReportDataSource("DataSet1", ds1.Tables[0]);
                ReportDataSource reportDataSource2 = new ReportDataSource("DataSet2", ds1.Tables[1]);
                reportViewer.LocalReport.DataSources.Add(reportDataSource1);
                reportViewer.LocalReport.DataSources.Add(reportDataSource2);
                reportViewer.LocalReport.ReportPath = "IDCard_H.rdlc";
                //reportViewer.LocalReport.ReportEmbeddedResource = "ViewSwitchingNavigation.Reports.Reports.IDCard_H.rdlc";
            }
            else
            {
                DataSet ds2 = GetData(1, SClass, SSection, selectedAdminNo);
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
        private DataSet GetData(int part,string selectedClass, string selectedSection,string selectedAdminNo)
        {
            DataSet ds = new DataSet();
            string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
            object SClass = selectedClass;
            object SSec = selectedSection;
            object SAdminNo = selectedAdminNo;
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("Usp_IDCardSelected", cn);
                cmd.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@Class", (String.IsNullOrWhiteSpace(SClass.ToString()) ? DBNull.Value : SClass));
                cmd.Parameters.AddWithValue("@Section", (String.IsNullOrWhiteSpace(SSec.ToString()) ? DBNull.Value : SSec));
                cmd.Parameters.AddWithValue("@FromAdminNo",  DBNull.Value );
                cmd.Parameters.AddWithValue("@ToAdminNo", DBNull.Value );
                cmd.Parameters.AddWithValue("@SelectedAdminNo", (String.IsNullOrWhiteSpace(SAdminNo.ToString()) ? DBNull.Value : SAdminNo));
                cmd.Parameters.AddWithValue("@Part", part);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(ds);
            }
            return ds;
        }
    }
}