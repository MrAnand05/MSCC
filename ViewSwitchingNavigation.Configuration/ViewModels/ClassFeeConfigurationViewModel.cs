using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using ViewSwitchingNavigation.Configuration.Model;
using ViewSwitchingNavigation.Infrastructure;


namespace ViewSwitchingNavigation.Configuration.ViewModels
{
    public class ClassFeeConfigurationViewModel:BindableBase
    {
        public IEnumerable<string> Classes { get; private set; }
        public ObservableCollection<ClassFeeConfigurationModel> ClassFeeConfiguration { get; set; }
        public ClassFeeConfigurationModel ClassFeeConfigurationObj { get; set; }
        public ClassFeeConfigurationTopModel ClassFeeConfigurationTopObj { get; set; }
        public DelegateCommand<object> EditCommand { get; private set; }
        public DelegateCommand<object> SaveCommand { get; private set; }
        public DelegateCommand<object> PrintCommand { get; private set; }
        public bool IsDatafromDB = false;
        public static string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
        SqlConnection con = new SqlConnection(connectionString);
        DataSet ds = new DataSet();
        public ClassFeeConfigurationViewModel()
        {
            int FinancialYear = AuthenticationContext.GlobalFinancialYear;
            this.Classes = new[] { "000-Nursery", "001-LKG", "002-UKG", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
            this.ClassFeeConfiguration = new ObservableCollection<ClassFeeConfigurationModel>();
            this.ClassFeeConfigurationTopObj = new ClassFeeConfigurationTopModel();
            this.EditCommand= new DelegateCommand<object>(OnEditExecuted);
            this.SaveCommand = new DelegateCommand<object>(OnSaveExecuted);
            this.PrintCommand= new DelegateCommand<object>(OnPrintExecuted);
            SqlCommand command = new SqlCommand("Usp_ClassFeeRelation", con);
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
            if(ds.Tables[0].Rows.Count==0)
            {
                IsDatafromDB = false;
                ClassFeeConfigurationTopObj.IsReadOnly = false;
                ClassFeeConfigurationTopObj.IsEnableSaveButton = true;
                ClassFeeConfigurationTopObj.IsEnableEditButton = false;
                foreach(string c in Classes)
                {
                    var obj = new ClassFeeConfigurationModel();
                    obj.StudentClass = c;
                    ClassFeeConfiguration.Add(obj);
                }
            }
            else
            {
                IsDatafromDB = true;
                ClassFeeConfigurationTopObj.IsReadOnly =true;
                ClassFeeConfigurationTopObj.IsEnableSaveButton = false;
                ClassFeeConfigurationTopObj.IsEnableEditButton = true;
                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    var obj = new ClassFeeConfigurationModel();
                    obj.StudentClass = dr["Class"].ToString();
                    obj.AdmissionFee = (int)dr["AdmissionFee"];
                    obj.AnnualFee=(int)dr["AnnualFee"];
                    obj.FormFee=(int)dr["FormFee"];
                    obj.IDCardFee = (int)dr["IDCardFee"];
                    obj.BoardRegistrationFee = (int)dr["BoardRegistrationFee"];
                    obj.MonthlyFee = (int)dr["MonthlyFee"];
                    obj.ComputerFee = (int)dr["ComputerFee"];
                    obj.TieBeltCharges = (int)dr["TieBeltCharge"];
                    obj.OtherCharges = (int)dr["OtherCharges"];
                    ClassFeeConfiguration.Add(obj);
                }
            }
        }
        public void OnEditExecuted(object parameter)
        {
            ClassFeeConfigurationTopObj.IsEnableEditButton = false;
            ClassFeeConfigurationTopObj.IsEnableSaveButton = true;
            ClassFeeConfigurationTopObj.IsReadOnly = false;
        }
        public void OnSaveExecuted(object parameter)
        {
            int ClassOrderNo = 0;
            foreach(ClassFeeConfigurationModel CFCM in ClassFeeConfiguration)
            {
                ClassOrderNo = ClassOrderNo + 1;
                SqlCommand cmd = new SqlCommand("Usp_SaveUpdateClassFeeConfig", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IsDatafromDB", ((object)IsDatafromDB ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@Class", ((object)CFCM.StudentClass ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@ClassOrderNo", ((object)ClassOrderNo ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@AdmissionFee", ((object)CFCM.AdmissionFee ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@AnnualFee", ((object)CFCM.AnnualFee?? DBNull.Value));
                cmd.Parameters.AddWithValue("@FormFee", ((object)CFCM.FormFee ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@IDCardFee", ((object)CFCM.IDCardFee ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@MonthlyFee", ((object)CFCM.MonthlyFee ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@ComputerFee", ((object)CFCM.ComputerFee ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@BoardRegistrationFee", ((object)CFCM.BoardRegistrationFee ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@TieBeltCharge", ((object)CFCM .TieBeltCharges?? DBNull.Value));
                cmd.Parameters.AddWithValue("@OtherCharges", ((object)CFCM.OtherCharges ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@EnteredBy", ((object)"" ?? DBNull.Value));
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
            MessageBox.Show("Class Fee Configuration Saved.");
            ClassFeeConfigurationTopObj.IsEnableEditButton = true;
            ClassFeeConfigurationTopObj.IsEnableSaveButton = false;
            ClassFeeConfigurationTopObj.IsReadOnly = true;

        }
        public void OnPrintExecuted(object parameter)
        {
            //PrintDialog dialog = new PrintDialog();
            //dialog.PrintVisual(ConfigurationList, "My Canvas");
        }
    }
}
