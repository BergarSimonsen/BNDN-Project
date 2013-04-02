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
        public void selectTest()
        {
            Connect("SMU");
            string query = "insert into user_account_data_type (name) values('kaboom')";
            ExecuteQuery(query, "SmuDatabase");
            CloseConnection();
        }

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

        public User[] getUsers(int group_id, string search_string, string search_fields, string order_by, string order)
        {
            Connect("SMU");

            string query = null;
            if (group_id != 0 && search_string == null && search_fields == null)
            {
                query = "select user_account.id, user_account.email, user_account.password_hash from user_account, (select user_account_id from user_account_in_user_group where user_group_id = " + group_id + ") uid where uid.user_account_id = user_account.id order by user_account."+order_by+" "+order;
            }
            else if(group_id != 0 && search_string != null && search_fields != null)
            {
                query = "select * from (select user_account.id, user_account.email, user_account.password_hash from user_account, (select user_account_id from user_account_in_user_group where user_group_id = " + group_id + ") uid where uid.user_account_id = user_account.id order by user_account."+order_by+" "+order+") users where "+search_fields+" = '"+search_string+"'";
            }
            else if(group_id == 0 && search_string != null && search_fields != null)
            {
                query = "select * from user_account where "+search_fields+" = '"+search_string+"' order by user_account."+order_by+" "+order;
            }
            else
            {
                query = "select * from user_account";
            }

            List<User> groupsUsers = null;
            SqlDataReader reader = ExecuteReader(query, "SmuDatabase");
            groupsUsers = new List<User>();
            while (reader.Read())
            {
                int userId = reader.GetInt32(reader.GetOrdinal("id"));
                string email = reader.GetString(reader.GetOrdinal("email"));
                string password = reader.GetString(reader.GetOrdinal("password_hash"));

                groupsUsers.Add(UserHandler.createUser(userId, email, password));
            }

            return groupsUsers.ToArray();
        }
    }
}
