using CameraControlTool;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ViewSwitchingNavigation.Email.Model;
using ViewSwitchingNavigation.Email.Views;
using ViewSwitchingNavigation.Infrastructure;

namespace ViewSwitchingNavigation.Email.ViewModels
{
    [Export]
    public class RegisterStudentViewModel:BindableBase
    {

        public List<string> Religion { get; set; }
        public List<string> Category { get; set; }
        public List<string> Classes { get; set; }
        public List<string> SMedium { get; set; }
        public List<string> SectionList { get; set; }
        public List<string> Classes1 { get; set; }
        public List<RouteNoName> RouteList { get; set; }
        public IEnumerable<int> Months { get; set; }
        private ObservableCollection<StopFair> stopList;
        public ObservableCollection<StopFair> StopList
        {
            get { return stopList; }
            set
            {
                SetProperty(ref this.stopList, value);
            }
        }
        
        //private readonly IStudentRegistrationService studentService;
        private  ObservableCollection<int> StudentClasses { get; set; }
        
        //public ObservableCollection<studentservice> LineItems { get; private set; }
        private StudentRegistration stud;
        public StudentRegistrationTopModel StudentRegistrationTop { get; set; }
        DateTime now = DateTime.Now;
        public static string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
        SqlConnection con = new SqlConnection(connectionString);
        DataSet ds = new DataSet();
        private int SelectedIndexPC;
        public int selectedIndexPC
        {
            get { return SelectedIndexPC; }
            set
            {
                SetProperty(ref this.SelectedIndexPC, value);
                //this.OnPropertyChanged(() => this.SelectedIndexPC);
            }
        }
        //Opening new Search window
        public InteractionRequest<SearchStudent> SearchStudentRequest { get; private set; }
        public InteractionRequest<INotification> NotificationRequest { get; private set; }
        public ICommand RaiseSearchStudentCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand NewRegistrationCommand { get; private set; }
        public ICommand UpdateRegistrationCommand { get; private set; }
        public ICommand DocIdCommand { get; private set; }
        public ICommand IsAvailingTransportCommand { get; private set; }
        public ICommand SelectionChangedCommand { get; private set; }
        public ICommand RouteSelectionChangedCommand { get; private set; }
        public ICommand RaiseNotificationCommand { get; private set; }
        public ICommand ImageSelectorCommand { get; private set; }
        public ICommand RemoveImageCommand { get; private set; }
        public ICommand BrowseImageCommand { get; private set; }
        public ICommand ShowChildWindowCommand { get; private set; }

        private string resultMessage;
        public SearchStudentOutputModel returnedStudent { get; set; }
        public string InteractionResultMessage
        {
            get
            {
                return this.resultMessage;
            }
            set
            {
                this.resultMessage = value;
                this.OnPropertyChanged("InteractionResultMessage");
            }
        }
        private Sibling sb1;
        public Sibling sb2 { get; set; }
        public RegisterStudentViewModel()
        {
            this.Months = new[] { 4, 5, 6, 7, 8, 9, 10, 11, 12, 1, 2, 3 };
            Sb1 = new Sibling();
            sb2 = new Sibling();
            stud = new StudentRegistration();
            this.StudentClasses = new ObservableCollection<int>() { 1,2};
            this.StudentRegistrationTop = new StudentRegistrationTopModel();
            this.stud.DOJ = now;
            this.stud.DOB = now;
            this.stud.YOPLC = now;            
            this.Religion = new List<string> { "Hindu", "Muslim", "Sikh","Christians","Others" };
            this.Category = new List<string> { "General", "OBC", "SC", "ST", "Others" };
            this.SMedium = new List<string> { "English", "Hindi", "Others" };
            this.SectionList = new List<string> { "A", "B", "C", "D" };
            this.Classes1 = new List<string>();
            this.RouteList = new List<RouteNoName>();
            this.StopList = new ObservableCollection<StopFair>();
            
            this.RaiseSearchStudentCommand = new DelegateCommand<object>(RaiseSearchStudent);
            //this.RaiseNotificationCommand = new DelegateCommand(this.RaiseNotification);
            this.SearchStudentRequest = new InteractionRequest<SearchStudent>();
            this.NotificationRequest = new InteractionRequest<INotification>();
            SelectionChangedCommand = new DelegateCommand<object>(SelectionChanged);
            RouteSelectionChangedCommand = new DelegateCommand<object>(RouteSelectionChanged);
            
            //AdmittedToClassChangedCommand = new DelegateCommand<object>(AdmittedToClassChanged);
            SaveCommand = new DelegateCommand<object>(Save,(stu)=> CanSave);
            NewRegistrationCommand = new DelegateCommand(NewRegistration);
            UpdateRegistrationCommand=new DelegateCommand(UpdateRegistration);
            ImageSelectorCommand = new DelegateCommand(ImageSelector);
            RemoveImageCommand = new DelegateCommand(RemoveImage);
            BrowseImageCommand = new DelegateCommand(BrowseImage);
            DocIdCommand = new DelegateCommand<object>(AssignDocId);
            IsAvailingTransportCommand = new DelegateCommand<object>(DeAssignRouteAndStop);
            //Getting Class and Fee Relation data from database into dataset
            SqlCommand command = new SqlCommand("Usp_ClassFeeRelation", con);
            SqlCommand commandTrans = new SqlCommand("Usp_TransportRouteFeeRelation", con);
            commandTrans.CommandType = CommandType.StoredProcedure;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                da.SelectCommand = command;
                da.Fill(ds);
                DataTable TransportRouteFeeRelation = new DataTable();
                ds.Tables.Add(TransportRouteFeeRelation);
                da.SelectCommand = commandTrans;
                da.Fill(ds.Tables[1]);
            }
            catch
            {

            }
            finally
            {
                command.Dispose();
                commandTrans.Dispose();
            }
            
