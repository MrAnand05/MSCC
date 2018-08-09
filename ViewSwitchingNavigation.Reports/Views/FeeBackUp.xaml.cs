﻿using Microsoft.Reporting.WinForms;
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
    public partial class FeeBackUp : UserControl
    {

        public FeeBackUp()
        {
            InitializeComponent();
            List<string> LSClass = new List<string>() {"","000-Nursery","001-LKG","002-UKG","01","02","03","04","05","06","07","08","09","10","11","12" };
            List<string> LSSection = new List<string>() {"","A","B","C","D" };
            //List<string> LTMonth = new List<string>() {"Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "Jan", "Feb", "Mar" };
            cmbClass.ItemsSource = LSClass;
            cmbSec.ItemsSource = LSSection;
            //cmbTMonth.ItemsSource = LTMonth;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            reportViewer.Reset();
            DataTable dt = GetData();
            ReportDataSource reportDataSource = new ReportDataSource("DataSet1", dt);
            reportViewer.LocalReport.DataSources.Add(reportDataSource);
            reportViewer.LocalReport.ReportPath = "FeeBackUp.rdlc";
            //reportViewer.LocalReport.ReportEmbeddedResource = "ViewSwitchingNavigation.Reports.Reports.FeeBackUp.rdlc";
            //reportViewer.LocalReport.ReportEmbeddedResource = "ViewSwitchingNavigation.Reports.Reports.FeeBalance.rdlc";
            reportViewer.RefreshReport();
        }
        private DataTable GetData()
        {
            DataTable dt = new DataTable();
            string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
            object SClass = cmbClass.SelectedValue;
            object SSec = cmbSec.SelectedValue;
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("Usp_FeeBalanceCSM", cn);
                cmd.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@Class", (String.IsNullOrWhiteSpace(SClass.ToString()) ? DBNull.Value : SClass));
                cmd.Parameters.AddWithValue("@Section", (String.IsNullOrWhiteSpace(SSec.ToString()) ? DBNull.Value : SSec));
                cmd.Parameters.AddWithValue("@TillMonth", DBNull.Value);
                cmd.Parameters.AddWithValue("@DuesAmount", DBNull.Value);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            return dt;
        }
    }
}
