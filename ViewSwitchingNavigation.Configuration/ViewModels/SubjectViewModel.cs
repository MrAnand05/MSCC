using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ViewSwitchingNavigation.Configuration.Model;

namespace ViewSwitchingNavigation.Configuration.ViewModels
{
    [Export]
    public class SubjectViewModel:BindableBase
    {
        public ObservableCollection<SubjectModel> Subject { get; set; }
        public SubjectModel SubjectObj { get; set; }
        public SubjectTopModel SubjectTopObj { get; set; }
        public DelegateCommand<object> SaveCommand { get; private set; }
        public DelegateCommand<object> ckhCheckedCommand { get; private set; }
        public DelegateCommand<object> UpdateCommand { get; private set; }
        public DelegateCommand<object> PrintCommand { get; private set; }

        public static string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
        SqlConnection con = new SqlConnection(connectionString);
        DataSet ds = new DataSet();
        public SubjectViewModel()
        {
            this.ckhCheckedCommand = new DelegateCommand<object>(OnChecked);
            this.UpdateCommand = new DelegateCommand<object>(OnUpdateExecuted);
            this.Subject = new ObservableCollection<SubjectModel>();
            this.SubjectTopObj = new SubjectTopModel();
            this.SaveCommand = new DelegateCommand<object>(OnSaveExecuted);
            this.SubjectTopObj.Reset();
            //this.PrintCommand = new DelegateCommand<object>(OnPrintExecuted);
            SqlCommand command = new SqlCommand("Usp_SubjectList", con);
            command.CommandType = CommandType.StoredProcedure;
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
            //TransFeeConfigurationTopObj.IsReadOnly = true;
            SubjectTopObj.IsEnableSaveButton = false;
            SubjectTopObj.IsEnableEditButton = false;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                var obj = new SubjectModel();
                obj.SNo= (int)dr["SubId"];
                obj.SubjectName = (string)dr["SubName"];
                Subject.Add(obj);
            }
        }
        public void OnChecked(object parameter)
        {
            int CheckboxTrueCounter = 0;
            foreach(var found in Subject)
            {
                if (found.IsChecked)
                {
                    CheckboxTrueCounter++;
                }
            }
            if (CheckboxTrueCounter != 0)
            {
                SubjectTopObj.IsEnableEditButton = true;
            }
            else
            {
                SubjectTopObj.IsEnableEditButton = false;
            }

        }
        public void OnUpdateExecuted(object parameter)
        {

            foreach (var found in Subject)
            {
                if (found.IsChecked)
                {
                    SqlCommand cmd = new SqlCommand("Usp_UpdateSubject", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SubId", ((object)found.SNo?? DBNull.Value));
                    cmd.Parameters.AddWithValue("@SubName", ((object)found.SubjectName ?? DBNull.Value));
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
            }
            System.Windows.Forms.MessageBox.Show("Source Details Updated");

            ds.Reset();
            SqlCommand command = new SqlCommand("Usp_SubjectList", con);
            command.CommandType = CommandType.StoredProcedure;
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
            Subject.Clear();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                var obj = new SubjectModel();
                obj.SNo = (int)dr["SubId"];
                obj.SubjectName = (string)dr["SubName"];
                Subject.Add(obj);
            }
            SubjectTopObj.Reset();

        }
        public void OnSaveExecuted(object parameter)
        {
            int SNo;
            if (Subject.Any(p => p.SNo == SubjectTopObj.SNo && p.SubjectName.ToUpper() == SubjectTopObj.SubjectName.ToUpper()))
            {
                System.Windows.Forms.MessageBox.Show(string.Format("Data already Exist "));
            }
            else
            {
                SqlCommand cmd = new SqlCommand("Usp_InsertSubject", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SubName", ((object)SubjectTopObj.SubjectName ?? DBNull.Value));
                //cmd.Parameters.AddWithValue("@SNo", ((object)InExpConfigurationTopObj.SNo ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@SubId", SqlDbType.Int));
                cmd.Parameters["@SubId"].Direction = ParameterDirection.Output;
                try
                {
                    con.Open();
                    cmd.ExecuteScalar();
                    SNo = (int)cmd.Parameters["@SubId"].Value;
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
                if (SNo==null)
                {
                    System.Windows.Forms.MessageBox.Show("Data not saved");
                }
                else
                {
                    ds.Reset();
                    System.Windows.Forms.MessageBox.Show(string.Format("SNo Generated: {0} ", SNo));
                    SqlCommand command = new SqlCommand("Usp_SubjectList", con);
                    command.CommandType = CommandType.StoredProcedure;
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
                    Subject.Clear();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        var obj = new SubjectModel();
                        obj.SNo = (int)dr["SubId"];
                        obj.SubjectName = (string)dr["SubName"];
                        Subject.Add(obj);
                    }
                    SubjectTopObj.Reset();
            }
            }
        }

    }
}
