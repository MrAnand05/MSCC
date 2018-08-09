using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Input;
using ViewSwitchingNavigation.Configuration.Model;
using ViewSwitchingNavigation.Infrastructure;

namespace ViewSwitchingNavigation.Configuration.ViewModels
{
    [Export]
    public class ClassSubConfigurationViewModel:BindableBase
    {
        public List<string> Classes1 { get; private set; }
        public ObservableCollection<SubIDName> LSubIDName { get; set; }
        public List<SubIDName> OLSubIDName { get; set; }
        public IEnumerable<int> Terms { get; set; }
        public ObservableCollection<ClassSubConfigurationModel> ClassSubConfiguration { get; set; }
        public ClassSubConfigurationModel ClassSubConfigurationObj { get; set; }
        public ClassSubConfigurationTopModel ClassSubConfigurationTopObj { get; set; }
        public DelegateCommand<object> SaveCommand { get; private set; }
        public DelegateCommand<object> ckhCheckedCommand { get; private set; }
        public DelegateCommand<object> UpdateCommand { get; private set; }
        public DelegateCommand<object> DeleteCommand { get; private set; }
        public ICommand ClassSelectionChangedCommand { get; private set; }

        public static string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
        SqlConnection con = new SqlConnection(connectionString);
        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        public class SubIDName : BindableBase
        {
            private int? subId;
            private string subName;
            public int? SubId
            {
                get { return this.subId; }
                set
                {
                    SetProperty(ref this.subId, value);
                }
            }
            public string SubName
            {
                get { return this.subName; }
                set
                {
                    SetProperty(ref this.subName, value);
                }
            }

        }
        public ClassSubConfigurationViewModel()
        {
            this.Classes1 = new List<string>();
            this.LSubIDName = new ObservableCollection<SubIDName>();
            this.OLSubIDName = new List<SubIDName>();
            this.Terms = new[] { 1, 2, 3 };
            this.ckhCheckedCommand = new DelegateCommand<object>(OnChecked);
            this.UpdateCommand = new DelegateCommand<object>(OnUpdateExecuted);
            this.DeleteCommand = new DelegateCommand<object>(OnDeleteExecuted);
            this.ClassSubConfiguration = new ObservableCollection<ClassSubConfigurationModel>();
            this.ClassSubConfigurationTopObj = new ClassSubConfigurationTopModel();
            this.SaveCommand = new DelegateCommand<object>(OnSaveExecuted);
            ClassSelectionChangedCommand = new DelegateCommand<object>(ClassSelectionChanged);
            this.ClassSubConfigurationTopObj.Reset();
            SqlCommand command = new SqlCommand("Usp_ClassFeeRelation", con);
            SqlCommand commandSub = new SqlCommand("Usp_Subjects", con);
            command.CommandType = CommandType.StoredProcedure;
            commandSub.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@FinancialYear", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                da.SelectCommand = command;
                da.Fill(ds);
                DataTable Sub = new DataTable();
                ds.Tables.Add(Sub);
                da.SelectCommand = commandSub;
                da.Fill(ds.Tables[1]);
            }
            catch
            {

            }
            finally
            {
                command.Dispose();
                commandSub.Dispose();
            }

