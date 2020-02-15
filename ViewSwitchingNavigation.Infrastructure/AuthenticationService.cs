using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Management;
using System.Windows;

namespace ViewSwitchingNavigation.Infrastructure
{
    public class AuthenticationService : IAuthenticationService {

        public  AuthenticationContext _authenticationContext = new AuthenticationContext();
        public static string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
        SqlConnection con = new SqlConnection(connectionString);
        DataSet ds = new DataSet();
        public AuthenticationContext Authenticate(string userName, string password,int FinancialYear) {
            Authorize(userName, password,FinancialYear);
            if (_authenticationContext.IsAuthorized) {
                GetApprovedModules();
            }
            return _authenticationContext;
        }

        private void Authorize(string userName, string password,int FinancialYear) {
            string SerialNumber = string.Empty;
            ManagementObjectSearcher MOS = new ManagementObjectSearcher("Select * From Win32_BaseBoard");
            foreach (ManagementObject getserial in MOS.Get())
            {
                SerialNumber = getserial["SerialNumber"].ToString();
            }
            var strings = new List<string> { ".FH524Y1.CN7620635D01IV.", "M80-3C009900859", "C" };
            string text=null; 
            bool exist = File.Exists(@"C:\Program Files\Common Files\P2A\io.txt");
            if(exist)
                text = System.IO.File.ReadAllText(@"C:\Program Files\Common Files\P2A\io.txt");
            //bool contains = strings.Contains(SerialNumber);
            //bool contains = true;
            if (text == "x945ehdu92ay")
            {
                if ((userName.Equals("admin") && password.Equals("pass") && FinancialYear != 0) || userName.Equals("mscc") && password.Equals("msccpass") && FinancialYear != 0)
                    {
                    _authenticationContext.FinancialYear = FinancialYear;
                    _authenticationContext.IsAuthorized = true;
                    _authenticationContext.AuthLevel = Enumerations.AuthorizationLevel.Admin;

                    SqlCommand command = new SqlCommand("Usp_ClassFeeRelation", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FinancialYear", ((object)FinancialYear ?? DBNull.Value));
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
                    int TableNumber=ds.Tables.Count;
                    if(TableNumber==0)
                    {
                        MessageBox.Show("DB not Running. 1. Type services.msc in run dialog. 2.Look for SQL SERVER(MSSQLSERVER) 3.Double click to open it. 4. Set StartUp type to 'Automatic'. 5. Click Start button in the dialog box. 6. Restart application again ");
                    }
                    else if (ds.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("विन्यास(Configuration) नहीं किया , विन्यास फाइल सेट करें");
                    }
                    //Start-Added for Payment check by Anand Date: 23sep'15
                    SqlCommand cmd = new SqlCommand();
                    Object returnValue = 0;
                    try
                    {
                        cmd.CommandText = "SELECT 1 FROM SFDetails WHERE renewdate>=CAST(GETDATE() AS DATE)";
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        con.Open();
                        returnValue = cmd.ExecuteScalar();
                        con.Close();
                    }
                    catch
                    {

                    }
                    finally
                    {
                        cmd.Dispose();
                    }
                    if (returnValue == null)
                    {
                        _authenticationContext.FinancialYear = FinancialYear;
                        _authenticationContext.IsAuthorized = false;
                        MessageBox.Show("कृपया शेष राशि का भुगतान करें ! संपर्क करें: 09711488665 या मेल करें: anandguptastar@gmail.com. संदर्भ: स्कूल सॉफ्टवेयर");
                    }
                    //End
                }
                else if (userName.Equals("readonly") && password.Equals("pass") && FinancialYear != 0)
                {
                    _authenticationContext.FinancialYear = FinancialYear;
                    _authenticationContext.IsAuthorized = true;
                    _authenticationContext.AuthLevel = Enumerations.AuthorizationLevel.ReadOnly;
                }
            }
            else
                MessageBox.Show("संपर्क करें: 09711488665 या मेल करें: anandguptastar@gmail.com. संदर्भ: स्कूल सॉफ्टवेयर");
        }

        private void GetApprovedModules() {
            IList<string> modules = new List<string>();
            switch (_authenticationContext.AuthLevel) {
                case Enumerations.AuthorizationLevel.Admin:
                    modules.Add(EmployeeConstants.EmployeeEditModuleName);
                    modules.Add(EmployeeConstants.EmployeeCreateModuleName);
                    break;
                case Enumerations.AuthorizationLevel.ReadWrite:
                    modules.Add(EmployeeConstants.EmployeeCreateModuleName);
                    break;
            }
            _authenticationContext.AllowedModules = modules;
        }

    }
}
