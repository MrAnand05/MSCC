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
    public class TransFeeConfigurationViewModel:BindableBase
    {
        public ObservableCollection<TransFeeConfigurationModel> TransFeeConfiguration { get; set; }
        public TransFeeConfigurationModel TransFeeConfigurationObj { get; set; }
        public TransFeeConfigurationTopModel TransFeeConfigurationTopObj { get; set; }
        public DelegateCommand<object> SaveCommand { get; private set; }
        public DelegateCommand<object> ckhCheckedCommand { get; private set; }
        public DelegateCommand<object> UpdateCommand { get; private set; }
        public DelegateCommand<object> PrintCommand { get; private set; }

        public static string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
        SqlConnection con = new SqlConnection(connectionString);
        DataSet ds = new DataSet();
        public TransFeeConfigurationViewModel()
        {
            this.ckhCheckedCommand = new DelegateCommand<object>(OnChecked);
            this.UpdateCommand = new DelegateCommand<object>(OnUpdateExecuted);
            this.TransFeeConfiguration = new ObservableCollection<TransFeeConfigurationModel>();
            this.TransFeeConfigurationTopObj = new TransFeeConfigurationTopModel();
            this.SaveCommand = new DelegateCommand<object>(OnSaveExecuted);
            this.TransFeeConfigurationTopObj.Reset();
            //this.PrintCommand = new DelegateCommand<object>(OnPrintExecuted);
            SqlCommand command = new SqlCommand("Usp_TransportRouteFeeRelation", con);
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
            TransFeeConfigurationTopObj.IsEnableSaveButton = false;
            TransFeeConfigurationTopObj.IsEnableEditButton = false;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                var obj = new TransFeeConfigurationModel();
                obj.RouteId = (int)dr["RouteId"];
                obj.RouteNo= (int)dr["RouteNo"];
                obj.RouteDescription = (string)dr["RouteDescription"];
                obj.Stop= (string)dr["Stop"];
                obj.TransportFee= (int)dr["TransportFee"];
                TransFeeConfiguration.Add(obj);
            }
        }
        public void OnChecked(object parameter)
        {
            int CheckboxTrueCounter = 0;
            foreach(var found in TransFeeConfiguration)
            {
                if (found.IsChecked)
                {
                    CheckboxTrueCounter++;
                }
            }
            if (CheckboxTrueCounter != 0)
            {
                TransFeeConfigurationTopObj.IsEnableEditButton = true;
               
            }
            else
            {
                TransFeeConfigurationTopObj.IsEnableEditButton = false;
            }

        }
        public void OnUpdateExecuted(object parameter)
        {

            foreach (var found in TransFeeConfiguration)
            {
                if (found.IsChecked)
                {
                    SqlCommand cmd = new SqlCommand("Usp_UpdateTransRoute", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RouteId", ((object)found.RouteId ?? DBNull.Value));
                    cmd.Parameters.AddWithValue("@RouteNo", ((object)found.RouteNo ?? DBNull.Value));
                    cmd.Parameters.AddWithValue("@RouteDescription", ((object)found.RouteDescription ?? DBNull.Value));
                    cmd.Parameters.AddWithValue("@Stop", ((object)found.Stop ?? DBNull.Value));
                    cmd.Parameters.AddWithValue("@TransportFee", ((object)found.TransportFee ?? DBNull.Value));
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
            System.Windows.Forms.MessageBox.Show("Transport Details Updated");

            ds.Reset();
            SqlCommand command = new SqlCommand("Usp_TransportRouteFeeRelation", con);
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
            TransFeeConfiguration.Clear();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                var obj = new TransFeeConfigurationModel();
                obj.RouteId = (int)dr["RouteId"];
                obj.RouteNo = (int)dr["RouteNo"];
                obj.RouteDescription = (string)dr["RouteDescription"];
                obj.Stop = (string)dr["Stop"];
                obj.TransportFee = (int)dr["TransportFee"];
                TransFeeConfiguration.Add(obj);
            }
            TransFeeConfigurationTopObj.Reset();

        }
        public void OnSaveExecuted(object parameter)
        {
            int routeid;
            if (TransFeeConfiguration.Any(p => p.RouteNo == TransFeeConfigurationTopObj.RouteNo && p.Stop.ToUpper()==TransFeeConfigurationTopObj.Stop.ToUpper()))
            {
                System.Windows.Forms.MessageBox.Show(string.Format("Data already Exist "));
            }
            else
            { 
                SqlCommand cmd = new SqlCommand("Usp_InsertTransRoute", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RouteNo", ((object)TransFeeConfigurationTopObj.RouteNo?? DBNull.Value));
                cmd.Parameters.AddWithValue("@RouteDescription", ((object)TransFeeConfigurationTopObj.RouteDescription ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@Stop", ((object)TransFeeConfigurationTopObj.Stop ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@TransportFee", ((object)TransFeeConfigurationTopObj.TransportFee ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@RouteId", SqlDbType.Int));
                cmd.Parameters["@RouteId"].Direction = ParameterDirection.Output;
                try
                {
                    con.Open();
                    cmd.ExecuteScalar();
                    routeid = (int)cmd.Parameters["@RouteId"].Value;
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
                if (routeid==null)
                {
                    System.Windows.Forms.MessageBox.Show("Data not saved");
                }
                else
                {
                    ds.Reset();
                    System.Windows.Forms.MessageBox.Show(string.Format("RouteId Generated: {0} ", routeid));
                    SqlCommand command = new SqlCommand("Usp_TransportRouteFeeRelation", con);
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
                    TransFeeConfiguration.Clear();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        var obj = new TransFeeConfigurationModel();
                        obj.RouteId = (int)dr["RouteId"];
                        obj.RouteNo = (int)dr["RouteNo"];
                        obj.RouteDescription = (string)dr["RouteDescription"];
                        obj.Stop = (string)dr["Stop"];
                        obj.TransportFee = (int)dr["TransportFee"];
                        TransFeeConfiguration.Add(obj);
                    }
                    TransFeeConfigurationTopObj.Reset();
            }
            }
        }

    }
}
