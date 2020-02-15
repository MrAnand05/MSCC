using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using ViewSwitchingNavigation.Infrastructure;

namespace ViewSwitchingNavigation.Exam.ViewModels
{
    public class MarksDetailsViewModel:BindableBase
    {
        public class ColumnData
        {
            public string Data
            { get; set; }
            public int Width
            { get; set; }
        }
        public class Column
        {
            public ObservableCollection<ColumnData> ColumnsData
            { get; set; }
        }
        public class Table
        {
            public ObservableCollection<Column> Columns
            { get; set; }
            public ObservableCollection<ColumnData> Headers
            { get; set; }

            public Table()
            {
                Columns = new ObservableCollection<Column>();
                Headers = new ObservableCollection<ColumnData>();
            }
        }
        
        public DelegateCommand<object> EditCommand { get; private set; }
        public DelegateCommand<object> SaveCommand { get; private set; }
        public DelegateCommand<object> PrintCommand { get; private set; }

        public bool IsDatafromDB = false;
        public static string connectionString = @"Server= .; Database= SM; Integrated Security=True;";
        SqlConnection con = new SqlConnection(connectionString);
        DataSet ds = new DataSet();
        public MarksDetailsViewModel()
        {
            SqlCommand command = new SqlCommand("Usp_StudentMarks", con);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Session", ((object)AuthenticationContext.GlobalFinancialYear ?? DBNull.Value));
            command.Parameters.AddWithValue("@Class", "000-Nursery");
            command.Parameters.AddWithValue("@Term", 1);
            
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
            
            Table DataContext = new Table();
            foreach (DataColumn dc in ds.Tables[0].Columns)
            {
                DataContext.Columns.Add(new Column()
                {
                    ColumnsData = new ObservableCollection<ColumnData>() 
                    { new ColumnData() 
                        { 
                            Data = dc.Caption, 
                            Width = 100 
                        }
                    }

                });
                DataContext.Headers.Add(new ColumnData()
                {
                    Data = "Column 1 Title",
                    Width = 100
                });
            }
        }

    }
}
