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
            connectionString = "Server=rentit.itu.dk;DATABASE=SmuDatabase;UID=Rentit26db;PASSWORD=ZAQ12wsx;";
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
            Connect("SMU");
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
            Connect("SMU");
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
            string query = "select * from user_account where id = " + id;
            SqlDataReader reader = ExecuteReader(query, "SmuDatabase");

            User user = null;
            while (reader.Read())
            {
                int userId = reader.GetInt32(reader.GetOrdinal("id"));
                string email = reader.GetString(reader.GetOrdinal("email"));
                string password = reader.GetString(reader.GetOrdinal("password_hash"));

                user = UserHandler.createUser(userId,email,password, null);
            }
            CloseConnection();

            return user;
        }

        public int PostUser(string email, string password, int[] userData)
        {
            // created and modified are the same at insertion.
            DateTime created = DateTime.Now;
            // Insert into user_account
            string query = "insert into user_account(email, password_hash, created, modified) values('"+email+"','"+password+"','"+created+"','"+created+"')";
            ExecuteQuery(query, "SmuDatabase");
            // Get user back from database in order to get the id
            User curUser = getUser(email);
            int curId = curUser.id;
            // Insert into user_account_data
            if (userData != null)
            {
                foreach (int i in userData)
                {
                    string newQuery = "insert into user_account_data values('','" + curId + "','" + i + "','')";
                    ExecuteQuery(newQuery, "SmuDatabase");
                }
            }

            return curId;
        }

        public void DeleteUser(int id)
        {
            // Check if user exists
            if (getUser(id) != null)
            {
                string userGroupQuery = "delete from user_account_in_user_group where user_account_id = " + id;
                ExecuteQuery(userGroupQuery, "SmuDatabase");
                // Delete user from database
                string userQuery = "delete from user_account where id = " + id;
                ExecuteQuery(userQuery, "SmuDatabase");
            }
            else
            {
                // User doesn't exist
                Console.WriteLine("User doesn't exist!!!");
            }
        }
        
        // NEEDS REVISION skal være som "putMedia"
        public void putUser(int id, User newUser)
        {
            string query = "";
            if (newUser.email != null && newUser.password != null)
                query = "UPDATE user_account SET email = '"+newUser.email+"', password_hash = '"+newUser.password+"' where id = '"+id+"'";
            else if (newUser.email != null && newUser.password == null)
                query = "UPDATE user_account SET email = '" + newUser.email + "' where id = '" + id + "'";
            else if (newUser.email == null && newUser.password != null)
                query = "UPDATE user_account SET password_hash = '" + newUser.password + "' where id = '" + id + "'";

            ExecuteQuery(query, "SmuDatabase");
        }

        public User getUser(string incmail)
        {
            string query = "select * from user_account where email = '" + incmail + "';";
            SqlDataReader reader = ExecuteReader(query, "SmuDatabase");

            User user = null;
            while (reader.Read())
            {
                int userId = reader.GetInt32(reader.GetOrdinal("id"));
                string email = reader.GetString(reader.GetOrdinal("email"));
                string password = reader.GetString(reader.GetOrdinal("password_hash"));

                user = UserHandler.createUser(userId, email, password,null);
            }
            CloseConnection();

            return user;
        }

        public User[] getUsers(int group_id, string search_string, string search_fields, string order_by, string order)
        {
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

                groupsUsers.Add(UserHandler.createUser(userId, email, password, null));
            }
            CloseConnection();

            return groupsUsers.ToArray();
        }
