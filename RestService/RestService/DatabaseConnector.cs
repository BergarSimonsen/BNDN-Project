using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace RestService
{
    class DatabaseConnector
    {
        private static DatabaseConnector instance;
        private SqlConnection connection;
        private string connectionString;
        private Dictionary<string, string> databases;

        public static DatabaseConnector GetInstance
        {
            get
            {
                if (instance == null) instance = new DatabaseConnector();
                return instance;
            }
        }

        private DatabaseConnector()
        {
            Initialize();
        }

        private void Initialize()
        { 
            //load info about databases and apps into dictionary here
            databases = new Dictionary<string,string>();
            databases.Add("ITU", "ItuDatabase");
            databases.Add("SMU","SmuDatabase");
        }

        private SqlConnection Connect(string s)
        { 
            connectionString = "Server=rentit.itu.dk;DATABASE="+databases[s]+";UID=Rentit26db;PASSWORD=ZAQ12wsx;";
            connection = new SqlConnection(connectionString);
            try
            {
                if (connection.State != System.Data.ConnectionState.Open) connection.Open();
                Console.WriteLine("works");
                return connection;
            }
            catch(SqlException e)
            {
                Console.WriteLine(e.StackTrace);
                return null;
            }
            
        }

        public bool CloseConnection()
        {
            if (connection.State != System.Data.ConnectionState.Closed) { connection.Close(); return true; }
            else return false;
        }

        private void ExecuteQuery(string query, string database)
        {
            if (connection.State != System.Data.ConnectionState.Open) Connect(database);
            if (connection.State != System.Data.ConnectionState.Open) ErrorMessage("Cannot open connection to server");
            else
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                CloseConnection();
            }
        }

        private SqlDataReader ExecuteReader(string query, string database)
        {
            if (connection.State != System.Data.ConnectionState.Open) Connect(database);
            if (connection.State != System.Data.ConnectionState.Open) ErrorMessage("Cannot open connection to server");
            else
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(query, connection);
                    SqlDataReader reader = cmd.ExecuteReader();
                    //CloseConnection();
                    return reader;
                }
                catch (SqlException e) { ErrorMessage(e.StackTrace); }
            }
            return null;
        }

        private void ErrorMessage(string s)
        {
            Console.Write(s);
        }

//******************************************************** User *******************************************************

        public User getUser(int id)
        {
            Connect("SMU");
            string query = "select * from user_account where id = " + id;
            SqlDataReader reader = ExecuteReader(query, "SmuDatabase");

            User user = null;
            while (reader.Read())
            {
                int userId = reader.GetInt32(reader.GetOrdinal("id"));
                string email = reader.GetString(reader.GetOrdinal("email"));
                string password = reader.GetString(reader.GetOrdinal("password_hash"));

                user = UserHandler.createUser(userId,email,password);
            }

            return user;
        }

        public void NewUser(string email, string password, int[] userData)
        {
            Connect("SMU");
            // created and modified are the same at insertion.
            DateTime created = DateTime.Now;
            // Insert into user_account
            string query = "insert into user_account values('', '"+email+"','"+password+"','"+created+"','"+created+"')";
            ExecuteQuery(query, "SmuDatabase");
            // Get user back from database in order to get the id
            User curUser = getUser(email);
            int curId = curUser.id;
            // Insert into user_account_data
            foreach(int i in userData) {
                string newQuery = "insert into user_account_data values('','"+curId+"','"+i+"','')";
                ExecuteQuery(newQuery, "SmuDatabase");
            }
        }

        public void DeleteUser(int id)
        {
            Connect("SMU");
            // Check if user exists
            if (getUser(id) != null)
            {
                // Delete user from database
                string query = "delete * from user_account where id = " + id;
                ExecuteQuery(query, "SmuDatabase");
            }
            else
            { 
                // User doesn't exist
                Console.WriteLine("User doesn't exist!!!");
            }
            
        public void putUser(string[] info, int id)
        {
            Connect("SMU");
            string query = "";
            if (info[0] != null && info[1] != null && info[2] != null)
                query = "UPDATE user_account SET email = '"+info[0]+"', password_hash = '"+info[2]+"' where id = '"+id+"'";
            else if (info[0] != null && info[1] == null || info[2] == null)
                query = "UPDATE user_account SET email = '" + info[0] + "' where id = '" + id + "'";
            else if (info[0] == null && info[1] != null && info[2] != null)
                query = "UPDATE user_account SET password_hash = '" + info[2] + "' where id = '" + id + "'";

            ExecuteQuery(query, "SMU");
            

        }

        public User getUser(string incmail)
        {
            Connect("SMU");
            string query = "select * from user_account where email = '" + incmail + "';";
            SqlDataReader reader = ExecuteReader(query, "SmuDatabase");

            User user = null;
            while (reader.Read())
            {
                int userId = reader.GetInt32(reader.GetOrdinal("id"));
                string email = reader.GetString(reader.GetOrdinal("email"));
                string password = reader.GetString(reader.GetOrdinal("password_hash"));

                user = UserHandler.createUser(userId, email, password);
            }

            return user;
        }
    }
}
