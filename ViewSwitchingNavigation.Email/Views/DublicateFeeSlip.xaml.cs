using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Windows;
using ViewSwitchingNavigation.Email.Model;
using ViewSwitchingNavigation.Infrastructure;

namespace ViewSwitchingNavigation.Email.Views
{
    /// <summary>
    /// Interaction logic for FeeBalance.xaml
    /// </summary>
    public partial class DublicateFeeSlip : Window
    {

        public DublicateFeeSlip(object parameters)
        {
            InitializeComponent();
            reportViewer.Reset();
            //DataTable dt = ConvertTo((ObservableCollection < EditRePrintFeeSlipDetailModel >) parameters);
            //ObservableCollection<EditRePrintFeeSlipDetailModel> abc = new ObservableCollection<EditRePrintFeeSlipDetailModel>();
            //abc = (ObservableCollection<EditRePrintFeeSlipDetailModel>)((object[])(parameters))[0];
            int p = (((ObservableCollection<EditRePrintFeeSlipDetailModel>)(parameters)))[0].FeeSlipNo;
            //int payslipNo = abc[0].FeeSlipNo;
            DataTable dt = GetData(p);
            ReportDataSource reportDataSource = new ReportDataSource("DataSet1", dt);
            reportViewer.LocalReport.DataSources.Add(reportDataSource);
            reportViewer.LocalReport.ReportEmbeddedResource = "ViewSwitchingNavigation.Email.FeeSlipPrint.rdlc";
            reportViewer.RefreshReport();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private DataTable GetData(int FeeSlipNo)
        {
            DataTable dt = new DataTable();
            string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("Usp_Get_PreviousPaySlip", cn);
                cmd.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@PaySlipNo", ((object)FeeSlipNo ?? DBNull.Value));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            return dt;
        }
public static DataTable ConvertTo<T>(IList<T> list)
{
    DataTable table = CreateTable<T>();
    Type entityType = typeof(T);
    PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

    foreach (T item in list)
    {
        DataRow row = table.NewRow();

        foreach (PropertyDescriptor prop in properties)
        {
            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
        }

        table.Rows.Add(row);
    }

    return table;
}    

public static DataTable CreateTable<T>()
{
    Type entityType = typeof(T);
    DataTable table = new DataTable(entityType.Name);
    PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

    foreach (PropertyDescriptor prop in properties)
    {
        table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(
            prop.PropertyType) ?? prop.PropertyType);
    }

    return table;
}


        public static DataTable GetDataTableFromObjects(object[] objects)
        {
            if (objects != null && objects.Length > 0)
            {
                Type t = objects[0].GetType();
                DataTable dt = new DataTable(t.Name);
                foreach (PropertyInfo pi in t.GetProperties())
                {
                    dt.Columns.Add(new DataColumn(pi.Name));
                }
                foreach (var o in objects)
                {
                    DataRow dr = dt.NewRow();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        dr[dc.ColumnName] = o.GetType().GetProperty(dc.ColumnName).GetValue(o, null);
                    }
                    dt.Rows.Add(dr);
                }
                return dt;
            }
            return null;
        }
    }
}
