using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ViewSwitchingNavigation.Email.Model;
using ViewSwitchingNavigation.Infrastructure;

namespace ViewSwitchingNavigation.Email.Views
{
    /// <summary>
    /// Interaction logic for SearchStudent.xaml
    /// </summary>
    public partial class SearchStudentViewTest : Window
    {
        public static string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
        SqlConnection con = new SqlConnection(connectionString);
        public SearchStudentOutputModel SelectedItem { get; set; }
        public List<string> Classes1 { get; private set; }
        public IEnumerable<string> SectionList { get; private set; }
        public ObservableCollection<SearchStudentOutputModel> SearchedStudents { get; set; }
        public SearchStudentViewTest()
        {
            InitializeComponent();
            this.Classes1 = new List<string>();
            this.SectionList = new[] { DBNull.Value.ToString(), "A", "B", "C", "D" };
            dtPickerStartDt.SelectedDate = DateTime.Now.AddDays(-15);
            dtPickerEndDt.SelectedDate = DateTime.Now;
            this.SelectedItem=new SearchStudentOutputModel();
            this.SearchedStudents = new ObservableCollection<SearchStudentOutputModel>();
            int FinancialYear = AuthenticationContext.GlobalFinancialYear;
            DataSet dc = new DataSet();

            SqlCommand command = new SqlCommand("Usp_ClassFeeRelation", con);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@FinancialYear", ((object)FinancialYear ?? DBNull.Value));
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                da.SelectCommand = command;
                da.Fill(dc);
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
            foreach (DataRow dr in dc.Tables[0].Rows)
            {
                Classes1.Add(dr["Class"].ToString());
            }
            cmbClass.ItemsSource = Classes1;
            cmbSection.ItemsSource = SectionList;
        }

        public string radioseleted;
        protected void HandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectedItem =(SearchStudentOutputModel)lstViewSearchedStudent.SelectedItem;
            this.Close();
        }
        private void RadiobtnAdmin_Checked(object sender, RoutedEventArgs e)
        {
            var button = sender as RadioButton;
            radioseleted = button.Name.ToString();
        }

