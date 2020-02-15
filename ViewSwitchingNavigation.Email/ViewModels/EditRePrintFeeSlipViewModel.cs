using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ViewSwitchingNavigation.Email.Model;
using ViewSwitchingNavigation.Email.Views;
using ViewSwitchingNavigation.Infrastructure;

namespace ViewSwitchingNavigation.Email.ViewModels
{
    [Export]
    public class EditRePrintFeeSlipViewModel:BindableBase
    {
        public IEnumerable<string> SectionList { get; private set; }
        public List<string> Classes1 { get; private set; }
        public EditRePrintFeeSlipTopModel EditRePrintFeeSlipTop { get; set; }
        public ObservableCollection<EditRePrintFeeSlipModel> EditRePrintFeeSlip { get; set; }
        public ObservableCollection<EditRePrintFeeSlipDetailModel> EditRePrintFeeSlipDetail { get; set; }
        public ObservableCollection<EditRePrintFeeSlipDetailModel> EditRePrintFeeSlipDetailOrg { get; set; }
        public DelegateCommand<object> SearchFeeSlipCommand { get; private set; }
        public DelegateCommand<object> RebateLostFocusCommand { get; set; }
        public DelegateCommand<object> RePrintFeeSlipCommand { get; set; }
        public DelegateCommand<object> UpdateFeeSlipCommand { get; set; }
        public DelegateCommand<object> CheckBoxChkUnChkCommand { get; set; }
        public DelegateCommand<object> ToPayLostFocusCommand { get; set; }
        public ICommand ShowDetailCommand { get; private set; }
        //public static string connectionString = @"Server= C1; Database= SM; User Id=as; Password=12345;";
        public static string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
        SqlConnection con = new SqlConnection(connectionString);
        DataSet ds = new DataSet();
        public EditRePrintFeeSlipViewModel()
        {
            this.SectionList = new[] { DBNull.Value.ToString(), "A", "B", "C", "D" };
            this.Classes1 = new List<string>();
            this.EditRePrintFeeSlipTop = new EditRePrintFeeSlipTopModel();
            this.EditRePrintFeeSlip = new ObservableCollection<EditRePrintFeeSlipModel>();
            this.EditRePrintFeeSlipDetail = new ObservableCollection<EditRePrintFeeSlipDetailModel>();
            this.EditRePrintFeeSlipDetailOrg = new ObservableCollection<EditRePrintFeeSlipDetailModel>();
            this.SearchFeeSlipCommand = new DelegateCommand<object>(this.Search);
            this.ShowDetailCommand = new DelegateCommand<object>(this.GetFeeSlipDetail);
            this.RebateLostFocusCommand = new DelegateCommand<object>(OnRebateLostFocusExecuted);
            this.ToPayLostFocusCommand = new DelegateCommand<object>(OnToPayLostFocusExecuted);
            this.RePrintFeeSlipCommand = new DelegateCommand<object>(RePrintFeeSlip);
            this.UpdateFeeSlipCommand = new DelegateCommand<object>(UpdateFeeSlip);
            this.CheckBoxChkUnChkCommand = new DelegateCommand<object>(UpdateEnableTotal);
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
            List<string> list = new List<string>();
            Classes1.Add("");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Classes1.Add(dr["Class"].ToString());
            }
        }
        public void Search(object parameters)
        {
            EditRePrintFeeSlip.Clear();
            DataSet ds = new DataSet();
            SqlCommand command = new SqlCommand("Usp_FindFeeSlip", con);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                command.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                command.Parameters.AddWithValue("@PaySlipNo", (String.IsNullOrWhiteSpace(((object[])parameters)[0].ToString()) ? DBNull.Value : ((object[])parameters)[0]));
                command.Parameters.AddWithValue("@AdminNo", (String.IsNullOrWhiteSpace(((object[])parameters)[1].ToString()) ? DBNull.Value : ((object[])parameters)[1]));
                command.Parameters.AddWithValue("@PaymentDate", (String.IsNullOrWhiteSpace(((object[])parameters)[2].ToString()) ? DBNull.Value : ((object[])parameters)[2]));
                command.Parameters.AddWithValue("@FName", (String.IsNullOrWhiteSpace(((object[])parameters)[3].ToString()) ? DBNull.Value : ((object[])parameters)[3]));
                command.Parameters.AddWithValue("@FirstName", (String.IsNullOrWhiteSpace(((object[])parameters)[4].ToString()) ? DBNull.Value : ((object[])parameters)[4]));
                command.Parameters.AddWithValue("@LastName", (String.IsNullOrWhiteSpace(((object[])parameters)[5].ToString()) ? DBNull.Value : ((object[])parameters)[5]));
                command.Parameters.AddWithValue("@CurrentClass", (String.IsNullOrWhiteSpace(((object[])parameters)[6].ToString()) ? DBNull.Value : ((object[])parameters)[6]));
                command.Parameters.AddWithValue("@Section", (String.IsNullOrWhiteSpace(((object[])parameters)[7].ToString()) ? DBNull.Value : ((object[])parameters)[7]));
                da.SelectCommand = command;
                da.Fill(ds);
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    var obj = new EditRePrintFeeSlipModel();
                    //{
                    //    SNo = (int)row["S.No"],
                    //    AdminNo =(int)row["AdminNo"],
                    //    StudentName= row["Name"].ToString(),
                    //    FName = row["FName"].ToString(),
                    //    Address = row["Address"].ToString(),
                    //    CurrentClass = row["CurrentClass"].ToString(),
                    //    Section = row["Section"].ToString()
                    //};
                    obj.PaySlipNo = Convert.ToInt32(row["PaySlipNo"].ToString());
                    obj.PaymentDate = ((DateTime)row["PaymentDate"]);
                    obj.TotalAmount = (int)row["TotalAmount"];
                    obj.TotalRebate = (int)row["TotalRebate"];
                    obj.AdminNo = (int)row["AdminNo"];
                    obj.Name = row["Name"].ToString();
                    obj.ClassSection = row["Class"].ToString();
                    obj.FName = row["FName"].ToString();
                    obj.Address = row["Address"].ToString();
                    EditRePrintFeeSlip.Add(obj);
                }
            }
            catch
            {

            }
            finally
            {
                command.Dispose();
            }
        }
        public void GetFeeSlipDetail(object parameters)
        {
            EditRePrintFeeSlipTop.TotalBalance=0; 
            EditRePrintFeeSlipTop.TotalRequired=0;
            EditRePrintFeeSlipTop.TotalRebate =0;
            EditRePrintFeeSlipTop.TotalToPay = 0;
            EditRePrintFeeSlipDetail.Clear();
            EditRePrintFeeSlipDetailOrg.Clear();
            EditRePrintFeeSlipTop.IsEnablePrintFeeSlip = true;
            DataSet ds = new DataSet();
            SqlCommand command = new SqlCommand("Usp_Get_PreviousPaySlip", con);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                command.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                command.Parameters.AddWithValue("@PaySlipNo", (object)parameters ?? DBNull.Value);
                da.SelectCommand = command;
                da.Fill(ds);
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    var obj = new EditRePrintFeeSlipDetailModel();
                    var objOrg = new EditRePrintFeeSlipDetailModel();
                    //var objOrg = new EditRePrintFeeSlipDetailModel();
                    if((int)row["IsVisible"]==1)
                    {
                        obj.IsCheckBoxVisible = "Visible";
                    }
                    else
                    {
                        obj.IsCheckBoxVisible="Hidden";
                    }
                    obj.Name = row["Name"].ToString();
                    obj.CurrentClass = row["Class"].ToString();
                    obj.FeeSlipNo = (int)row["FeeSlipNo"];
                    obj.Address = row["Address"].ToString();
                    obj.FatherName = row["FatherName"].ToString();
                    obj.DOP = ((DateTime)row["PaymentDate"]);
                    obj.AdminNo= (int)row["AdminNo"];
                    obj.Description=row["Description"].ToString();
                    obj.RequiredFee=(int)row["RequiredAmount"];
                    obj.Paid = (int)row["PAmountPaid"];
                    obj.RebateGiven = (int)row["PRebate"];
                    obj.Balance = (int)row["Balance"];
                    obj.Rebate= (int)row["CRebate"];
                    obj.ToPay = (int)row["CAmountPaid"];
                    obj.FeeSlipNo = (int)row["FeeSlipNo"];
                    //
                    objOrg.Name = row["Name"].ToString();
                    objOrg.CurrentClass = row["Class"].ToString();
                    objOrg.FeeSlipNo = (int)row["FeeSlipNo"];
                    objOrg.Address = row["Address"].ToString();
                    objOrg.FatherName = row["FatherName"].ToString();
                    objOrg.DOP = ((DateTime)row["PaymentDate"]);
                    objOrg.AdminNo = (int)row["AdminNo"];
                    objOrg.Description = row["Description"].ToString();
                    objOrg.RequiredFee = (int)row["RequiredAmount"];
                    objOrg.Paid = (int)row["PAmountPaid"];
                    objOrg.RebateGiven = (int)row["PRebate"];
                    objOrg.Balance = (int)row["Balance"];
                    objOrg.Rebate = (int)row["CRebate"];
                    objOrg.ToPay = (int)row["CAmountPaid"];
                    objOrg.FeeSlipNo = (int)row["FeeSlipNo"];
                    //
                    EditRePrintFeeSlipDetail.Add(obj);
                    EditRePrintFeeSlipDetailOrg.Add(objOrg);
                    EditRePrintFeeSlipTop.TotalRequired = EditRePrintFeeSlipTop.TotalRequired + (int)row["Balance"];
                    EditRePrintFeeSlipTop.TotalRebate = EditRePrintFeeSlipTop.TotalRebate + (int)row["CRebate"];
                    EditRePrintFeeSlipTop.TotalToPay = EditRePrintFeeSlipTop.TotalToPay + (int)row["CAmountPaid"];
                    EditRePrintFeeSlipTop.PaySlipNo = (int)row["FeeSlipNo"];
                }
                EditRePrintFeeSlipTop.TotalBalance =EditRePrintFeeSlipTop.TotalRequired-EditRePrintFeeSlipTop.TotalRebate-EditRePrintFeeSlipTop.TotalToPay;
            }
            catch
            {

            }
            finally
            {
                command.Dispose();
            }
        }
        public void RePrintFeeSlip(object parameters)
        {
            DublicateFeeSlip vm = new DublicateFeeSlip(parameters);
            vm.ShowDialog();
        }
        public void UpdateFeeSlip(object parameters)
        {
            object PaySlipNo=null;
            StringBuilder msg=new StringBuilder();
            msg.Append("");
            int counter = 0;
            try
            {
                con.Open();
                foreach (EditRePrintFeeSlipDetailModel EPFSD in EditRePrintFeeSlipDetail)
                {
                    PaySlipNo = (object)EPFSD.FeeSlipNo;
                    var foundOrg = EditRePrintFeeSlipDetailOrg.FirstOrDefault(x => x.Description == EPFSD.Description);
                    if (EPFSD.Checkboxtesting == true)
                    {
                        counter++;
                        if (EPFSD.Rebate != foundOrg.Rebate || EPFSD.ToPay != foundOrg.ToPay)
                        {
                            SqlCommand cmd2 = new SqlCommand("USP_UpdateFeeSlip", con);
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.AddWithValue("@PSD_PaySlipNo", ((object)EPFSD.FeeSlipNo ?? DBNull.Value));
                            cmd2.Parameters.AddWithValue("@PSD_TotalAmount", ((object)EditRePrintFeeSlipTop.TotalToPay ?? DBNull.Value));
                            cmd2.Parameters.AddWithValue("@PSD_TotalRebate", ((object)EditRePrintFeeSlipTop.TotalRebate ?? DBNull.Value));
                            cmd2.Parameters.AddWithValue("@PaymentFor", ((object)EPFSD.Description ?? DBNull.Value));
                            cmd2.Parameters.AddWithValue("@AmountPaid", ((object)EPFSD.ToPay ?? DBNull.Value));
                            cmd2.Parameters.AddWithValue("@RebateIfAny", ((object)EPFSD.Rebate ?? DBNull.Value));
                            cmd2.Parameters.AddWithValue("@Counter", ((object)counter ?? DBNull.Value));
                            cmd2.ExecuteNonQuery();
                        }
                        else
                        {
                            msg.Append(EPFSD.Description + ",");
                        }

                    }
                }
                if (msg.Length == 0)
                {
                    msg.Append("All selected rows updated");
                }
                else
                {
                    msg.Append(" is/are not updated, because there values are same as previous.");
                }
                MessageBox.Show(msg.ToString());
                GetFeeSlipDetail(PaySlipNo);
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
        public void UpdateEnableTotal(object parameter)
        {
            string Description= ((object[])(parameter))[0].ToString();
            var found = EditRePrintFeeSlipDetail.FirstOrDefault(x => x.Description == ((object[])(parameter))[0]);
            if ((bool)((object[])(parameter))[1])
            {
                //string s = "Description like '" + Description + "'";
                //DataRow[] foundRow = ds.Tables[1].Select(s);
                //found.ToPay = (int)foundRow[0]["Balance"];
            }
            else
            {
                var foundOrg= EditRePrintFeeSlipDetailOrg.FirstOrDefault(x => x.Description == found.Description);
                found.Rebate=foundOrg.Rebate;
                found.ToPay=foundOrg.ToPay;
            }
            UpdateTotalRebateAndToPay();
        }
        void UpdateTotalRebateAndToPay()
        {
            EditRePrintFeeSlipTop.TotalBalance = 0;
            //EditRePrintFeeSlipTop.TotalRequired = 0;
            EditRePrintFeeSlipTop.TotalRebate = 0;
            EditRePrintFeeSlipTop.TotalToPay = 0;
            foreach (EditRePrintFeeSlipDetailModel EPFSD in EditRePrintFeeSlipDetail)
            {
                EditRePrintFeeSlipTop.TotalRebate = EditRePrintFeeSlipTop.TotalRebate + EPFSD.Rebate;
                EditRePrintFeeSlipTop.TotalToPay = EditRePrintFeeSlipTop.TotalToPay + EPFSD.ToPay;
            }
            EditRePrintFeeSlipTop.TotalBalance = EditRePrintFeeSlipTop.TotalRequired - EditRePrintFeeSlipTop.TotalRebate - EditRePrintFeeSlipTop.TotalToPay;
            
            int CheckboxTrueCounter = 0;
            foreach (var founds in EditRePrintFeeSlipDetail)
            {
                if (founds.Checkboxtesting)
                {
                    CheckboxTrueCounter++;
                }

            }

            if (CheckboxTrueCounter != 0)
            {
                EditRePrintFeeSlipTop.IsCheckBoxSelected = true;
                EditRePrintFeeSlipTop.IsEnableUpdateFeeSlip = true;
            }
            else
            {
                EditRePrintFeeSlipTop.IsCheckBoxSelected = false;
                EditRePrintFeeSlipTop.IsEnableUpdateFeeSlip = false;
            }
        }
        void OnRebateLostFocusExecuted(object parameter)
        {
            UpdateTotalRebateAndToPay();
            var found = EditRePrintFeeSlipDetail.FirstOrDefault(x => x.Description == ((object[])(parameter))[0]);
            if (found.ToPay > (found.Balance - Convert.ToInt32(((object[])(parameter))[1])))
            {
                found.ToPay = found.Balance - Convert.ToInt32(((object[])(parameter))[1]);
            }
            
        }
        void OnToPayLostFocusExecuted(object parameter)
        {
            UpdateTotalRebateAndToPay();
        }
    }
}
