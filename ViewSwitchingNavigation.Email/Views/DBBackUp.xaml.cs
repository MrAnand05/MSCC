using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Forms;

namespace ViewSwitchingNavigation.Email.Views
{
    /// <summary>
    /// Interaction logic for DBBackUp.xaml
    /// </summary>
    public partial class DBBackUp : System.Windows.Controls.UserControl
    {
        public DBBackUp()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if(dlg.ShowDialog()==DialogResult.OK)
            {
                txtLocation.Text = dlg.SelectedPath;
            }
        }
        
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLocation.Text))
            {
                System.Windows.Forms.MessageBox.Show("Please select Location to Save");
            }
            else
            {
                string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
                SqlConnection con = new SqlConnection(connectionString);
                string sql;
                try
                {
                    con.Open();
                    sql = "BACKUP DATABASE SM TO DISK='" + txtLocation.Text + "\\SM_" + DateTime.Now.Day+"_" + DateTime.Now.Month +"_"+ DateTime.Now.Year+"_" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".bak'";
                    SqlCommand command = new SqlCommand(sql, con);
                    command.ExecuteNonQuery();
                    con.Close();
                    con.Dispose();
                    System.Windows.Forms.MessageBox.Show("DataBase BackUp Completed");
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);                
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }
    }
}