        private void btn_SearchClick(object sender, RoutedEventArgs e)
        {
            SearchStudentModel CopySte = new SearchStudentModel();
            if (txtAdminNo.Text!="" && txtAdminNo.Text!=null)
                CopySte.Admino = Convert.ToInt32(txtAdminNo.Text);
            CopySte.StartDate = dtPickerStartDt.SelectedDate;
            CopySte.EndDate = dtPickerEndDt.SelectedDate;
            CopySte.FirstName = txtSFirstName.Text;
            CopySte.LastName = "";//txtSLName.Text;
            CopySte.FName = txtSFatherName.Text;
            if (cmbClass.SelectedItem!=null)
                CopySte.Class = cmbClass.SelectedItem.ToString();
            if(cmbSection.SelectedItem!=null)
            CopySte.Section = cmbSection.SelectedItem.ToString();
            if (radioseleted == "Radio1" || radioseleted == null)
            {
                CopySte.StartDate = null;
                CopySte.EndDate = null;
                CopySte.FirstName = null;
                CopySte.LastName = null;
                CopySte.FName = null;
                CopySte.Class = null;
                CopySte.Section = null;
                CopySte.RollNo = null;
            }
            if (radioseleted == "Radio2")
            {
                CopySte.Admino = null;
                CopySte.FirstName = null;
                CopySte.LastName = null;
                CopySte.FName = null;
                CopySte.Class = null;
                CopySte.Section = null;
                CopySte.RollNo = null;
            }
            if (radioseleted == "Radio3")
            {
                CopySte.Admino = null;
                CopySte.StartDate = null;
                CopySte.EndDate = null;
                CopySte.FirstName = null;
                CopySte.LastName = null;
                CopySte.FName = null;
                CopySte.Class = null;
                CopySte.Section = null;
                CopySte.RollNo = null;
            }
            if (radioseleted == "Radio4")
            {
                CopySte.Admino = null;
                CopySte.StartDate = null;
                CopySte.EndDate = null;
            }
            SearchedStudents.Clear();
            //this.SearchedStudents = new ObservableCollection<SearchStudentOutputModel>();
            DataSet ds = new DataSet();
            SqlCommand command = new SqlCommand("Usp_FindStudent", con);
            command.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                command.Parameters.AddWithValue("@AdminNo", ((object)CopySte.Admino ?? DBNull.Value));
                command.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                command.Parameters.AddWithValue("@FirstName", ((object)CopySte.FirstName ?? DBNull.Value));
                command.Parameters.AddWithValue("@LastName", ((object)CopySte.LastName ?? DBNull.Value));
                command.Parameters.AddWithValue("@FName", ((object)CopySte.FName ?? DBNull.Value));
                command.Parameters.AddWithValue("@RollNo", ((object)CopySte.RollNo ?? DBNull.Value));
                command.Parameters.AddWithValue("@Class", String.IsNullOrWhiteSpace(CopySte.Class) ? (object)DBNull.Value : (object)CopySte.Class);
                //command.Parameters.AddWithValue("@Class", ((object)CopySte.Class ?? DBNull.Value));
                command.Parameters.AddWithValue("@Section", String.IsNullOrWhiteSpace(CopySte.Section) ? (object)DBNull.Value : (object)CopySte.Section);
                //command.Parameters.AddWithValue("@Section", ((object)CopySte.Section ?? DBNull.Value));
                command.Parameters.AddWithValue("@StartDate", ((object)CopySte.StartDate ?? DBNull.Value));
                command.Parameters.AddWithValue("@EndDate", ((object)CopySte.EndDate ?? DBNull.Value));
                da.SelectCommand = command;
                da.Fill(ds);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var obj = new SearchStudentOutputModel();
                    if(row["Photo"]!=DBNull.Value)
                    {
                        //Added by Anand Start
                        byte[] content = (byte[])row["Photo"];
                        MemoryStream stream = new MemoryStream(content);
                        Bitmap image = new Bitmap(stream);
                        System.Drawing.Image returnImage = System.Drawing.Image.FromStream(stream);
                        returnImage.Save(System.IO.Path.GetTempPath() + "\\myImage.Jpeg", ImageFormat.Jpeg);
                        //System.Drawing.Image image1 = Bitmap.FromStream(stream);
                        //obj.StuImage = loadBitmap(image);

                        using (MemoryStream mStream = new MemoryStream())
                        {
                            mStream.Write(content, 0, content.Length);
                            mStream.Seek(0, SeekOrigin.Begin);

                            Bitmap bm = new Bitmap(mStream);
                            //obj.StuImage = loadBitmap(bm);
                            //return bm;
                            obj.StuImage = image;
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
                    obj.Admino = (int?)row["AdminNo"];
                    obj.DOJ = ((DateTime)row["DOJ"]).ToShortDateString();
                    obj.FirstName = row["FirstName"].ToString();
                    obj.LastName = row["LastName"].ToString();
                    obj.FName = row["FName"].ToString();
                    obj.MName = row["MName"].ToString();
                    obj.DOB = ((DateTime)row["DOB"]).ToShortDateString();
                    if (row["Gender"].ToString() == "Male")
                    {
                        obj.Gender = Gender.Male;
                    }
                    else
                        obj.Gender = Gender.Female;
                    obj.Religion = row["Religion"].ToString();
                    obj.Category = row["Category"].ToString();
                    obj.PSchoolName = row["PSchoolName"].ToString();
                    obj.LClass = row["LClass"].ToString();
                    obj.YOPLC = ((DateTime)row["YOPLC"]).ToShortDateString();
                    obj.LCMarks = (int)row["LCMarks"];
                    if (row["Medium"].ToString() == "E")
                    {
                        obj.Medium = "English";
                    }
                    else if (row["Medium"].ToString() == "H")
                    {
                        obj.Medium = "Hindi";
                    }
                    else
                    {
                        obj.Medium = "Others";
                    }
                    obj.DocID = (int)row["DocID"];
                    obj.PAddress = row["PAddress"].ToString();
                    obj.PDistrict = row["PDistrict"].ToString();
                    obj.PContact = (Int64)row["PContact"];
                    obj.LAddress = row["LAddress"].ToString();
                    obj.LDistrict = row["LDistrict"].ToString();
                    obj.LContact = (Int64)row["LContact"];
                    obj.Class = row["CurrentClass"].ToString();
                    obj.Section = row["Section"].ToString();
                    obj.RollNo = row["RollNo"].ToString();
                    obj.SiblingConcession = (bool)row["SiblingConcession"];
                    if (obj.SiblingConcession && row["SiblingLessAmount"]!=null)
                    {
                        obj.SiblingLessAmount = (int)row["SiblingLessAmount"];
                    }
                    obj.IsAvailingTransport = (bool)row["IsAvailingTransport"];
                    if ((bool)row["IsAvailingTransport"])
                    {
                        obj.RountNo = (int)row["RouteNo"];
                        obj.Stop = row["Stop"].ToString();
                        obj.StartMonth = (int)row["startMonth"];
                    }
                    obj.FinancialYear = (int)row["FinancialYear"];
                    obj.CurrentFinancialYear = (int)row["CurrentFinancialYear"];
                    if (row["ScholarNo"] != null && row["ScholarNo"] != "")
                        obj.ScholarNo = (int?)row["ScholarNo"];
                    SearchedStudents.Add(obj);
                }
                lstViewSearchedStudent.ItemsSource = SearchedStudents;          
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                command.Dispose();
            }
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
