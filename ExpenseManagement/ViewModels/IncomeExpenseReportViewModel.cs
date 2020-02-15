using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using ViewSwitchingNavigation.ExpenseManagement.Model;
using ViewSwitchingNavigation.ExpenseManagement.Views;
using ViewSwitchingNavigation.Infrastructure;

namespace ViewSwitchingNavigation.ExpenseManagement.ViewModels
{
    [Export]
    public class IncomeExpenseReportViewModel:BindableBase
    {
        public ObservableCollection<IncomeExpenseModel> IncomeExpense { get; set; }
        public List<string> Source { get; set; }
        public List<string> IncomeExpenseOption { get; set; }
        public IncomeExpenseModel IncomeExpenseObj { get; set; }
    #region Properties
        private string cmbSelectedSource;
        private string cmbSelectedType;
        private int enteredAmount;
        private DateTime selectedDate;
        public string CmbSelectedSource
        {
            get { return cmbSelectedSource; }
            set
            {
                SetProperty(ref this.cmbSelectedSource, value);
            }
        }
        public string CmbSelectedType
        {
            get { return cmbSelectedType; }
            set
            {
                SetProperty(ref this.cmbSelectedType, value);
            }
        }
        public int EnteredAmount
        {
            get { return enteredAmount; }
            set
            {
                SetProperty(ref this.enteredAmount, value);
            }
        }
        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set
            {
                SetProperty(ref this.selectedDate, value);
            }
        }
    #endregion
        public DelegateCommand<object> AddItemCommand { get; private set; }
        public DelegateCommand<object> DeleteCommand { get; private set; }
        public DelegateCommand<object> SaveUpdateCommand { get; private set; }
        public DelegateCommand<object> EditCommand { get; private set; }
        public DelegateCommand<object> PrintCommand { get; private set; }
        public DelegateCommand<object> SelectionChangedCommand { get; private set; }

