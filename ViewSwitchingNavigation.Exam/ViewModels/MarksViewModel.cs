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

namespace ViewSwitchingNavigation.Exam.ViewModels
{
    [Export]
    public class MarksViewModel : BindableBase
    {
        public List<Classes> ObjClasses { get; set; }
        //public List<string> TermL = new List<string>() { "1", "2" };
        public Classes SelectedClass { get; set; }
        public Subjects SelectedSubject { get; set; }
        private ObservableCollection<Subjects> objSubjects;
        public ObservableCollection<Subjects> ObjSubjects
        {
            get { return objSubjects; }
            set
            {
                SetProperty(ref this.objSubjects, value);
            }
        }
        public InteractionRequest<INotification> NotificationRequest { get; private set; }
        public ICommand ClassSelectionChangedCommand { get; private set; }
        public ICommand RaiseNotificationCommand { get; private set; }
        public static string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
        SqlConnection con = new SqlConnection(connectionString);
        DataSet ds = new DataSet();


            SqlDataAdapter da = new SqlDataAdapter();
        public MarksViewModel()
        {
            this.ObjClasses = new List<Classes>();
            this.ObjSubjects = new ObservableCollection<Subjects>();
            this.SelectedClass = new Classes();
            this.NotificationRequest = new InteractionRequest<INotification>();
            ClassSelectionChangedCommand = new DelegateCommand<object>(ClassSelectionChanged);
            SqlCommand command = new SqlCommand("Usp_GetClasses", con);
            command.CommandType = CommandType.StoredProcedure;
            SqlCommand command1 = new SqlCommand("Usp_ClassSubRel", con);
            command1.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                da.SelectCommand = command;
                da.Fill(ds);
                DataTable dtSubjects = new DataTable();
                ds.Tables.Add(dtSubjects);
                da.SelectCommand = command1;
                da.Fill(ds.Tables[1]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                command.Dispose();
                command1.Dispose();
            }
            DataTable dt = ds.Tables[0].DefaultView.ToTable(true, "SNo", "Class");
            var res = dt.AsEnumerable().Select(a => new { SNo = a.Field<int>("SNo"), Classes = a.Field<string>("Class") });
            foreach (var item in res)
            {
                Classes ObjtClasses = new Classes();
                ObjtClasses.SNo= item.SNo;
                ObjtClasses.SClass= item.Classes;
                ObjClasses.Add(ObjtClasses);
            }
            ClassSelectionChanged(ObjClasses[0]);
        }
        public class Classes : BindableBase
        {
            private string sClass;
            private int? sNo;
            public string SClass
            {
                get { return this.sClass; }
                set
                {
                    SetProperty(ref this.sClass, value);
                }
            }
            public int? SNo
            {
                get { return this.sNo; }
                set
                {
                    SetProperty(ref this.sNo, value);
                }
            }

        }
        public class Subjects : BindableBase
        {
            private string subject;
            private string sNo;
            public string Subject
            {
                get { return this.subject; }
                set
                {
                    SetProperty(ref this.subject, value);
                }
            }
            public string SNo
            {
                get { return this.sNo; }
                set
                {
                    SetProperty(ref this.sNo, value);
                }
            }

        }
        public void ClassSelectionChanged(object args)
        {
            string ObjSelectedClass = null;
            bool execute = false;
            if (args != null)
            {
                ObjSelectedClass = ((Classes)(args)).SClass;
                execute = true;
            }
            else if (SelectedClass.SClass != null)
            {
                ObjSelectedClass = SelectedClass.SClass;
                execute = true;
            }
            else
            {
                execute = false;
            }
            if (execute)
            {
                ObjSubjects.Clear();
                //Subjects SubAll = new Subjects();
                //SubAll.SNo = "0";
                //SubAll.Subject = "ALL";
                //ObjSubjects.Add(SubAll);
                string SelectedTClass = ObjSelectedClass;
                DataRow[] row1 = ds.Tables[1].Select("Class='" + ObjSelectedClass.ToString()+"'");
                foreach (DataRow dr in row1)
                {
                    Subjects Sub = new Subjects();
                    Sub.Subject= dr["SubName"].ToString();
                    Sub.SNo = dr["SubID"].ToString();
                    ObjSubjects.Add(Sub);
                }
                
            }
        }
       
    }
}
