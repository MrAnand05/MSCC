using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ViewSwitchingNavigation.Configuration.Model;

namespace ViewSwitchingNavigation.Configuration.ViewModels
{
    [Export]
    public class InExpConfigurationViewModel:BindableBase
    {
        public ObservableCollection<InExpConfigurationModel> InExpConfiguration { get; set; }
        public InExpConfigurationModel InExpConfigurationObj { get; set; }
        public InExpConfigurationTopModel InExpConfigurationTopObj { get; set; }
        public DelegateCommand<object> SaveCommand { get; private set; }
        public DelegateCommand<object> ckhCheckedCommand { get; private set; }
        public DelegateCommand<object> UpdateCommand { get; private set; }
        public DelegateCommand<object> PrintCommand { get; private set; }

        public static string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
        SqlConnection con = new SqlConnection(connectionString);
        DataSet ds = new DataSet();
        public InExpConfigurationViewModel()
        {
            this.ckhCheckedCommand = new DelegateCommand<object>(OnChecked);
            this.UpdateCommand = new DelegateCommand<object>(OnUpdateExecuted);
            this.InExpConfiguration = new ObservableCollection<InExpConfigurationModel>();
            this.InExpConfigurationTopObj = new InExpConfigurationTopModel();
            this.SaveCommand = new DelegateCommand<object>(OnSaveExecuted);
            this.InExpConfigurationTopObj.Reset();
            //this.PrintCommand = new DelegateCommand<object>(OnPrintExecuted);
            SqlCommand command = new SqlCommand("Usp_InExpSourceList", con);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                da.SelectCommand = command;
                da.Fill(ds);
            }
            catch
            {

            }
            finally
            {
                command.Dispose();
            }
            //TransFeeConfigurationTopObj.IsReadOnly = true;
            InExpConfigurationTopObj.IsEnableSaveButton = false;
            InExpConfigurationTopObj.IsEnableEditButton = false;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                var obj = new InExpConfigurationModel();
                obj.SNo= (int)dr["SNo"];
                obj.IncomeExpenseSource = (string)dr["IncomeExpenseSource"];
                InExpConfiguration.Add(obj);
            }
        }
        public void OnChecked(object parameter)
        {
            int CheckboxTrueCounter = 0;
            foreach(var found in InExpConfiguration)
            {
                if (found.IsChecked)
                {
                    CheckboxTrueCounter++;
                }
            }
            if (CheckboxTrueCounter != 0)
            {
                InExpConfigurationTopObj.IsEnableEditButton = true;
            }
            else
            {
                InExpConfigurationTopObj.IsEnableEditButton = false;
            }

        }
        public void OnUpdateExecuted(object parameter)
        {

            foreach (var found in InExpConfiguration)
            {
                if (found.IsChecked)
                {
                    SqlCommand cmd = new SqlCommand("Usp_UpdateInExpSource", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SNo", ((object)found.SNo?? DBNull.Value));
                    cmd.Parameters.AddWithValue("@IncomeExpenseSource", ((object)found.IncomeExpenseSource ?? DBNull.Value));
                    try
                    {
                        con.Open();
                        cmd.ExecuteScalar();
                    }
                    catch (SqlException ex)
                    {
                        con.Close();
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                    }
                    
                }
            }
            System.Windows.Forms.MessageBox.Show("Source Details Updated");

            ds.Reset();
            SqlCommand command = new SqlCommand("Usp_InExpSourceList", con);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                da.SelectCommand = command;
                da.Fill(ds);
            }
            catch
            {

            }
            finally
            {
                command.Dispose();
            }
            InExpConfiguration.Clear();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                var obj = new InExpConfigurationModel();
                obj.SNo = (int)dr["SNo"];
                obj.IncomeExpenseSource = (string)dr["IncomeExpenseSource"];
                InExpConfiguration.Add(obj);
            }
            InExpConfigurationTopObj.Reset();

        }
        public void OnSaveExecuted(object parameter)
        {
            int SNo;
            if (InExpConfiguration.Any(p => p.SNo == InExpConfigurationTopObj.SNo && p.IncomeExpenseSource.ToUpper() == InExpConfigurationTopObj.IncomeExpenseSource.ToUpper()))
            {
                System.Windows.Forms.MessageBox.Show(string.Format("Data already Exist "));
            }
            else
            { 
                SqlCommand cmd = new SqlCommand("Usp_InsertInExpSource", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IncomeExpenseSource", ((object)InExpConfigurationTopObj.IncomeExpenseSource ?? DBNull.Value));
                //cmd.Parameters.AddWithValue("@SNo", ((object)InExpConfigurationTopObj.SNo ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@SNo", SqlDbType.Int));
                cmd.Parameters["@SNo"].Direction = ParameterDirection.Output;
                try
                {
                    con.Open();
                    cmd.ExecuteScalar();
                    SNo = (int)cmd.Parameters["@SNo"].Value;
                }
                catch (SqlException ex)
                {
                    con.Close();
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
                if (SNo==null)
                {
                    System.Windows.Forms.MessageBox.Show("Data not saved");
                }
                else
                {
                    ds.Reset();
                    System.Windows.Forms.MessageBox.Show(string.Format("SNo Generated: {0} ", SNo));
                    SqlCommand command = new SqlCommand("Usp_InExpSourceList", con);
                    command.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter();
                    try
                    {
                        da.SelectCommand = command;
                        da.Fill(ds);
                    }
                    catch
                    {

                    }
                    finally
                    {
                        command.Dispose();
                    }
                    InExpConfiguration.Clear();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        var obj = new InExpConfigurationModel();
                        obj.SNo = (int)dr["SNo"];
                        obj.IncomeExpenseSource = (string)dr["IncomeExpenseSource"];
                        InExpConfiguration.Add(obj);
                    }
                    InExpConfigurationTopObj.Reset();
            }
            }
        }

    }
}