            List<string> list = new List<string>();
            foreach(DataRow dr in ds.Tables[0].Rows)
            {
                Classes1.Add(dr["Class"].ToString());
            }
            //TransportRouteFee Table
            DataTable dt = ds.Tables[1].DefaultView.ToTable(true, "RouteNo", "RouteDescription");
            var res = dt.AsEnumerable().Select(a => new { Route = a.Field<int>("RouteNo") +"-"+ a.Field<string>("RouteDescription"),RouteNo=a.Field<int>("RouteNo") });
            foreach (var item in res)
            {
                RouteNoName routeNoName = new RouteNoName();
                routeNoName.RouteNo = item.RouteNo;
                routeNoName.RouteDescription = item.Route;
                RouteList.Add(routeNoName);
            }
        }
        public class RouteNoName:BindableBase
        {
            private string routeDescription;
            private int? routeNo;
            public string RouteDescription
            {
                get { return this.routeDescription; }
                set
                {
                    SetProperty(ref this.routeDescription, value);
                }
            }
            public int? RouteNo
            {
                get { return this.routeNo; }
                set
                {
                    SetProperty(ref this.routeNo, value);
                }
            }

        }
        public class StopFair : BindableBase
        {
            private string stops;
            private string fare;
            public string Stops
            {
                get { return this.stops; }
                set
                {
                    SetProperty(ref this.stops, value);
                }
            }
            public string Fare
            {
                get { return this.fare; }
                set
                {
                    SetProperty(ref this.fare, value);
                }
            }

        }
        public StudentRegistration Stud
        {
            get { return stud; }
            set { stud = value; }
        }
        public Sibling Sb1
        {
            get { return sb1; }
            set { sb1 = value; }
        }
        public void SelectionChanged(object args)
        {
            int a = int.Parse(args.ToString());
            selectedIndexPC = a + 1;
            
        }
        public void RouteSelectionChanged(object args)
        {
            int? RouteNoName=null;
            bool execute = false;
            if(args!=null)
            {
                //RouteNoName = args.ToString();
                //((ViewSwitchingNavigation.Email.ViewModels.RegisterStudentViewModel.RouteNoName)(args)).RouteNo
                RouteNoName=((RouteNoName)(args)).RouteNo;
                execute = true;
            }
            else if(Stud.RouteNo!=null)
            {
                RouteNoName = Stud.RouteNo;
                execute = true;
            }
            else
            {
                execute = false;
            }
            if(execute)
            {
                StopList.Clear();
                int? RouteNo = RouteNoName;
                DataRow[] row1 = ds.Tables[1].Select(("RouteNo=" + RouteNo));
                foreach (DataRow dr in row1)
                {
                    StopFair SF = new StopFair();
                    SF.Stops = dr["Stop"].ToString();
                    SF.Fare = dr["Stop"].ToString() + "-" + dr["TransportFee"].ToString();
                    StopList.Add(SF);
                }
            }            
        }
        public bool CanSave
        {
            get { return true; }
        }
        private byte[] GetArrayOfPixels(BitmapSource bitmapsource)
        {

            Int32 stride = bitmapsource.PixelWidth * bitmapsource.Format.BitsPerPixel / 8;

            Int32 ByteSize = stride * bitmapsource.PixelHeight * bitmapsource.Format.BitsPerPixel / 8;

            byte[] arrayofpixel = new byte[ByteSize];

            bitmapsource.CopyPixels(arrayofpixel, stride, 0);

            return arrayofpixel;

        }
        public void Save(object stu)
        {
            if ((string.IsNullOrWhiteSpace(stud.FirstName) ? string.Empty : stud.FirstName) == string.Empty)
            {
                System.Windows.Forms.MessageBox.Show("Student Name is Mandatory Fields");
            }
            else if (stud.IsAvailingTransport && (stud.StartMonth==null || stud.RouteNo == null || (string.IsNullOrWhiteSpace(stud.Stop) ? string.Empty : stud.Stop) == string.Empty))
            {
                System.Windows.Forms.MessageBox.Show("Please check RouteNo , Stop or StartMonth");
            }
            else if (stud.Class == null )
            {
                System.Windows.Forms.MessageBox.Show("Please Select Class.");
            }
            else
            {
                byte[] content=null;
                if(Stud.Image!=null)
                {
                    bitmap1 = GetBitmap(Stud.Image);
                    content = ReadBitmap2ByteArray(bitmap1);
                }
                //int p = LogOn.a;
                int FinancialYear = AuthenticationContext.GlobalFinancialYear;
                //if (stud.IsOldStudentRegistration)
                //{ 
                //    FinancialYear = 0;  //For Old Student Financial Year is 0.
                //}
                //Commented By Anand

                //byte[] photo = File.ReadAllBytes("pack://application:,,,/ViewSwitchingNavigation;component/Resources/NMCLogo.png");
                //stud.Sib1AdminNo = null;
                //stud.Sib2AdminNo = null;
                //byte[] photo = GetPhoto(stud.Photo);
                int adminno;
                SqlCommand cmd = new SqlCommand("Usp_RegisterStudent", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FirstName", ((object)stud.FirstName ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@LastName", ((object)stud.LastName ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@DOJ", ((object)stud.DOJ ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@FName", ((object)stud.FName ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@MName", ((object)stud.MName ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@DOB", ((object)stud.DOB ?? DBNull.Value));
                cmd.Parameters.Add("@Gender", SqlDbType.NVarChar).Value = stud.Gender;
                //cmd.Parameters.AddWithValue("@Gender", ((object)stud.Gender ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@Religion", ((object)stud.Religion ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@Category", ((object)stud.Category ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@PAddress", ((object)stud.PAddress ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@PDistrict", ((object)stud.PDistrict ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@PContact", ((object)stud.PContact ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@LAddress", ((object)stud.LAddress ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@LDistrict", ((object)stud.LDistrict ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@LContact", ((object)stud.LContact ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@Sib1AdminNo", ((object)stud.Sib1AdminNo ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@Sib2AdminNo", ((object)stud.Sib2AdminNo ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@SiblingConcession", ((object)stud.SiblingConcession ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@SiblingLessAmount", ((object)stud.SiblingLessAmount ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@Class", ((object)stud.Class ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@Section", ((object)stud.Section ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@PSchoolName", ((object)stud.PSchoolName ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@LClass", ((object)stud.LClass ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@YOPLC", ((object)stud.YOPLC ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@LCMarks", ((object)stud.LCMarks ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@Medium", ((object)stud.Medium ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@DocID", ((object)stud.DocID ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@IsOldStudentRegistration", ((object)stud.IsOldStudentRegistration ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@IsAvailingTransport", ((object)stud.IsAvailingTransport?? DBNull.Value));
                cmd.Parameters.AddWithValue("@StartMonth", ((object)stud.StartMonth?? DBNull.Value));
                cmd.Parameters.AddWithValue("@RouteNo", ((object)stud.RouteNo ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@Stop", ((object)stud.Stop?? DBNull.Value));
                cmd.Parameters.AddWithValue("@FinancialYear", ((object)FinancialYear ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@StudentRemark", ((object)stud.StudentRemark ?? DBNull.Value));
                //cmd.Parameters.AddWithValue("@Photo", ((object)stud.Photo ?? DBNull.Value));
                //Comented By anand
                //cmd.Parameters.Add("@Photo", System.Data.SqlDbType.Image, photo.Length).Value = photo;
                cmd.Parameters.AddWithValue("@Photo", ((object)content ?? (object)DBNull.Value)).SqlDbType = SqlDbType.Image;
                cmd.Parameters.AddWithValue("@EnteredBy", ((object)"" ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@UpdatedBy", ((object)"" ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@ScholarNo", ((object)stud.ScholarNo ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@AdminNo", SqlDbType.Int));
                cmd.Parameters["@AdminNo"].Direction = ParameterDirection.Output;

                //cmd.Parameters.AddWithValue("@LastName", stud.LastName);
                try
                {
                    con.Open();
                    cmd.ExecuteScalar();
                    adminno = (int)cmd.Parameters["@AdminNo"].Value;
                    //stud.FirstName = "changed";
                    stud.AdminNo = adminno;
                    StudentRegistrationTop.IsNewRegistration = false;
                    StudentRegistrationTop.IsSaveBtnEnable = false;
                    StudentRegistrationTop.IsUpdateBtnEnable = false;


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
                System.Windows.Forms.MessageBox.Show(string.Format("Admission Number Generated: {0} ", adminno));
              }
        }
        public void UpdateRegistration()
        {
            if(stud.SiblingConcession==false)
            {
                stud.SiblingLessAmount = 0;
            }
            if ((string.IsNullOrWhiteSpace(stud.FirstName) ? string.Empty : stud.FirstName) == string.Empty)
            {
                System.Windows.Forms.MessageBox.Show("Student Name is Mandatory Fields");
            }
            else if (stud.IsAvailingTransport && (stud.StartMonth == null || stud.RouteNo == null || (string.IsNullOrWhiteSpace(stud.Stop) ? string.Empty : stud.Stop) == string.Empty))
            {
                System.Windows.Forms.MessageBox.Show("Please check RouteNo , Stop or StartMonth");
            }
            else
            {
                Bitmap bitmap1;
                byte[] UpdatedPic = null;
                //Stud.Image = loadBitmap(bitmap);
                if(Stud.Image!=null)
                { 
                    bitmap1 = GetBitmap(Stud.Image);
                    UpdatedPic = ReadBitmap2ByteArray(bitmap1);
                }
                SqlCommand cmd = new SqlCommand("Usp_UpdateStudent", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FirstName", ((object)stud.FirstName ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@LastName", ((object)stud.LastName ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@DOJ", ((object)stud.DOJ ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@FName", ((object)stud.FName ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@MName", ((object)stud.MName ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@DOB", ((object)stud.DOB ?? DBNull.Value));
                cmd.Parameters.Add("@Gender", SqlDbType.NVarChar).Value = stud.Gender;
                //cmd.Parameters.AddWithValue("@Gender", ((object)stud.Gender ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@Religion", ((object)stud.Religion ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@Category", ((object)stud.Category ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@PAddress", ((object)stud.PAddress ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@PDistrict", ((object)stud.PDistrict ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@PContact", ((object)stud.PContact ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@LAddress", ((object)stud.LAddress ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@LDistrict", ((object)stud.LDistrict ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@LContact", ((object)stud.LContact ?? DBNull.Value));
                //cmd.Parameters.AddWithValue("@Sib1AdminNo", ((object)stud.Sib1AdminNo ?? DBNull.Value));
                //cmd.Parameters.AddWithValue("@Sib2AdminNo", ((object)stud.Sib2AdminNo ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@SiblingConcession", ((object)stud.SiblingConcession ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@SiblingLessAmount", ((object)stud.SiblingLessAmount ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@Class", ((object)stud.Class ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@Section", ((object)stud.Section ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@PSchoolName", ((object)stud.PSchoolName ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@LClass", ((object)stud.LClass ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@YOPLC", ((object)stud.YOPLC ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@LCMarks", ((object)stud.LCMarks ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@Medium", ((object)stud.Medium ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@DocID", ((object)stud.DocID ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@IsOldStudentRegistration", ((object)stud.IsOldStudentRegistration ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@IsAvailingTransport", ((object)stud.IsAvailingTransport ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@StartMonth", ((object)stud.StartMonth ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@RouteNo", ((object)stud.RouteNo ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@Stop", ((object)stud.Stop ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@StudentRemark", ((object)stud.StudentRemark ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@Photo", ((object)UpdatedPic ?? DBNull.Value)).SqlDbType=SqlDbType.Image;
                //Comented By anand
                //cmd.Parameters.Add("@Photo", System.Data.SqlDbType.Image, photo.Length).Value = photo;
                //cmd.Parameters.AddWithValue("@Photo",((object)stud.Photo??DBNull.Value));
                cmd.Parameters.AddWithValue("@EnteredBy", ((object)"" ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@UpdatedBy", ((object)"" ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@ScholarNo", ((object)stud.ScholarNo ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@AdminNo", ((object)stud.AdminNo ?? DBNull.Value));
                //cmd.Parameters.Add(new SqlParameter("@AdminNo", SqlDbType.Int));
                //cmd.Parameters["@AdminNo"].Direction = ParameterDirection.Output;

                //cmd.Parameters.AddWithValue("@LastName", stud.LastName);
                try
                {
                    con.Open();
                    cmd.ExecuteScalar();
                    //adminno = (int)cmd.Parameters["@AdminNo"].Value;
                    ////stud.FirstName = "changed";
                    //stud.AdminNo = adminno;
                    StudentRegistrationTop.IsNewRegistration = false;
                    StudentRegistrationTop.IsSaveBtnEnable = false;
                    StudentRegistrationTop.IsUpdateBtnEnable = false;


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
                System.Windows.Forms.MessageBox.Show("Student Details Updated");
            }
        }
        public void NewRegistration()
        {
            StudentRegistrationTop.IsNewRegistration =true;
            StudentRegistrationTop.IsSaveBtnEnable = true;
            StudentRegistrationTop.IsUpdateBtnEnable = false;
            StudentRegistrationTop.IsBSTranClassFieldEnable = true;
            StudentRegistrationTop.IsClassEnable = true;
            stud.Reset();
            Sb1.Reset();
            sb2.Reset();
        }
        public void AssignDocId(object parameters)
        {
            stud.DocID = 0;
            if ((bool)((object[])parameters)[0])
            {
                stud.DocID = 1;
            }
            if ((bool)((object[])parameters)[1])
            {
                stud.DocID = 2;
            }
            if ((bool)((object[])parameters)[0] && (bool)((object[])parameters)[1])
            {
                stud.DocID = 3;
            }

        }
        public void DeAssignRouteAndStop(object parameters)
        {
            if(!(bool)((object[])parameters)[0])
            {
                stud.StartMonth = null;
                stud.RouteNo = null;
                stud.Stop = string.Empty;
            }
        }
        public static byte[] GetPhoto(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            byte[] photo = br.ReadBytes((int)fs.Length);

            br.Close();
            fs.Close();

            return photo;
        }
        private void RaiseNotification(string req)
        {
            // By invoking the Raise method we are raising the Raised event and triggering any InteractionRequestTrigger that
            // is subscribed to it.
            // As parameters we are passing a Notification, which is a default implementation of INotification provided by Prism
            // and a callback that is executed when the interaction finishes.
            this.NotificationRequest.Raise(
               new Notification { Content = "Notification Message", Title = "Notification" },
               n => { InteractionResultMessage = "The user was notified."; });
        }
        public void RaiseSearchStudent(object obj)
        {
            SearchStudent student1 = new SearchStudent();
            
            SearchStudentViewTest vm = new SearchStudentViewTest();
            vm.ShowDialog();
            returnedStudent = vm.SelectedItem;
            //Added By Anand Start
            DataSet ds = new DataSet();
            SqlCommand command = new SqlCommand("Usp_FindStudent", con);
            command.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                command.Parameters.AddWithValue("@AdminNo", ((object)returnedStudent.Admino ?? DBNull.Value));
                command.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                command.Parameters.AddWithValue("@FirstName", DBNull.Value);
                command.Parameters.AddWithValue("@LastName", DBNull.Value);
                command.Parameters.AddWithValue("@FName", DBNull.Value);
                command.Parameters.AddWithValue("@RollNo", DBNull.Value);
                command.Parameters.AddWithValue("@Class", DBNull.Value);
                //command.Parameters.AddWithValue("@Class", ((object)CopySte.Class ?? DBNull.Value));
                command.Parameters.AddWithValue("@Section", DBNull.Value);
                //command.Parameters.AddWithValue("@Section", ((object)CopySte.Section ?? DBNull.Value));
                command.Parameters.AddWithValue("@StartDate", DBNull.Value);
                command.Parameters.AddWithValue("@EndDate", DBNull.Value);
                da.SelectCommand = command;
                da.Fill(ds);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (row["Photo"] != DBNull.Value)
                    {
                        //Added by Anand Start
                        byte[] content = (byte[])row["Photo"];
                        MemoryStream stream = new MemoryStream(content);
                        Bitmap image = new Bitmap(stream);
                        System.Drawing.Image returnImage = System.Drawing.Image.FromStream(stream);
                        returnImage.Save(System.IO.Path.GetTempPath() + "\\myImage.Jpeg", ImageFormat.Jpeg);
                        //System.Drawing.Image image1 = Bitmap.FromStream(stream);
                        //obj.StuImage = loadBitmap(image);
                        Stud.Image = loadBitmap(image);

                        using (MemoryStream mStream = new MemoryStream())
                        {
                            mStream.Write(content, 0, content.Length);
                            mStream.Seek(0, SeekOrigin.Begin);

                            Bitmap bm = new Bitmap(mStream);
                            //obj.StuImage = loadBitmap(bm);
                            //return bm;
                            //Stud.Image=loadBitmap(image);
                        }

                        MemoryStream strm = new MemoryStream();

                        strm.Write(content, 0, content.Length);

                        strm.Position = 0;

                        System.Drawing.Image img = System.Drawing.Image.FromStream(strm);

                        BitmapImage bi = new BitmapImage();

                        bi.BeginInit();

                        MemoryStream ms = new MemoryStream();

                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

                        img.Save(System.IO.Path.GetTempPath() + "\\test.Jpeg", ImageFormat.Jpeg);
                        ms.Seek(0, SeekOrigin.Begin);

                        bi.StreamSource = ms;

                        bi.EndInit();
                        //loadBitmap(returnImage);
                        //obj.StuImage = bi;
                        //End
                    }
                }
            }
            //End

            catch
            {

            }
            finally
            {
                command.Dispose();
            }
            if (obj.ToString() == "SearchSb1")
            {

                //SearchStudentViewModel SelectedItem
                this.Stud.Sib1AdminNo = returnedStudent.Admino;
                this.Sb1.Sname = string.Format("{0} {1}", returnedStudent.FirstName, returnedStudent.LastName);
                this.Sb1.Sclass = returnedStudent.Class;
                this.Sb1.Ssection = returnedStudent.Section;
            }
            if (obj.ToString() == "SearchSb2")
            {
                this.Stud.Sib2AdminNo = returnedStudent.Admino;
                this.sb2.Sname = string.Format("{0} {1}", returnedStudent.FirstName, returnedStudent.LastName);
                this.sb2.Sclass = returnedStudent.Class;
                this.sb2.Ssection = returnedStudent.Section;
            }
            if (obj.ToString() == "SearchEdit")
            {
                //Added by Anand Start
                //if(returnedStudent.StuImage!=null)
                    //this.Stud.Image = loadBitmap(returnedStudent.StuImage);
                //End
                StudentRegistrationTop.IsNewRegistration = true;
                StudentRegistrationTop.IsUpdateBtnEnable = true;
                StudentRegistrationTop.IsSaveBtnEnable = false;
                StudentRegistrationTop.IsClassEnable = true;
                //if (returnedStudent.FinancialYear != returnedStudent.CurrentFinancialYear)
                //{
                //    StudentRegistrationTop.IsClassEnable = false;
                //}
                stud.Reset();
                if(returnedStudent.StuImage!=null)
                    this.Stud.Image = loadBitmap(returnedStudent.StuImage);
                this.Stud.FirstName = returnedStudent.FirstName;
                this.Stud.LastName = returnedStudent.LastName;
                this.Stud.DOJ = Convert.ToDateTime(returnedStudent.DOJ);
                this.Stud.FName = returnedStudent.FName;
                this.Stud.MName = returnedStudent.MName;
                this.Stud.DOB = Convert.ToDateTime(returnedStudent.DOB);
                this.Stud.Gender = returnedStudent.Gender;
                this.Stud.Religion = returnedStudent.Religion;
                this.Stud.Category = returnedStudent.Category;
                this.Stud.PSchoolName = returnedStudent.PSchoolName;
                this.Stud.LClass = returnedStudent.LClass;
                this.Stud.YOPLC = Convert.ToDateTime(returnedStudent.YOPLC);
                this.Stud.LCMarks = returnedStudent.LCMarks;
                this.Stud.Medium = returnedStudent.Medium;
                this.Stud.PAddress = returnedStudent.PAddress;
                this.Stud.PDistrict = returnedStudent.PDistrict;
                this.Stud.PContact = returnedStudent.PContact;
                this.Stud.LAddress = returnedStudent.LAddress;
                this.Stud.LDistrict = returnedStudent.LDistrict;
                this.Stud.LContact = returnedStudent.LContact;
                this.Stud.Sib1AdminNo = returnedStudent.Sib1AdminNo;
                this.Stud.Sib2AdminNo = returnedStudent.Sib2AdminNo;
                this.Stud.Class = returnedStudent.Class;
                this.Stud.Section = returnedStudent.Section;
                this.Stud.AdminNo = returnedStudent.Admino;
                this.Stud.SiblingConcession = returnedStudent.SiblingConcession;
                this.Stud.SiblingLessAmount = returnedStudent.SiblingLessAmount;
                if (returnedStudent.FinancialYear == returnedStudent.CurrentFinancialYear)
                    this.Stud.IsOldStudentRegistration = false;
                else
                    this.Stud.IsOldStudentRegistration = true;
                this.Stud.IsAvailingTransport = returnedStudent.IsAvailingTransport;
                this.Stud.StartMonth = returnedStudent.StartMonth;
                this.Stud.RouteNo = returnedStudent.RountNo;
                this.Stud.Stop = returnedStudent.Stop;
                this.Stud.ScholarNo = returnedStudent.ScholarNo;
                this.Stud.AdminNo = returnedStudent.Admino;
                StudentRegistrationTop.IsBSTranClassFieldEnable = true;
            }

        }
        Bitmap bitmap;
        Bitmap bitmap1;
        public void ImageSelector()
        {
            FormCameraControlTool frm = new FormCameraControlTool();
            //DublicateFeeSlip vm = new DublicateFeeSlip(parameters);
            frm.ShowDialog();
            bitmap = frm.ScreenShot;
            Stud.Image = loadBitmap(bitmap);
            bitmap1 = GetBitmap(Stud.Image);
            
        }
        public void RemoveImage()
        {
            Stud.Image = null;
        }
        public void BrowseImage()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = "c:\\";
            dlg.Filter = "All image formats (*.jpg; *.jpeg; *.bmp; *.png; *.gif)|*.jpg;*.jpeg;*.bmp;*.png;*.gif";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string selectedFileName = dlg.FileName;
                //FileNameLabel.Content = selectedFileName;
                BitmapImage bitmap2 = new BitmapImage();
                Bitmap bitmap3 = new Bitmap(selectedFileName);
                bitmap2.BeginInit();
                bitmap2.UriSource = new Uri(selectedFileName);
                bitmap2.EndInit();
                //Anand Start
                //double maxAspect = (double)118 / (double)150;
                //double aspect = (double)bitmap3.Width / (double)bitmap3.Height;
                //double resizeWidth = 118;
                //double resizeHeight = 150;

                //if (maxAspect > aspect && bitmap3.Width > 118)
                //{
                    //Width is the bigger dimension relative to max bounds
                    //resizeWidth = 118;
                    //resizeHeight = 118/ aspect;
                //}
                //else if (maxAspect <= aspect && bitmap3.Height > 150)
                //{
                    //Height is the bigger dimension
                    //resizeHeight = 150;
                    //resizeWidth = 150 * aspect;
                //}
                //End
                //Rectangle r = new Rectangle(0, 0, 118, 150);
                //bitmap3 = cropAtRect(bitmap3, r);

                Bitmap newImage = new Bitmap(118, 150);
                using (Graphics gr = Graphics.FromImage(newImage))
                {
                    gr.SmoothingMode = SmoothingMode.HighQuality;
                    gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    gr.DrawImage(bitmap3, new Rectangle(0, 0, 118, 150));
                }
                Bitmap FinalImage = FixedSize(bitmap3, 118, 150, true);
                Stud.Image = loadBitmap(FinalImage);
                //ImageViewer1.Source = bitmap;
            }
        }
        private Bitmap cropImage(Bitmap img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        }
        public Bitmap cropAtRect(Bitmap b, Rectangle r)
        {
            Bitmap nb = new Bitmap(r.Width, r.Height);
            Graphics g = Graphics.FromImage(nb);
            g.DrawImage(b, -r.X, -r.Y);
            return nb;
        }
        public Bitmap FixedSize(Bitmap image, int Width, int Height, bool needToFill)
        {
            #region много арифметики
            int sourceWidth = image.Width;
            int sourceHeight = image.Height;
            int sourceX = 0;
            int sourceY = 0;
            double destX = 0;
            double destY = 0;

            double nScale = 0;
            double nScaleW = 0;
            double nScaleH = 0;

            nScaleW = ((double)Width / (double)sourceWidth);
            nScaleH = ((double)Height / (double)sourceHeight);
            if (!needToFill)
            {
                nScale = Math.Min(nScaleH, nScaleW);
            }
            else
            {
                nScale = Math.Max(nScaleH, nScaleW);
                destY = (Height - sourceHeight * nScale) / 2;
                destX = (Width - sourceWidth * nScale) / 2;
            }

            if (nScale > 1)
                nScale = 1;

            int destWidth = (int)Math.Round(sourceWidth * nScale);
            int destHeight = (int)Math.Round(sourceHeight * nScale);
            #endregion

            System.Drawing.Bitmap bmPhoto = null;
            try
            {
                bmPhoto = new System.Drawing.Bitmap(destWidth + (int)Math.Round(2 * destX), destHeight + (int)Math.Round(2 * destY));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("destWidth:{0}, destX:{1}, destHeight:{2}, desxtY:{3}, Width:{4}, Height:{5}",
                    destWidth, destX, destHeight, destY, Width, Height), ex);
            }
            using (System.Drawing.Graphics grPhoto = System.Drawing.Graphics.FromImage(bmPhoto))
            {
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic; // _interpolationMode;
                grPhoto.CompositingQuality = CompositingQuality.HighQuality; // _compositingQuality;
                grPhoto.SmoothingMode = SmoothingMode.HighQuality; ;// _smoothingMode;

                Rectangle to = new System.Drawing.Rectangle((int)Math.Round(destX), (int)Math.Round(destY), destWidth, destHeight);
                Rectangle from = new System.Drawing.Rectangle(sourceX, sourceY, sourceWidth, sourceHeight);
                //Console.WriteLine("From: " + from.ToString());
                //Console.WriteLine("To: " + to.ToString());
                grPhoto.DrawImage(image, to, from, System.Drawing.GraphicsUnit.Pixel);

                return bmPhoto as Bitmap;
            }
        }
        public Bitmap GetBitmap(BitmapSource source)
        {
            Bitmap bmp = new Bitmap(
              source.PixelWidth,
              source.PixelHeight,
              PixelFormat.Format32bppPArgb);
            BitmapData data = bmp.LockBits(
              new Rectangle(System.Drawing.Point.Empty, bmp.Size),
              ImageLockMode.WriteOnly,
              PixelFormat.Format32bppPArgb);
            source.CopyPixels(
              Int32Rect.Empty,
              data.Scan0,
              data.Height * data.Stride,
              data.Stride);
            bmp.UnlockBits(data);
            return bmp;
        }

        protected static byte[] ReadBitmap2ByteArray(Bitmap image)
        {
            //using (Bitmap image = new Bitmap(fileName))
            //{
                MemoryStream stream = new MemoryStream();
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                return stream.ToArray();
            //}
        }
        [DllImport("gdi32")]
        static extern int DeleteObject(IntPtr o);

        public static BitmapSource loadBitmap(System.Drawing.Bitmap source)
        {
            IntPtr ip = source.GetHbitmap();
            BitmapSource bs = null;
            try
            {
                bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip,
                   IntPtr.Zero, Int32Rect.Empty,
                   System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(ip);
            }

            return bs;
        }
 
    }
}
