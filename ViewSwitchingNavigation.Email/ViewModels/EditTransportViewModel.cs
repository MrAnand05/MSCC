using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using ViewSwitchingNavigation.Email.Model;
using ViewSwitchingNavigation.Infrastructure;

namespace ViewSwitchingNavigation.Email.ViewModels
{
    [Export]
    public class EditTransportViewModel:BindableBase
    {
        public EditTransportViewModel()
        {
            this.Months = new[] { "04", "05", "06", "07", "08", "09", "10", "11", "12", "01", "02", "03" };
            
            //this.Months = new ObservableCollection<string>();
            this.RaiseSearchStudentCommand = new DelegateCommand<object>(RaiseSearchStudent);
            this.SearchStudentRequest = new InteractionRequest<SearchStudent>();
            returnedStudent = new SearchStudentOutputModel();
            EditTransport = new EditTransportModel();
            this.EndMonthsCurrentList = new List<string>();
            EndMonthCurrentSelectionChangedCommand = new DelegateCommand<object>(EndMonthCurrentSelectionChanged);
            StartMonthNewSelectionChangedCommand = new DelegateCommand<object>(StartMonthNewSelectionChanged);
            RouteSelectionChangedCommand = new DelegateCommand<object>(RouteSelectionChanged);
            this.UpdateCommand = new DelegateCommand<EditTransportModel>(UpdateTransport);
            this.RouteObservable = new ObservableCollection<int>();
            this.StopObservable = new ObservableCollection<string>();

        }
        public static string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
        SqlConnection con = new SqlConnection(connectionString);
        DataSet ds = new DataSet();
        public IEnumerable<string> Months { get; private set; }
        public ICommand RaiseSearchStudentCommand { get; private set; }
        public ICommand EndMonthCurrentSelectionChangedCommand { get; private set; }
        public ICommand StartMonthNewSelectionChangedCommand { get; private set; }
        public ICommand RouteSelectionChangedCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }
        private ObservableCollection<string> endMonthsCurrentObservable;
        private ObservableCollection<string> startMonthsNewObservable;
        private ObservableCollection<string> endMonthsNewObservable;
        private ObservableCollection<int> routeObservable;
        private ObservableCollection<string> stopObservable;
        public ObservableCollection<string> EndMonthsCurrentObservable
        {
            get { return endMonthsCurrentObservable; }
            set
            {
                SetProperty(ref this.endMonthsCurrentObservable, value);
            }
        }
        public ObservableCollection<string> StartMonthsNewObservable
        {
            get { return startMonthsNewObservable; }
            set
            {
                SetProperty(ref this.startMonthsNewObservable, value);
            }
        }
        public ObservableCollection<string> EndMonthsNewObservable
        {
            get { return endMonthsNewObservable; }
            set
            {
                SetProperty(ref this.endMonthsNewObservable, value);
            }
        }
        public ObservableCollection<int> RouteObservable
        {
            get { return routeObservable; }
            set
            {
                SetProperty(ref this.routeObservable, value);
            }
        }
        public ObservableCollection<string> StopObservable
        {
            get { return stopObservable; }
            set
            {
                SetProperty(ref this.stopObservable, value);
            }
        }
        public IEnumerable<string> EndMonthsCurrentEnumerable  { get; private set; }
        public List<string> EndMonthsCurrentList { get; private set; }
        //public ObservableCollection<string> Months { get; private set; }
        
