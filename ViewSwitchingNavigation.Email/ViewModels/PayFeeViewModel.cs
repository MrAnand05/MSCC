using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ViewSwitchingNavigation.Email.Model;
using ViewSwitchingNavigation.Email.Views;
using ViewSwitchingNavigation.Infrastructure;

namespace ViewSwitchingNavigation.Email.ViewModels
{
    [Export]
    public class PayFeeViewModel : BindableBase
    {
        private readonly ImageCon _imgcon = new ImageCon();
        //ImageConverter imgcon = new ImageConverter();
        public PayFeeViewModel()
        {
            int NextYear = AuthenticationContext.GlobalFinancialYear + 1;
            EditedFinancialYear = "20" + AuthenticationContext.GlobalFinancialYear + "-" + "20" + NextYear.ToString();
            PayFeeModelObject = new PayFeeModel();
            PayMonthlyFeeModelObject = new PayMonthlyFeeModel();
            SavePaymentCommand = new DelegateCommand<object>(SavePayment);
            //SavePaymentCommand = new DelegateCommand<ObservableCollection<PayFeeModel>>(SavePayment, (PayFeeDetails) => CanSave);
            this.RaiseSearchStudentCommand = new DelegateCommand<object>(RaiseSearchStudent);
            this.BuyCommand = new DelegateCommand<object>(OnBuyExecuted);
            this.AddMonthlyPayFeeCommand = new DelegateCommand<object>(OnAddMonthlyPayFeeExecuted);
            this.RebateLostFocusCommand = new DelegateCommand<object>(OnRebateLostFocusExecuted);
            this.MonthlyRebateLostFocusCommand = new DelegateCommand<object>(OnMonthlyRebateLostFocusExecuted);
            this.ToPayLostFocusCommand = new DelegateCommand<object>(OnToPayLostFocusExecuted);
            this.MonthlyToPayLostFocusCommand = new DelegateCommand<object>(OnMonthlyToPayLostFocusExecuted);
            this.SearchStudentRequest = new InteractionRequest<SearchStudent>();
            this.PayFeeDetails = new ObservableCollection<PayFeeModel>();
            this.PayMonthlyFeeDetails = new ObservableCollection<PayMonthlyFeeModel>();
        }
        private string editedFinancialYear;
        private PayFeeModel payFeeModelObject;
        public string EditedFinancialYear
        {
            get { return editedFinancialYear; }
            set { SetProperty(ref this.editedFinancialYear, value); }
        }
        public PayFeeModel PayFeeModelObject
        {
            get { return payFeeModelObject; }
            set { payFeeModelObject = value; }
        }
        private PayMonthlyFeeModel payMonthlyFeeModelObject;
        public PayMonthlyFeeModel PayMonthlyFeeModelObject
        {
            get { return payMonthlyFeeModelObject; }
            set { payMonthlyFeeModelObject = value; }
        }
        public ICommand SavePaymentCommand { get; private set; }
        public InteractionRequest<SearchStudent> SearchStudentRequest { get; private set; }
        public ICommand RaiseSearchStudentCommand { get; private set; }
        public DelegateCommand<object> BuyCommand { get; private set; }
        public DelegateCommand<object> AddMonthlyPayFeeCommand { get; private set; }
        public DelegateCommand<object> RebateLostFocusCommand { get; set; }
        public DelegateCommand<object> MonthlyRebateLostFocusCommand { get; set; }
        public DelegateCommand<object> ToPayLostFocusCommand { get; set; }
        public DelegateCommand<object> MonthlyToPayLostFocusCommand { get; set; }
        public SearchStudentOutputModel returnedStudent { get; set; }

        public ObservableCollection<PayFeeModel> PayFeeDetails { get; set; }
        public ObservableCollection<PayMonthlyFeeModel> PayMonthlyFeeDetails { get; set; }
        public static string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
        //public int NextYear = CurrentYear + 1;

