using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ViewSwitchingNavigation.Email.Model;
using ViewSwitchingNavigation.Infrastructure;

namespace ViewSwitchingNavigation.Reports.Views
{
    /// <summary>
    /// Interaction logic for FeeBalance.xaml
    /// </summary>
    public partial class SelectIDCards : UserControl
    {
        List<int?> selectedAdminNo = new List<int?>();
        public SelectIDCards()
        {
            InitializeComponent();
            List<string> LSClass = new List<string>() {"","000-Nursery","001-LKG","002-UKG","01","02","03","04","05","06","07","08","09","10","11","12" };
            List<string> LSSection = new List<string>() {"","A","B","C","D" };
            cmbClass.ItemsSource = LSClass;
            cmbSec.ItemsSource = LSSection;
        }
        private readonly ImageCon _imgcon = new ImageCon();
        byte[] content;
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            selectedAdminNo.Clear();
            DataTable dt1 = GetData();
            ImageConverter imgCon = new ImageConverter();
            content = (byte[])imgCon.ConvertTo(Properties.Resources.NMCLogo, typeof(byte[]));
            MemoryStream stream = new MemoryStream(content);
            Bitmap image = new Bitmap(stream);
            
            List<PayFeeModel> lstStudentDB = new List<PayFeeModel>();
            lstStudentDB = (from DataRow dr in dt1.Rows
                            select new PayFeeModel()
                           {
                               Checkboxtesting=false,
                               AdminNo= Convert.ToInt32(dr["AdminNo"]),
                               Name = dr["Name"].ToString(),
                               CurrentClass = dr["CurrentClass"].ToString(),
                               FatherName=dr["FName"].ToString(),
                               Address = dr["Address"].ToString(),
                               Image = _imgcon.loadBitmap(new Bitmap(new MemoryStream(getcontent(dr))))
                           }).ToList();
            lstStudent.ItemsSource = lstStudentDB;
         
        }
        private byte[] getcontent(DataRow dr)
        {
            if (dr["Photo"] != DBNull.Value)
            {
                return (byte[])dr["Photo"];
            }
            else
                return content;


        }
        private DataTable GetData()
        {
            DataTable dt = new DataTable();
            string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
            object SClass = cmbClass.SelectedValue;
            object SSec = cmbSec.SelectedValue;
            object FrmAdminNo = txtFromAdminNo.Text;
            object ToAdminNo = txtToAdminNo.Text;
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("Usp_IDCardAll", cn);
                cmd.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@Class", (String.IsNullOrWhiteSpace(SClass.ToString()) ? DBNull.Value : SClass));
                cmd.Parameters.AddWithValue("@Section", (String.IsNullOrWhiteSpace(SSec.ToString()) ? DBNull.Value : SSec));
                cmd.Parameters.AddWithValue("@FromAdminNo", (String.IsNullOrWhiteSpace(FrmAdminNo.ToString()) ? DBNull.Value : FrmAdminNo));
                cmd.Parameters.AddWithValue("@ToAdminNo", (String.IsNullOrWhiteSpace(ToAdminNo.ToString()) ? DBNull.Value : ToAdminNo));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }
            //dt.Columns.Add("ExamType", typeof(String),txtExam.Text.ToString());
            return dt;
        }

        private void chk_SelectAll_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string Sclass,SSec;
            Sclass=cmbClass.SelectedValue.ToString();
            SSec=cmbSec.SelectedValue.ToString();
            string str = string.Empty;
            str = String.Join(",", selectedAdminNo);
            GenerateIdCard GID = new GenerateIdCard(Sclass, SSec,str,IsChkV.IsChecked);
            GID.ShowDialog();
        }

        
        
        private void chk_Student_Checked(object sender, RoutedEventArgs e)
        {
            var cbSender = sender as CheckBox;
            int? admin = (int)cbSender.Tag;
            selectedAdminNo.Add(admin);
            
        }

        private void chk_Student_Unchecked(object sender, RoutedEventArgs e)
        {
            var cbSender = sender as CheckBox;
            int? admin = (int)cbSender.Tag;
            if (selectedAdminNo.Contains(admin))
                selectedAdminNo.Remove(admin);
        }

       
    }
}
