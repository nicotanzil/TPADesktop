using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data; 
using System.Data.SqlClient; 

namespace TPA_Desktop_NT20_2.Models.SQL
{
    public class DbManager
    {
        static SqlConnection connection = null; 

        private DbManager() { }

        public static SqlConnection GetInstance()
        {
            if(DbManager.connection == null)
            {
                string attachDbFilename = @"'D:\Nico\1. Bluejack\3. TPA\2. Desktop\TPA Desktop NT20-2\TPA Desktop NT20-2\KongBuBank.mdf'";
                DbManager.connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + attachDbFilename + ";Integrated Security=True"); 

            }
            return DbManager.connection; 
        }

        public static DataTable Get(string query)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = GetInstance())
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();

                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader rd = cmd.ExecuteReader();
                dt.Load(rd);
                connection.Close(); 
            }
            return dt; 
        }
    }
}