        SqlConnection con = new SqlConnection(connectionString);
        DataSet ds = new DataSet();
        PayFeeModel payFeeRows;
        PayMonthlyFeeModel MonthlyPayFeeRows;
        public int? PaySlipNo;
        public bool CanSave
        {
            get { return true; }
        }
        public void SavePayment(object obj)
        {

            ObservableCollection<PayFeeModel> SavePayFeeObject = new ObservableCollection<PayFeeModel>();
            ObservableCollection<PayMonthlyFeeModel> SavePayMonthlyFeeObject = new ObservableCollection<PayMonthlyFeeModel>();
            SavePayFeeObject = (ObservableCollection<PayFeeModel>)((object[])(obj))[0];
            SavePayMonthlyFeeObject = (ObservableCollection<PayMonthlyFeeModel>)((object[])(obj))[1];
            ObservableCollection<PayFeeModel> YearlyFeePaid = new ObservableCollection<PayFeeModel>(SavePayFeeObject.Where(x => x.Checkboxtesting == true));
            ObservableCollection<PayMonthlyFeeModel> MonthlyFeePaid = new ObservableCollection<PayMonthlyFeeModel>(SavePayMonthlyFeeObject.Where(x => x.MonthlyCheckBox == true));
            //object YearlyFeePaid = SavePayFeeObject.Where(x => x.Checkboxtesting == true);
            //object MonthlyFeePaid = SavePayMonthlyFeeObject.Where(x => x.MonthlyCheckBox== true);
            PrintFeeSlipView vm = new PrintFeeSlipView(PayFeeModelObject, YearlyFeePaid, MonthlyFeePaid);
            //TransactionsReportR tr = new TransactionsReportR();

            int SumTotalRowIndex = PayFeeDetails.IndexOf(PayFeeDetails.Where(x => x.Description == "SumTotal").FirstOrDefault());
            SqlCommand cmd = new SqlCommand("Usp_GeneratePaySlip", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
            cmd.Parameters.AddWithValue("@AdminNo", ((object)PayFeeDetails[0].AdminNo ?? DBNull.Value));
            cmd.Parameters.AddWithValue("@TotalAmount", ((object)PayMonthlyFeeModelObject.YearlyMonthlyToPayTotal ?? DBNull.Value));
            cmd.Parameters.AddWithValue("@TotalRebate", ((object)PayMonthlyFeeModelObject.YearlyMonthlyRebateTotal ?? DBNull.Value));
            cmd.Parameters.AddWithValue("@EnteredBy", ((object)"" ?? DBNull.Value));
            cmd.Parameters.AddWithValue("@UpdatedBy", ((object)"" ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@PaySlipNo", SqlDbType.Int));
            cmd.Parameters["@PaySlipNo"].Direction = ParameterDirection.Output;

            //cmd.Parameters.AddWithValue("@LastName", stud.LastName);
            try
            {
                con.Open();
                cmd.ExecuteScalar();
                PaySlipNo = (int)cmd.Parameters["@PaySlipNo"].Value;
                PayFeeModelObject.FeeSlipNo = PaySlipNo;
                PayFeeModelObject.DOP = DateTime.Now;
                payMonthlyFeeModelObject.FeeSlipNo = PaySlipNo;
                payMonthlyFeeModelObject.MonthlyDOP = DateTime.Now;
                int? AdminNo = 0, FinancialYear = 0;  //to be used in Monthly Payment
                foreach (PayFeeModel pe in SavePayFeeObject)
                {
                    AdminNo = pe.AdminNo;
                    FinancialYear = pe.FinancialYear;
                    if (pe.Checkboxtesting)
                    {
                        //AdminNo = pe.AdminNo;
                        //FinancialYear = pe.FinancialYear;
                        SqlCommand cmd1 = new SqlCommand("Usp_SavePaySlipItems", con);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@AdminNo", ((object)pe.AdminNo ?? DBNull.Value));
                        cmd1.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                        cmd1.Parameters.AddWithValue("@PaySlipNo", ((object)PaySlipNo ?? DBNull.Value));
                        cmd1.Parameters.AddWithValue("@PaymentFor", ((object)pe.Description ?? DBNull.Value));
                        cmd1.Parameters.AddWithValue("@AmountPaid", ((object)pe.ToPay ?? DBNull.Value));
                        cmd1.Parameters.AddWithValue("@RebateIfAny", ((object)pe.Rebate ?? DBNull.Value));
                        cmd1.Parameters.AddWithValue("@RebateRemark", ((object)pe.RebateRemark ?? DBNull.Value));
                        cmd1.Parameters.AddWithValue("@EnteredBy", ((object)"" ?? DBNull.Value));
                        cmd1.Parameters.AddWithValue("@UpdatedBy", ((object)"" ?? DBNull.Value));
                        cmd1.ExecuteNonQuery();
                    }
                }
                foreach (PayMonthlyFeeModel PayMonthly in SavePayMonthlyFeeObject)
                {
                    if (PayMonthly.MonthlyCheckBox)
                    {
                        SqlCommand cmd2 = new SqlCommand("Usp_SaveMonthlyPaySlipItems", con);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.AddWithValue("@AdminNo", ((object)AdminNo ?? DBNull.Value));
                        cmd2.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                        cmd2.Parameters.AddWithValue("@PaySlipNo", ((object)PaySlipNo ?? DBNull.Value));
                        cmd2.Parameters.AddWithValue("@PaymentFor", ((object)PayMonthly.MonthName ?? DBNull.Value));
                        cmd2.Parameters.AddWithValue("@RequiredMonthlyFee", ((object)PayMonthly.RequiredMonthlyFee ?? DBNull.Value));
                        cmd2.Parameters.AddWithValue("@AmountPaid", ((object)PayMonthly.MonthlyToPay ?? DBNull.Value));
                        cmd2.Parameters.AddWithValue("@RebateIfAny", ((object)PayMonthly.MonthlyRebate ?? DBNull.Value));
                        cmd2.Parameters.AddWithValue("@RebateRemark", ((object)PayMonthly.MonthlyRebateRemark ?? DBNull.Value));
                        cmd2.Parameters.AddWithValue("@EnteredBy", ((object)"" ?? DBNull.Value));
                        cmd2.Parameters.AddWithValue("@UpdatedBy", ((object)"" ?? DBNull.Value));
                        cmd2.ExecuteNonQuery();
                    }
                }
                GetRequiredAndPaidFee(PayFeeModelObject.AdminNo, returnedStudent.Class, PayFeeModelObject.FinancialYear, PayFeeModelObject.SiblingConcession);
                foreach (PayFeeModel pe in SavePayFeeObject)
                {
                    pe.Checkboxtesting = false;
                }
                payFeeModelObject.Checkboxtesting = false;
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
            MessageBox.Show("Fee Slip No Generated:" + PaySlipNo);
            vm.SetOtherField(PaySlipNo, payMonthlyFeeModelObject.MonthlyDOP, EditedFinancialYear);
            vm.ShowDialog();
            //tr.ShowDialog();

        }
        public void RaiseSearchStudent(object obj)
        {
            SearchStudentViewTest vm = new SearchStudentViewTest();
            vm.ShowDialog();
            returnedStudent = vm.SelectedItem;
            try
            {
                if (obj.ToString() == "SearchPayFee" && returnedStudent.Admino!=null )
                {
                    PaySlipNo = null;
                    this.PayFeeModelObject.AdminNo = returnedStudent.Admino;
                    this.PayFeeModelObject.Name = string.Format("{0} {1}", returnedStudent.FirstName, returnedStudent.LastName);
                    //this.PayFeeModelObject.CurrentClass = returnedStudent.Class;
                    //this.PayFeeModelObject.Section = returnedStudent.Section;
                    this.PayFeeModelObject.FinancialYear = AuthenticationContext.GlobalFinancialYear;
                    this.PayFeeModelObject.FatherName = returnedStudent.FName;
                    this.PayFeeModelObject.Address = string.Format("{0}-{1}", returnedStudent.PAddress, returnedStudent.PDistrict);
                    this.PayFeeModelObject.SiblingConcession = returnedStudent.SiblingConcession;
                    //if(returnedStudent.Section!="")
                    //this.PayFeeModelObject.CurrentClass = returnedStudent.Class + "/" + returnedStudent.Section;

                    //
                    string StudentClass;
                    string Section;
                    if (returnedStudent.Class.ToString().Length > 4)
                        StudentClass = returnedStudent.Class.ToString().Substring(4);
                    else
                        StudentClass = returnedStudent.Class;
                    if (returnedStudent.Section.ToString().Trim() != "")
                        Section = "/" + returnedStudent.Section;
                    else
                        Section = returnedStudent.Section;

                    this.PayFeeModelObject.CurrentClass = StudentClass;
                    this.PayFeeModelObject.Section = Section;
                    //
                    //Added By Anand Start

                    //Bitmap bitmap1;
                    //byte[] UpdatedPic = null;
                    //Stud.Image = loadBitmap(bitmap);
                    if (returnedStudent.StuImage != null)
                    {
                        this.PayFeeModelObject.Image = _imgcon.loadBitmap(returnedStudent.StuImage);
                    }
                    else
                    {
                        this.PayFeeModelObject.Image = null;
                    }
                    //End
                    PayMonthlyFeeModelObject.YearlyMonthlyRequiredTotal = 0;
                    PayMonthlyFeeModelObject.YearlyMonthlyRebateTotal = 0;
                    PayMonthlyFeeModelObject.YearlyMonthlyToPayTotal = 0;
                    PayMonthlyFeeModelObject.FeeSlipNo = PaySlipNo;
                    //this.PayFeeModelObject.Photo=returnedStudent.
                    if (PayFeeModelObject.AdminNo != null)
                        GetRequiredAndPaidFee(PayFeeModelObject.AdminNo, returnedStudent.Class, PayFeeModelObject.FinancialYear, PayFeeModelObject.SiblingConcession);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void GetRequiredAndPaidFee(int? AdminNo, string CurrentClass, int FinancialYear, bool SiblingConcession)
        {
            PayFeeDetails.Clear();
            PayMonthlyFeeDetails.Clear();
            ds.Reset();
            SqlCommand command = new SqlCommand("Usp_RequiredAndPaidFee", con);
            SqlCommand MonthlyFeecommand = new SqlCommand("Usp_RequiredAndPaidFeeMonthly", con);
            command.CommandType = CommandType.StoredProcedure;
            MonthlyFeecommand.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                command.Parameters.AddWithValue("@AdminNo", ((object)AdminNo ?? DBNull.Value));
                command.Parameters.AddWithValue("@Class", ((object)CurrentClass ?? DBNull.Value));
                command.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                da.SelectCommand = command;
                da.Fill(ds);
                DataTable MonthlyFeeDetail = new DataTable();
                ds.Tables.Add(MonthlyFeeDetail);
                MonthlyFeecommand.Parameters.AddWithValue("@AdminNo", ((object)AdminNo ?? DBNull.Value));
                MonthlyFeecommand.Parameters.AddWithValue("@Class", ((object)CurrentClass ?? DBNull.Value));
                MonthlyFeecommand.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                MonthlyFeecommand.Parameters.AddWithValue("@SiblingConcession", ((object)SiblingConcession ?? DBNull.Value));
                da.SelectCommand = MonthlyFeecommand;
                da.Fill(ds.Tables[1]);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    payFeeRows = new PayFeeModel();
                    payFeeRows.AdminNo = Convert.ToInt32((object)PayFeeModelObject.AdminNo);
                    payFeeRows.CurrentClass = Convert.ToString((object)PayFeeModelObject.CurrentClass);
                    payFeeRows.FinancialYear = Convert.ToInt32((object)PayFeeModelObject.FinancialYear);
                    payFeeRows.Description = row["Description"].ToString();
                    payFeeRows.RequiredFee = (int)row["RequiredFee"];
                    payFeeRows.Paid = (int)row["AmountPaid"];
                    payFeeRows.RebateGiven = (int)row["RebateIfAny"];
                    payFeeRows.Balance  = (int)row["Balance"];
                    payFeeRows.LastPaymentDate = DBNull.Value.Equals(row["DateOfPayment"]) ? "" : ((DateTime)row["DateOfPayment"]).ToShortDateString();
                    PayFeeDetails.Add(payFeeRows);
                }
                foreach (DataRow row in ds.Tables[1].Rows)
                {
                    //string MonthNamePostFix = "";
                    //if ((int)row["SiblingConcession"] != 0)
                    //{
                    //    MonthNamePostFix = MonthNamePostFix+"-" + (int)row["SiblingConcession"] + "(BS)";
                    //}
                    //if ((int)row["TransportFee"] != 0)
                    //{
                    //    MonthNamePostFix = MonthNamePostFix + "+" + (int)row["TransportFee"] + "(T)";
                    //}
                    //if ((int)row["ExamFee"] != 0)
                    //{
                    //    MonthNamePostFix = MonthNamePostFix + "+" + (int)row["ExamFee"] + "(E)";
                    //}
                    //if ((int)row["ComputerFee"] != 0)
                    //{
                    //    MonthNamePostFix = MonthNamePostFix + "+" + (int)row["ComputerFee"] + "(C)";
                    //}
                    MonthlyPayFeeRows = new PayMonthlyFeeModel();
                    MonthlyPayFeeRows.MonthName = row["Description"].ToString(); //+ MonthNamePostFix;
                    MonthlyPayFeeRows.RequiredMonthlyFee = (int)row["RequiredMonthlyFee"]; //(int)row["MonthlyFee"] + (int)row["TransportFee"] + (int)row["ComputerFee"] + (int)row["ExamFee"] - (int)row["SiblingConcession"];
                    MonthlyPayFeeRows.Monthlypaid = (int)row["AmountPaid"];
                    MonthlyPayFeeRows.MonthlyRebateGiven = (int)row["RebateIfAny"];
                    MonthlyPayFeeRows.MonthlyBalance = (int)row["Balance"];
                    MonthlyPayFeeRows.MontlyLastPaymentDate = DBNull.Value.Equals(row["DateOfPayment"]) ? "" : ((DateTime)row["DateOfPayment"]).ToShortDateString();
                    PayMonthlyFeeDetails.Add(MonthlyPayFeeRows);
                }
                UpdateTotalRebateAndToPay();

            }
            catch
            {

            }
            finally
            {
                command.Dispose();
            }
        }
        void OnBuyExecuted(object parameter)
        {
            var found = PayFeeDetails.FirstOrDefault(x => x.Description == ((object[])(parameter))[0]);
            if ((bool)((object[])(parameter))[1])
            {
                string s = "Description like '" + ((object[])(parameter))[0] + "'";
                DataRow[] foundRow = ds.Tables[0].Select(s);
                found.ToPay = (int)foundRow[0]["Balance"];
            }
            else
            {
                found.ToPay = 0;
                found.Rebate = 0;
            }
            //Edited By Anand Date 20Sep'15 Start : Sum total removed.
            //int sumtotalToPay = 0, sumtotalRebate = 0;
            //var fo = PayFeeDetails.Where(x => x.Description != "SumTotal");
            //foreach (var foun in fo)
            //{
            //    sumtotalToPay = sumtotalToPay + foun.ToPay;
            //    sumtotalRebate = sumtotalRebate + foun.Rebate;
            //}
            //var SumTotalRow = PayFeeDetails.FirstOrDefault(x => x.Description == "SumTotal");
            //SumTotalRow.ToPay = sumtotalToPay;
            //SumTotalRow.Rebate = sumtotalRebate;
            //End
            UpdateTotalRebateAndToPay();
        }
        void OnAddMonthlyPayFeeExecuted(object parameter)
        {
            string MonthName = ((object[])(parameter))[0].ToString().Substring(0, 6);
            var found = PayMonthlyFeeDetails.FirstOrDefault(x => x.MonthName == ((object[])(parameter))[0]);
            if ((bool)((object[])(parameter))[1])
            {
                string s = "PaymentForMonth like '" + MonthName + "'";
                DataRow[] foundRow = ds.Tables[1].Select(s);
                found.MonthlyToPay = (int)foundRow[0]["Balance"];
            }
            else
            {
                found.MonthlyToPay = 0;
                found.MonthlyRebate = 0;
            }
            UpdateTotalRebateAndToPay();
            //PayMonthlyFeeModelObject.YearlyMonthlyRebateTotal = TotalRebateSumtotal;
            //PayMonthlyFeeModelObject.YearlyMonthlyToPayTotal = TotalToPaysumtotal;
        }
        void OnToPayLostFocusExecuted(object parameter)
        {
            int ToPaysumtotal = 0;
            var fo = PayFeeDetails.Where(x => x.Description != "SumTotal");
            foreach (var found in fo)
            {
                ToPaysumtotal = ToPaysumtotal + found.ToPay;
            }
            //Edited By Anand Date 20Sep'15 Start : Sum total removed.
            //var SumTotalRow = PayFeeDetails.FirstOrDefault(x => x.Description == "SumTotal");
            //SumTotalRow.ToPay = ToPaysumtotal;
            //End
            UpdateTotalRebateAndToPay();
        }
        void OnMonthlyToPayLostFocusExecuted(object MonthlyParameter)
        {
            int YearlyMonthlyToPaysumtotal = 0;
            var fo = PayFeeDetails.Where(x => x.Description != "SumTotal");
            foreach (var found in fo)
            {
                YearlyMonthlyToPaysumtotal = YearlyMonthlyToPaysumtotal + found.ToPay;
            }
            //var Monthlyfound = PayMonthlyFeeDetails.FirstOrDefault(x => x.MonthName == ((object[])(MonthlyParameter))[0]);
            UpdateTotalRebateAndToPay();
            //foreach (var Monthlyfounds in PayMonthlyFeeDetails)
            //{

            //    YearlyMonthlyToPaysumtotal = YearlyMonthlyToPaysumtotal + Monthlyfounds.MonthlyToPay;
            //}
            //PayMonthlyFeeModelObject.YearlyMonthlyToPayTotal = YearlyMonthlyToPaysumtotal;

        }
        void OnRebateLostFocusExecuted(object parameter)
        {
            int RebateSumtotal = 0, ToPaysumtotal = 0;
            var found = PayFeeDetails.FirstOrDefault(x => x.Description == ((object[])(parameter))[0]);
            if (found.ToPay > (found.Balance - Convert.ToInt32(((object[])(parameter))[1])))
            {
                found.ToPay = found.Balance - Convert.ToInt32(((object[])(parameter))[1]);
            }
            //found.ToPay=found.Balance-Convert.ToInt32(((object[])(parameter))[1]);
            var fo = PayFeeDetails.Where(x => x.Description != "SumTotal");
            foreach (var founds in fo)
            {

                RebateSumtotal = RebateSumtotal + founds.Rebate;
                ToPaysumtotal = ToPaysumtotal + founds.ToPay;
            }
            //Edited By Anand Date 20Sep'15 Start : Sum total removed.
            //var SumTotalRow = PayFeeDetails.FirstOrDefault(x => x.Description == "SumTotal");
            //SumTotalRow.Rebate = RebateSumtotal;
            //SumTotalRow.ToPay = ToPaysumtotal;
            //End
            UpdateTotalRebateAndToPay();

        }
        void OnMonthlyRebateLostFocusExecuted(object Monthlyparameter)
        {
            int TotalRebateSumtotal = 0, TotalToPaysumtotal = 0;
            var found = PayMonthlyFeeDetails.FirstOrDefault(x => x.MonthName == ((object[])(Monthlyparameter))[0]);
            if (found.MonthlyToPay > (found.MonthlyBalance - Convert.ToInt32(((object[])(Monthlyparameter))[1])))
            {
                found.MonthlyToPay = found.MonthlyBalance - Convert.ToInt32(((object[])(Monthlyparameter))[1]);
            }
            //found.ToPay=found.Balance-Convert.ToInt32(((object[])(parameter))[1]);
            UpdateTotalRebateAndToPay();
            //var fo = PayFeeDetails.Where(x => x.Description != "SumTotal");
            //foreach (var founds in fo)
            //{

            //    TotalRebateSumtotal = TotalRebateSumtotal + founds.Rebate;
            //    TotalToPaysumtotal = TotalToPaysumtotal + founds.ToPay;
            //}
            ////var Monthlyfo = PayMonthlyFeeDetails.Where(x => x.Description != "SumTotal");
            //foreach (var Monthlyfounds in PayMonthlyFeeDetails)
            //{

            //    TotalRebateSumtotal = TotalRebateSumtotal + Monthlyfounds.MonthlyRebate;
            //    TotalToPaysumtotal = TotalToPaysumtotal + Monthlyfounds.MonthlyToPay;
            //}
            //PayMonthlyFeeModelObject.YearlyMonthlyRebateTotal = TotalRebateSumtotal;
            //PayMonthlyFeeModelObject.YearlyMonthlyToPayTotal = TotalToPaysumtotal;

        }
        void UpdateTotalRebateAndToPay()
        {
            int TotalRebateSumtotal = 0, TotalToPaysumtotal = 0, TotalRequiredTotal = 0, TotalLessGiven = 0, TotalAmountPaid = 0;
            int CheckboxTrueCounter = 0;
            var fo = PayFeeDetails.Where(x => x.Description != "SumTotal");
            foreach (var founds in fo)
            {
                if (founds.Checkboxtesting)
                {
                    TotalRequiredTotal = TotalRequiredTotal + founds.RequiredFee;
                    TotalLessGiven = TotalLessGiven + founds.RebateGiven;
                    TotalAmountPaid = TotalAmountPaid + founds.Paid;

                }
                TotalRebateSumtotal = TotalRebateSumtotal + founds.Rebate;
                TotalToPaysumtotal = TotalToPaysumtotal + founds.ToPay;
                if (founds.Checkboxtesting)
                {
                    CheckboxTrueCounter++;
                }
            }
            foreach (var Monthlyfounds in PayMonthlyFeeDetails)
            {
                if (Monthlyfounds.MonthlyCheckBox)
                {
                    TotalRequiredTotal = TotalRequiredTotal + Monthlyfounds.RequiredMonthlyFee;
                    TotalLessGiven = TotalLessGiven + Monthlyfounds.MonthlyRebateGiven;
                    TotalAmountPaid = TotalAmountPaid + Monthlyfounds.Monthlypaid;
                }
                TotalRebateSumtotal = TotalRebateSumtotal + Monthlyfounds.MonthlyRebate;
                TotalToPaysumtotal = TotalToPaysumtotal + Monthlyfounds.MonthlyToPay;
                if (Monthlyfounds.MonthlyCheckBox)
                {
                    CheckboxTrueCounter++;
                }
            }
            PayMonthlyFeeModelObject.YearlyMonthlyBalance = TotalRequiredTotal - TotalRebateSumtotal - TotalToPaysumtotal - TotalLessGiven - TotalAmountPaid;
            PayMonthlyFeeModelObject.YearlyMonthlyRequiredTotal = TotalRequiredTotal - TotalLessGiven - TotalAmountPaid;
            PayMonthlyFeeModelObject.YearlyMonthlyRebateTotal = TotalRebateSumtotal;
            PayMonthlyFeeModelObject.YearlyMonthlyToPayTotal = TotalToPaysumtotal;
            payMonthlyFeeModelObject.FeeSlipNo = PaySlipNo;

            if (CheckboxTrueCounter != 0)
            {
                payFeeModelObject.Checkboxtesting = true;
            }
            else
            {
                payFeeModelObject.Checkboxtesting = false;
            }
        }

    }
}
