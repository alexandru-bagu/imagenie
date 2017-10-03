using System;
using System.Configuration;
using System.Data.SqlClient;

namespace imaGenie
{
    public class DatabaseManager
    {
        public static string ConnectionString
        {
            get
            {
                foreach (ConnectionStringSettings connectionString in ConfigurationManager.ConnectionStrings)
                    if (connectionString.Name == "DefaultConnection")
                        return connectionString.ConnectionString;
                throw new Exception("");
            }
        }
        public static SqlConnection Connection
        {
            get
            {
                SqlConnection connection = new SqlConnection(ConnectionString);
                connection.Open();
                return connection;
            }
        }
    }
}