            List<string> list = new List<string>();
            Classes1.Add("");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Classes1.Add(dr["Class"].ToString());
            }
            //DataTable dt = ds.Tables[1].DefaultView.ToTable(true, "RouteNo", "RouteDescription");
            //var res = dt.AsEnumerable().Select(a => new { Route = a.Field<int>("RouteNo") + "-" + a.Field<string>("RouteDescription"), RouteNo = a.Field<int>("RouteNo") });
            SubIDName def=new SubIDName();
            def.SubId=null;
            def.SubName="";
            LSubIDName.Add(def);
            OLSubIDName.Add(def);
            foreach (var item in ds.Tables[1].AsEnumerable())
            {
                SubIDName subIdName = new SubIDName();
                subIdName.SubId = Convert.ToInt32(item[0].ToString());
                subIdName.SubName = item[1].ToString();
                LSubIDName.Add(subIdName);
                OLSubIDName.Add(subIdName);
            }
            SqlCommand commandSCR = new SqlCommand("Usp_ClassSubRel", con);
            commandSCR.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da2 = new SqlDataAdapter();
            try
            {
                da2.SelectCommand = commandSCR;
                da2.Fill(ds1);
            }
            catch
            {

            }
            finally
            {
                commandSCR.Dispose();
            }
            //TransFeeConfigurationTopObj.IsEnableSaveButton = false;
            //TransFeeConfigurationTopObj.IsEnableEditButton = false;
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                var obj = new ClassSubConfigurationModel();
                obj.Id = (int)dr["ID"];
                obj.Sclass = (string)dr["Class"];
                obj.SubId = (int)dr["SubID"];
                obj.Subject = (string)dr["SubName"];
                obj.Term = (int)dr["Terms"];
                obj.Mm = (int)dr["MM"];
                obj.Mm1 = (int)dr["MMD1"];
                obj.Mm2 = (int)dr["MMD2"];
                ClassSubConfiguration.Add(obj);
            }
        }
        public void ClassSelectionChanged(object args)
        {
            LSubIDName.Clear();
            string SelClass = args.ToString();
            IEnumerable<ClassSubConfigurationModel> methodResults = ClassSubConfiguration.Where(a => a.Sclass.ToUpper() == SelClass.ToUpper());
            foreach (SubIDName subname in OLSubIDName)
            {
                if (!methodResults.Where(a=>a.SubId==subname.SubId).Any())
                    LSubIDName.Add(subname);
            }
        }
        public void OnChecked(object parameter)
        {
            int CheckboxTrueCounter = 0;
            foreach (var found in ClassSubConfiguration)
            {
                if (found.IsChecked)
                {
                    CheckboxTrueCounter++;
                }
            }
            if (CheckboxTrueCounter != 0)
            {
                ClassSubConfigurationTopObj.IsEnableEditButton = true;
               
            }
            else
            {
                ClassSubConfigurationTopObj.IsEnableEditButton = false;
            }

        }
        public void OnUpdateExecuted(object parameter)
        {

            foreach (var found in ClassSubConfiguration)
            {
                if (found.IsChecked)
                {
                    SqlCommand cmd = new SqlCommand("Usp_UpdateClassSubRel", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", ((object)found.Id ?? DBNull.Value));
                    cmd.Parameters.AddWithValue("@Term", ((object)found.Term ?? DBNull.Value));
                    cmd.Parameters.AddWithValue("@MM", ((object)found.Mm ?? DBNull.Value));
                    cmd.Parameters.AddWithValue("@Mm1", ((object)found.Mm1 ?? DBNull.Value));
                    cmd.Parameters.AddWithValue("@Mm2", ((object)found.Mm2 ?? DBNull.Value));
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
            System.Windows.Forms.MessageBox.Show("Class Sub Rel Updated");

            UpdateGrid();
        }
        public void OnDeleteExecuted(object parameter)
        {
            foreach (var found in ClassSubConfiguration)
            {
                if (found.IsChecked)
                {
                    SqlCommand cmd = new SqlCommand("Usp_DelClassSubRel", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ((object)found.Id ?? DBNull.Value));
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
            System.Windows.Forms.MessageBox.Show("Class Subject Relation Deleted");

            UpdateGrid();

        }

        private void UpdateGrid()
        {
            ds1.Reset();
            SqlCommand commandSCR = new SqlCommand("Usp_ClassSubRel", con);
            commandSCR.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da2 = new SqlDataAdapter();
            try
            {
                da2.SelectCommand = commandSCR;
                da2.Fill(ds1);
            }
            catch
            {

            }
            finally
            {
                commandSCR.Dispose();
            }
            //TransFeeConfigurationTopObj.IsEnableSaveButton = false;
            //TransFeeConfigurationTopObj.IsEnableEditButton = false;
            ClassSubConfiguration.Clear();
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                var obj = new ClassSubConfigurationModel();
                obj.Id = (int)dr["ID"];
                obj.Sclass = (string)dr["Class"];
                obj.SubId = (int)dr["SubID"];
                obj.Subject = (string)dr["SubName"];
                obj.Term = (int)dr["Terms"];
                obj.Mm = (int)dr["MM"];
                obj.Mm1 = (int)dr["MMD1"];
                obj.Mm2 = (int)dr["MMD2"];
                ClassSubConfiguration.Add(obj);
            }
            ClassSubConfigurationTopObj.Reset();
        }
        public void OnSaveExecuted(object parameter)
        {
            if (ClassSubConfigurationTopObj.Sclass == "" || ClassSubConfigurationTopObj.SubId == null || ClassSubConfigurationTopObj.Mm.ToString() == "" || (ClassSubConfigurationTopObj.Mm1 + ClassSubConfigurationTopObj.Mm2) != ClassSubConfigurationTopObj.Mm)
            {
                System.Windows.Forms.MessageBox.Show(string.Format("Please fill all fields"));
                return;
            }
            int Id;
            if (ClassSubConfiguration.Any(p => p.Sclass.ToUpper() == ClassSubConfigurationTopObj.Sclass.ToUpper() && p.SubId == ClassSubConfigurationTopObj.SubId))
            {
                System.Windows.Forms.MessageBox.Show(string.Format("Data already Exist "));
            }
            else 
            {
                
                
                SqlCommand cmd = new SqlCommand("Usp_InsertClassSubRel", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SClass", ((object)ClassSubConfigurationTopObj.Sclass ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@SubId", ((object)ClassSubConfigurationTopObj.SubId ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@Term", ((object)ClassSubConfigurationTopObj.Term ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@Mm", ((object)ClassSubConfigurationTopObj.Mm ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@Mm1", ((object)ClassSubConfigurationTopObj.Mm1 ?? DBNull.Value));
                cmd.Parameters.AddWithValue("@Mm2", ((object)ClassSubConfigurationTopObj.Mm2 ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int));
                cmd.Parameters["@Id"].Direction = ParameterDirection.Output;
                try
                {
                    con.Open();
                    cmd.ExecuteScalar();
                    Id = (int)cmd.Parameters["@Id"].Value;
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
                if (Id == null)
                {
                    System.Windows.Forms.MessageBox.Show("Data not saved");
                }
                else
                {
                    ds1.Reset();
                    System.Windows.Forms.MessageBox.Show(string.Format("Id Generated: {0} ", Id));
                    SqlCommand command = new SqlCommand("Usp_ClassSubRel", con);
                    command.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter();
                    try
                    {
                        da.SelectCommand = command;
                        da.Fill(ds1);
                    }
                    catch
                    {

                    }
                    finally
                    {
                        command.Dispose();
                    }
                    ClassSubConfiguration.Clear();
                    foreach (DataRow dr in ds1.Tables[0].Rows)
                    {
                        var obj = new ClassSubConfigurationModel();
                        obj.Id = (int)dr["ID"];
                        obj.Sclass = (string)dr["Class"];
                        obj.SubId = (int)dr["SubID"];
                        obj.Subject = (string)dr["SubName"];
                        obj.Term = (int)dr["Terms"];
                        obj.Mm = (int)dr["MM"];
                        obj.Mm1 = (int)dr["MMD1"];
                        obj.Mm2 = (int)dr["MMD2"];
                        ClassSubConfiguration.Add(obj);
                    }
                    ClassSubConfigurationTopObj.Reset();
                }
            }
        }

    }
}