        public static string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
        SqlConnection con = new SqlConnection(connectionString);
        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        DateTime IEDate = new DateTime();
        public IncomeExpenseReportViewModel()
        {
            this.IncomeExpense = new ObservableCollection<IncomeExpenseModel>();
            this.Source = new List<string>();
            this.IncomeExpenseOption = new List<string> {"Income","Expense"};
            this.IncomeExpenseObj = new IncomeExpenseModel();
            this.AddItemCommand = new DelegateCommand<object>(OnAddItemExecuted);
            this.DeleteCommand= new DelegateCommand<object>(OnDeleteItemExecuted);
            this.SaveUpdateCommand = new DelegateCommand<object>(OnSaveUpdateExecuted);
            this.EditCommand = new DelegateCommand<object>(OnEditExecuted);
            this.PrintCommand = new DelegateCommand<object>(OnPrintExecuted);
            this.SelectionChangedCommand = new DelegateCommand<object>(OnSourceListChange);
            this.IEDate=this.SelectedDate = DateTime.Now;
            UpdateSourceList();
            GetIncomeExpense(IEDate,IEDate);
        }
        public void UpdateSourceList()
        {
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
            foreach(DataRow dr in ds1.Tables[0].Rows)
            {
                Source.Add((string)dr["IncomeExpenseSource"]);
            }
        }
        public void GetIncomeExpense(DateTime dt,DateTime frmdt)
        {
            IEDate = dt;
            IncomeExpense.Clear();
            ds.Clear();
            SqlCommand command = new SqlCommand("Usp_GetIncomeExpenseByDate", con);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@IEDate", ((object)dt ?? DBNull.Value));
            command.Parameters.AddWithValue("@FrmIEDate", ((object)frmdt ?? DBNull.Value));
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                da.SelectCommand = command;
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                command.Dispose();
            }
            if (ds.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    var obj = new IncomeExpenseModel();
                    obj.SNo = (int)dr["SNo"];
                    obj.IESource = (string)dr["IESource"];
                    obj.Income = (int)dr["Income"];
                    obj.Expense = (int)dr["Expense"];
                    obj.IEDate = IEDate = (DateTime)dr["IEDate"];
                    obj.Balance = (int)dr["Balance"];
                    obj.IsDBData = true;
                    obj.IsVisible = "Hidden";
                    obj.IsChkVisible = "Visible";
                    obj.IsChecked = false;
                    IncomeExpense.Add(obj);
                }
            }
        }
        public void OnAddItemExecuted(object parameters)
        {
            int maxSNo = (IncomeExpense.Count==0)?0: IncomeExpense.Max(x => x.SNo);
            var obj = new IncomeExpenseModel();
            obj.SNo = maxSNo+1;
            obj.IESource = CmbSelectedSource;
            if (CmbSelectedType == "Income")
                obj.Income = EnteredAmount;
            else
                obj.Expense = EnteredAmount;
            obj.IEDate =IEDate;
            obj.IsDBData = false;
            obj.IsVisible = "Visible";
            obj.IsChkVisible = "Hidden";
            obj.IsChecked = true;
            IncomeExpense.Add(obj);
        }
        public void OnDeleteItemExecuted(object parameters)
        {
            IncomeExpense.Remove(IncomeExpense.Single(s => s.SNo ==(int)parameters));
            foreach (var item in IncomeExpense.Where(w => w.SNo > (int)parameters))
            {
                item.SNo = item.SNo-1;
            }
        }
        public void OnSaveUpdateExecuted(object parameters)
        {
            var CheckedIncomeExpense = IncomeExpense.Where(c => c.IsChecked == true);
            try
            {
                con.Open();
                foreach (IncomeExpenseModel item in CheckedIncomeExpense)
                {
                    SqlCommand cmd = new SqlCommand("Usp_SaveUpdateIncomeExpense", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SNo", ((object)item.SNo ?? DBNull.Value));
                    cmd.Parameters.AddWithValue("@IESource", ((object)item.IESource ?? DBNull.Value));
                    cmd.Parameters.AddWithValue("@Income", ((object)item.Income ?? DBNull.Value));
                    cmd.Parameters.AddWithValue("@Expense", ((object)item.Expense?? DBNull.Value));
                    cmd.Parameters.AddWithValue("@IEDate", ((object)IEDate ?? DBNull.Value));
                    cmd.ExecuteScalar();
                    cmd.Dispose();
                }
                if (CheckedIncomeExpense.Count() != 0)
                    MessageBox.Show("Data Saved.");
                else
                    MessageBox.Show("Please add or select row to Update.");
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
            GetIncomeExpense(IEDate,IEDate);
        }
        public void OnEditExecuted(object parameters)
        {
            GetIncomeExpense(SelectedDate,SelectedDate);
        }
        public void OnPrintExecuted(object parameters)
        {

            //DublicateFeeSlip vm2 = new DublicateFeeSlip(SelectedDate);
            IncomeExpensePrint vm = new IncomeExpensePrint(SelectedDate);
            vm.ShowDialog();
        }
        public void OnSourceListChange(object parameters)
        {
            if(parameters.ToString().ToUpper()=="Fee Collection".ToUpper())
            {
                DataTable dt = new DataTable();
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("Usp_GetFeeTransaction", cn);
                    cmd.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                    //cmd.Parameters.AddWithValue("@FrmDate", ((object)DateTime.Now ?? DBNull.Value));
                    //cmd.Parameters.AddWithValue("@ToDate", ((object)DateTime.Now ?? DBNull.Value));
                    cmd.Parameters.AddWithValue("@FrmDate", ((object)SelectedDate ?? DBNull.Value));
                    cmd.Parameters.AddWithValue("@ToDate", ((object)SelectedDate ?? DBNull.Value));
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                }
                object sumObject;
                sumObject = dt.Compute("Sum(TotalAmount)", "");
                if( !string.IsNullOrEmpty(sumObject.ToString()))
                { 
                    EnteredAmount =Convert.ToInt32(sumObject);

                }
            }
            else
                EnteredAmount = 0;
        }
    }
}