        public InteractionRequest<SearchStudent> SearchStudentRequest { get; private set; }
        public SearchStudentOutputModel returnedStudent { get; set; }
        public EditTransportModel EditTransport { get; set; }
        public void RaiseSearchStudent(object obj)
        {
            SearchStudent student1 = new SearchStudent();
            student1.Title = "Search Student";
            this.SearchStudentRequest.Raise(student1,
                returned =>
                {
                    if (returned != null && returned.Confirmed && returned.SelectedStudent != null)
                    {
                        this.returnedStudent = returned.SelectedStudent;
                        if (obj.ToString() == "SearchTransport")
                        {
                            SqlCommand commandTrans = new SqlCommand("Usp_TransportRouteFeeRelation", con);
                            commandTrans.CommandType = CommandType.StoredProcedure;
                            SqlDataAdapter da = new SqlDataAdapter();
                            try
                            {
                                da.SelectCommand = commandTrans;
                                da.Fill(ds);
                            }
                            catch
                            {

                            }
                            finally
                            {
                                commandTrans.Dispose();
                            }
                            RouteObservable.Clear();
                            DataTable dt = ds.Tables[0].DefaultView.ToTable(true, "RouteNo", "RouteDescription");
                            var res = dt.AsEnumerable().Select(a => new { Route = a.Field<int>("RouteNo") + "-" + a.Field<string>("RouteDescription"), RouteNo = a.Field<int>("RouteNo") });
                            foreach (var item in res)
                            {
                                RouteObservable.Add(item.RouteNo);
                            }
                            //foreach (DataRow dr in ds.Tables[0].Rows)
                            //{
                            //    RouteObservable.Add(int.Parse(dr["RouteNo"].ToString()));
                            //}
                            if (endMonthsCurrentObservable!=null)
                            {
                                endMonthsCurrentObservable.Clear();
                            }
                            if (startMonthsNewObservable!=null)
                            {
                                startMonthsNewObservable.Clear();
                            }
                            if (EndMonthsNewObservable!=null)
                            {
                                EndMonthsNewObservable.Clear();
                            }
                            this.EditTransport.AdminNo = returnedStudent.Admino;
                            this.EditTransport.Name = string.Format("{0} {1}", returnedStudent.FirstName, returnedStudent.LastName);
                            this.EditTransport.ClassSec = string.Format("{0}/{1}", returnedStudent.Class, returnedStudent.Section);
                            this.EditTransport.Address = string.Format("{0}-{1}", returnedStudent.PAddress, returnedStudent.PDistrict);
                            this.EditTransport.RouteNoCurrent = returnedStudent.RountNo;
                            this.EditTransport.StopCurrent = returnedStudent.Stop;
                            this.EditTransport.StartMonthCurrent = returnedStudent.StartMonth;
                            this.EditTransport.IsAvailingTransport=returnedStudent.IsAvailingTransport;
                            int length=2;

                            string SMonth=returnedStudent.StartMonth.ToString().PadLeft(length,'0').ToString();
                            if(returnedStudent.IsAvailingTransport)
                            {
                                this.EndMonthsCurrentList = new List<string>();
                                int index=Months.ToList().IndexOf(SMonth)+1;

                                SqlCommand command = new SqlCommand("Usp_FeePaidMonthIndex", con);
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@AdminNo", ((object)returnedStudent.Admino ?? DBNull.Value));
                                command.Parameters.AddWithValue("@Financialyear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                                try
                                {
                                    SqlParameter returnParameter = command.Parameters.Add("@return_value", SqlDbType.Int);
                                    returnParameter.Direction = ParameterDirection.ReturnValue;
                                    con.Open();
                                    command.ExecuteNonQuery();

                                    int FeePaidMonthIndex = (int)returnParameter.Value;


                                    if (index < FeePaidMonthIndex)
                                    {
                                        index = FeePaidMonthIndex;
                                    }
                                    for (int i = index; i < Months.ToList().Count(); i++)
                                    {
                                        this.EndMonthsCurrentList.Add(Months.ToList()[i].ToString());
                                    }
                                    this.EndMonthsCurrentObservable = new ObservableCollection<string>(EndMonthsCurrentList);
                                }
                                catch
                                {

                                }
                                finally
                                {
                                    con.Close();
                                    command.Dispose();
                                }
                            }
                            else
                            {

                                object NewstartMonth = "New";
                                EndMonthCurrentSelectionChanged(NewstartMonth);
                            }
                        }
                    }
                    else
                    {
                        //this.InteractionResultMessage = "The user cancelled the operation or didn't select an item.";
                    }

                });
        }

        public void UpdateTransport(EditTransportModel TransportModel)
        {
            bool Execute = false;
            SqlCommand cmd = new SqlCommand("Usp_UpdateTransport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            if (TransportModel.IsAvailingTransport && (string.IsNullOrWhiteSpace(TransportModel.EndMonthCurrent) ? string.Empty : TransportModel.EndMonthCurrent) == string.Empty)
            {
                MessageBox.Show("Please close current Transport Details");
            }
            else if (TransportModel.IsAvailingTransport && (string.IsNullOrWhiteSpace(TransportModel.StartMonthNew) ? string.Empty : TransportModel.StartMonthNew) == string.Empty
                && (string.IsNullOrWhiteSpace(TransportModel.EndMonthCurrent) ? string.Empty : TransportModel.EndMonthCurrent) != string.Empty)
            {
                //Close Old Transport Details
                cmd.Parameters.AddWithValue("@AdminNo", ((object)TransportModel.AdminNo ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@EndMonthCurrent", ((object)TransportModel.EndMonthCurrent ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@RouteNo",  DBNull.Value);
                cmd.Parameters.AddWithValue("@Stop", DBNull.Value);
                cmd.Parameters.AddWithValue("@StartMonthNew", DBNull.Value);
                Execute = true;
            }
            else if (!TransportModel.IsAvailingTransport && (string.IsNullOrWhiteSpace(TransportModel.StartMonthNew) ? string.Empty : TransportModel.StartMonthNew) != string.Empty
                && (string.IsNullOrWhiteSpace(TransportModel.StopNew) ? string.Empty : TransportModel.StopNew) != string.Empty)
            {
                //Enter New Transport Details
                cmd.Parameters.AddWithValue("@AdminNo", ((object)TransportModel.AdminNo ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@EndMonthCurrent", DBNull.Value);
                cmd.Parameters.AddWithValue("@RouteNo", ((object)TransportModel.RouteNoNew ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@Stop", ((object)TransportModel.StopNew ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@StartMonthNew", ((object)TransportModel.StartMonthNew ?? DBNull.Value));
                Execute = true;
            }
            else if(TransportModel.IsAvailingTransport && (string.IsNullOrWhiteSpace(TransportModel.EndMonthCurrent) ? string.Empty : TransportModel.EndMonthCurrent) != string.Empty
                && (string.IsNullOrWhiteSpace(TransportModel.StartMonthNew) ? string.Empty : TransportModel.StartMonthNew) != string.Empty
                && (string.IsNullOrWhiteSpace(TransportModel.StopNew) ? string.Empty : TransportModel.StopNew) != string.Empty)
            {
                //Close Old Transport Detail and Add New Transport
                cmd.Parameters.AddWithValue("@AdminNo", ((object)TransportModel.AdminNo ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@EndMonthCurrent", ((object)TransportModel.EndMonthCurrent ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@RouteNo", ((object)TransportModel.RouteNoNew ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@Stop", ((object)TransportModel.StopNew ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@StartMonthNew", ((object)TransportModel.StartMonthNew ?? DBNull.Value));
                Execute = true;
            }
            else
            {
                MessageBox.Show("Check all transport details ");
            }
            if(Execute)
            {
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
                MessageBox.Show("Transport Details Update ");
            }
        }
        public void EndMonthCurrentSelectionChanged(object args)
         {
            int index;
            if (StartMonthsNewObservable != null)
            {
                StartMonthsNewObservable.Clear();
            }
            if(args.ToString()=="New")
            {
                index = 0;
            }
            else
            {
                string EndMonthCurrent = args.ToString();
                index = Months.ToList().IndexOf(EndMonthCurrent);
            }
            this.EndMonthsCurrentList = new List<string>();
            for (int i = index; i < Months.ToList().Count(); i++)
            {
                this.EndMonthsCurrentList.Add(Months.ToList()[i].ToString());
            }
            this.StartMonthsNewObservable = new ObservableCollection<string>(EndMonthsCurrentList);
        }
        public void StartMonthNewSelectionChanged(object args)
        {
            if (EndMonthsNewObservable!=null)
            {
                EndMonthsNewObservable.Clear();
            }
            if(args!=null)
            {
                string StartMonthNew = args.ToString();
                this.EndMonthsCurrentList = new List<string>();
                int index = Months.ToList().IndexOf(StartMonthNew) + 1;
                for (int i = index; i < Months.ToList().Count(); i++)
                {
                    this.EndMonthsCurrentList.Add(Months.ToList()[i].ToString());
                }
                this.EndMonthsNewObservable = new ObservableCollection<string>(EndMonthsCurrentList);
            }
        }
        public void RouteSelectionChanged(object args)
        {
            StopObservable.Clear();
            if(args!=null)
            {
                string RouteNoName = args.ToString();
                int RouteNo = Convert.ToInt32(RouteNoName);

                DataTable dt = ds.Tables[0].DefaultView.ToTable(true,"RouteNo", "Stop");
                DataRow[] row1 = dt.Select(("RouteNo=" + RouteNo));
                //DataRow[] row1 = ds.Tables[0].Select(("RouteNo=" + RouteNo));

                foreach (DataRow dr in row1)
                {
                    StopObservable.Add(dr["Stop"].ToString());
                }
            }
        }
    }
}