//*****************************************************************************************************************************************************
//********************************************************** Media ************************************************************************************

        public Media getMedia(int id)
        {
            string query = "SELECT * FROM media WHERE id = '"+id+"'";
            SqlDataReader reader = ExecuteReader(query, "SmuDatabase");

            Media returnMedia = null;
            while(reader.Read())
            {
                int mediaId = reader.GetInt32(reader.GetOrdinal("id"));
                int mediaCategory = reader.GetInt32(reader.GetOrdinal("media_category_id"));
                int user = reader.GetInt32(reader.GetOrdinal("user_account_id"));
                string fileLocation = reader.GetString(reader.GetOrdinal("file_location"));
                string title = reader.GetString(reader.GetOrdinal("title"));
                string description = reader.GetString(reader.GetOrdinal("description"));
                int mediaLength = reader.GetInt32(reader.GetOrdinal("minutes"));
                string format = reader.GetString(reader.GetOrdinal("format"));

                returnMedia = MediaHandler.createMedia(mediaId, mediaCategory, user, fileLocation, title, description, mediaLength, format,null);
            }

            query = "SELECT tag_id from media_has_tag where media_id = " + id;
            reader = ExecuteReader(query, "SmuDatabase");

            List<int> returnMediaTags = new List<int>();
            while (reader.Read())
            {
                returnMediaTags.Add(reader.GetInt32(reader.GetOrdinal("tag_id")));
            }

            returnMedia.tags = returnMediaTags.ToArray();

            return returnMedia;
        }

        public void deleteMedia(int id)
        {
            string query = "DELETE * FROM media WHERE id = '"+id+"'";
            ExecuteQuery(query, "SmuDatabase");
        }

        

        /// <summary>
        /// first creates SQL "SET" commands and then creates a full "INSERT" SQL command
        /// </summary>
        /// <param name="table">what table to be edited</param>
        /// <param name="value">what to be edited</param>
        /// <param name="id">identifyer of item to be changed</param>
        public void putMedia(string[] table, string[] value, int id)
        {
            List<string> updates = new List<string>();

            // creates ready-to-use "SET" SQL operations
            for (int i = 0; i < table.Length; i++)
            {
                // this will give: "SET title = 'tempTitle'" (used when this is the last "SET" operation)
                if (i == table.Length - 1) updates.Add(table[i] + " = '" + value[i] + "'");

                // this will give: "SET title = 'tempTitle'," (used when this is NOT the last "SET" operation)
                else updates.Add("SET "+table[i] + " = '" + value[i] + "',");
            }

            string doneUpdate = "";

            // combines all the "SET" operations into one ready-to-use string
            foreach (string s in updates)
            {
                doneUpdate = doneUpdate + s;
            }

            string query = "UPDATE media "+doneUpdate+" WHERE id = '"+id+"'";
            ExecuteQuery(query, "SMU");
        }

        /// <summary>
        /// Inserts a new media category into the database.
        /// </summary>
        /// <param name="name">The name of the new media category.</param>
        /// <returns>Id of the newly inserted media category</returns>
        public int postMediaCategory(MediaCategory mediaCategory)
        {
            string query = "insert into media_category (name,description) values('" + mediaCategory.name + "','"+mediaCategory.description+"')";
            ExecuteQuery(query, "SMU");
            return getMediaCategoryId(mediaCategory.name);
        }

        /// <summary>
        /// Gets a media category from the database based on its id
        /// </summary>
        /// <param name="id">The id of the media category</param>
        /// <returns>A media category object.</returns>
        public MediaCategory getMediaCategory(int id)
        {
            MediaCategory mediaCategory = null;
            string query = "SELECT * FROM media_category WHERE id = '" + id + "'";
            SqlDataReader reader = ExecuteReader(query, "SMU");
            while (reader.Read())
            {
                int mId = reader.GetInt32(reader.GetOrdinal("id"));
                string name = reader.GetString(reader.GetOrdinal("name"));
                string description = reader.GetString(reader.GetOrdinal("description"));
                mediaCategory = new MediaCategory(mId, name,description);
            }
            return mediaCategory;
        }

        /// <summary>
        /// Selects all media categories in the database.
        /// </summary>
        /// <returns>Array of all media categories</returns>
        public MediaCategory[] getMediaCategory()
        {
            List<MediaCategory> mediaCategories = new List<MediaCategory>();
            string query = "SELECT * FROM media_category";
            SqlDataReader reader = ExecuteReader(query, "SMU");
            while (reader.Read())
            {
                int mId = reader.GetInt32(reader.GetOrdinal("id"));
                string name = reader.GetString(reader.GetOrdinal("name"));
                string description = reader.GetString(reader.GetOrdinal("description"));
                mediaCategories.Add(new MediaCategory(mId, name,description));
            }
            return mediaCategories.ToArray();
        }

        /// <summary>
        /// Deletes a media category from the database based on its id.
        /// </summary>
        /// <param name="id">Id of the media category to delete.</param>
        public void deleteMediaCategory(int id)
        {
            string query = "DELETE FROM media WHERE media_category_id = " + id;
            ExecuteQuery(query, "SmuDatabase");

            query = "DELETE FROM media_category WHERE id = '" + id + "'";
            ExecuteQuery(query, "SmuDatabase");
        }

        /// <summary>
        /// Updates a media category.
        /// </summary>
        /// <param name="id">The id of the media category to update.</param>
        /// <param name="name">The new name of the media category</param>
        public void putMediaCategory(int id, string name)
        {
            string query = "UPDATE media_category SET name = '" + name + "' WHERE id = '" + id + "'";
            ExecuteQuery(query, "SMU");
        }

        /// <summary>
        /// Private helper method.
        /// Retreives a media category from the database based on its name.
        /// </summary>
        /// <param name="name">the name of the media category</param>
        /// <returns>The id of the media category. Returns -1 if not found.</returns>
        private int getMediaCategoryId(string name)
        {
            int id = -1;
            string query = "SELECT id FROM media_category WHERE name = '" + name + "'";

            SqlDataReader reader = ExecuteReader(query, "SmuDatabase");
            while (reader.Read())
            {
                id = reader.GetInt32(reader.GetOrdinal("id"));
            }
            return id;
        }
        
        
//*****************************************************************************************************************************************************
    }
}
