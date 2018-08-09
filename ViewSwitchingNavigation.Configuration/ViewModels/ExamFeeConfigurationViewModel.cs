using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using ViewSwitchingNavigation.Configuration.Model;
using ViewSwitchingNavigation.Infrastructure;

namespace ViewSwitchingNavigation.Configuration.ViewModels
{
    public class ExamFeeConfigurationViewModel:BindableBase
    {
        public IEnumerable<string> Classes { get; set; }
        public ObservableCollection<ExamFeeConfigurationModel> ExamFeeConfiguration { get; set; }
        public ExamFeeConfigurationModel ExamFeeConfigurationObj { get; set; }
        public ExamFeeConfigurationTopModel ExamFeeConfigurationTopObj { get; set; }
        Dictionary<string, string> Months { get; set; }
        public DelegateCommand<object> EditCommand { get; private set; }
        public DelegateCommand<object> SaveCommand { get; private set; }
        public DelegateCommand<object> PrintCommand { get; private set; }

        public bool IsDatafromDB = false;
        public static string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
        SqlConnection con = new SqlConnection(connectionString);
        DataSet ds = new DataSet();
        public ExamFeeConfigurationViewModel()
        {
            this.Classes = new[] { "000-Nursery", "001-LKG", "002-UKG", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
            this.Months = new Dictionary<string, string>() { { "Apr", "04-Apr" }, { "May", "05-May" }, {"Jun","06-Jun" }, {"Jul","07-Jul" }, {"Aug","08-Aug" }, {"Sep","09-Sep" }, { "Oct","10-Oct"}, {"Nov","11-Nov" }, {"Dec","12-Dec" }, {"Jan","01-Jan" }, {"Feb","02-Feb" }, {"Mar","03-Mar"} };
            this.ExamFeeConfiguration = new ObservableCollection<ExamFeeConfigurationModel>();
            this.ExamFeeConfigurationTopObj = new ExamFeeConfigurationTopModel();
            this.EditCommand = new DelegateCommand<object>(OnEditExecuted);
            this.SaveCommand = new DelegateCommand<object>(OnSaveExecuted);
            this.PrintCommand = new DelegateCommand<object>(OnPrintExecuted);
            SqlCommand command = new SqlCommand("Usp_ExamFeeMonthRelation", con);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
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
            if (ds.Tables[0].Rows.Count == 0)
            {
                IsDatafromDB = false;
                ExamFeeConfigurationTopObj.IsReadOnly = false;
                ExamFeeConfigurationTopObj.IsEnableSaveButton = true;
                ExamFeeConfigurationTopObj.IsEnableEditButton = false;
                foreach (string c in Classes)
                {
                    var obj = new ExamFeeConfigurationModel();
                    obj.StudentClass = c;
                    ExamFeeConfiguration.Add(obj);
                }
            }
            else
            {
                IsDatafromDB = true;
                ExamFeeConfigurationTopObj.IsReadOnly = true;
                ExamFeeConfigurationTopObj.IsEnableSaveButton = false;
                ExamFeeConfigurationTopObj.IsEnableEditButton = true;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    var obj = new ExamFeeConfigurationModel();
                    obj.StudentClass = dr["Class"].ToString();
                    if (dr["04-Apr"]!=System.DBNull.Value)
                        obj.Apr = (int)dr["04-Apr"];
                    if (dr["05-May"] != System.DBNull.Value)
                        obj.May= (int)dr["05-May"];
                    if (dr["06-Jun"] != System.DBNull.Value)
                        obj.Jun= (int)dr["06-Jun"];
                    if (dr["07-Jul"] != System.DBNull.Value)
                        obj.Jul= (int)dr["07-Jul"];
                    if (dr["08-Aug"] != System.DBNull.Value)
                        obj.Aug= (int)dr["08-Aug"];
                    if (dr["09-Sep"] != System.DBNull.Value)
                        obj.Sep= (int)dr["09-Sep"];
                    if (dr["10-Oct"] != System.DBNull.Value)
                        obj.Oct= (int)dr["10-Oct"];
                    if (dr["11-Nov"] != System.DBNull.Value)
                        obj.Nov= (int)dr["11-Nov"];
                    if (dr["12-Dec"] != System.DBNull.Value)
                        obj.Dec = (int)dr["12-Dec"];
                    if (dr["01-Jan"] != System.DBNull.Value)
                        obj.Jan = (int)dr["01-Jan"];
                    if (dr["02-Feb"] != System.DBNull.Value)
                        obj.Feb = (int)dr["02-Feb"];
                    if (dr["03-Mar"] != System.DBNull.Value)
                        obj.Mar = (int)dr["03-Mar"];
                    ExamFeeConfiguration.Add(obj);
                }
            }

        }
        public void OnEditExecuted(object parameter)
        {
            ExamFeeConfigurationTopObj.IsEnableEditButton = false;
            ExamFeeConfigurationTopObj.IsEnableSaveButton = true;
            ExamFeeConfigurationTopObj.IsReadOnly = false;
        }
        public void OnSaveExecuted(object parameter)
        {
            int ClassOrderNo = 0;
            foreach (ExamFeeConfigurationModel CFCM in ExamFeeConfiguration)
            {
                string MonthName;
                object ExamFee;
                ClassOrderNo = ClassOrderNo + 1;
                foreach (PropertyInfo propertyInfo in CFCM.GetType().GetProperties())
                {
                    
                    if (new string[] { "Apr", "May", "Jun","Jul","Aug","Sep","Oct","Nov","Dec","Jan","Feb","Mar" }.Contains(propertyInfo.Name))
                    {
                        MonthName = Months[propertyInfo.Name];
                        ExamFee=propertyInfo.GetValue(CFCM);
                        if(ExamFee!=null)
                        {
                            SqlCommand cmd = new SqlCommand("Usp_SaveUpdateExamFeeConfig", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            //cmd.Parameters.AddWithValue("@IsDatafromDB", ((object)IsDatafromDB ?? DBNull.Value));
                            cmd.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                            cmd.Parameters.AddWithValue("@Class", ((object)CFCM.StudentClass ?? DBNull.Value));
                            cmd.Parameters.AddWithValue("@MonthName", ((object)MonthName ?? DBNull.Value));
                            cmd.Parameters.AddWithValue("@ExamFee", ((object)ExamFee ?? DBNull.Value));
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
                }
            }
            MessageBox.Show("Exam Fee Configuration Saved.");
            ExamFeeConfigurationTopObj.IsEnableEditButton = true;
            ExamFeeConfigurationTopObj.IsEnableSaveButton = false;
            ExamFeeConfigurationTopObj.IsReadOnly = true;
        }
        public void OnPrintExecuted(object parameter)
        {
            //PrintDialog dialog = new PrintDialog();
            //dialog.PrintVisual(ConfigurationList, "My Canvas");
        }

    }
}
