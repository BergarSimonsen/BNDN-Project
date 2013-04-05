using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace RestService
{
    public class DatabaseConnector
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
            databases.Add("SMU", "SmuDatabase");
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

        /// <summary>
        /// Executes a query with no return value
        /// </summary>
        /// <param name="query">Query to execute</param>
        /// <param name="database">Database to execute the query on</param>
        private void ExecuteQuery(string query, string database)
        {
            Connect(database);
            if (connection.State != System.Data.ConnectionState.Open) Connect(database);
            if (connection.State != System.Data.ConnectionState.Open) ErrorMessage("Cannot open connection to server");
            else
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                CloseConnection();
            }
        }

        /// <summary>
        /// Execute a query with a return value
        /// </summary>
        /// <param name="query">Query to execute</param>
        /// <param name="database">Database to execute query on</param>
        /// <returns>SQLDataReader object with return values</returns>
        private SqlDataReader ExecuteReader(string query, string database)
        {
            Connect(database);
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

        /// <summary>
        /// Gets a user from the database
        /// </summary>
        /// <param name="id">The id of the user</param>
        /// <returns>User object</returns>
        public User getUser(int id)
        {
            string query = "select * from user_account where id = " + id;
            SqlDataReader reader = ExecuteReader(query, "SMU");

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

        /// <summary>
        /// Inserts a new user into the database
        /// </summary>
        /// <param name="email">User's email</param>
        /// <param name="password">User's password</param>
        /// <param name="userData">User's user data</param>
        /// <returns>Id of the user</returns>
        public int PostUser(string email, string password, int[] userData)
        {
            // created and modified are the same at insertion.
            DateTime created = DateTime.Now;
            // Insert into user_account
            string query = "insert into user_account(email, password_hash, created, modified) values('"+email+"','"+password+"','"+created+"','"+created+"')";
            ExecuteQuery(query, "SMU");
            // Get user back from database in order to get the id
            User curUser = getUser(email);
            int curId = curUser.id;
            // Insert into user_account_data
            if (userData != null)
            {
                foreach (int i in userData)
                {
                    string newQuery = "insert into user_account_data values('','" + curId + "','" + i + "','')";
                    ExecuteQuery(newQuery, "SMU");
                }
            }

            return curId;
        }

        /// <summary>
        /// Deletes a user from the database.
        /// </summary>
        /// <param name="id">Id of the user to delete</param>
        public void DeleteUser(int id)
        {
            // Check if user exists
            if (getUser(id) != null)
            {
                string userGroupQuery = "delete from user_account_in_user_group where user_account_id = " + id;
                ExecuteQuery(userGroupQuery, "SMU");
                // Delete user from database
                string userQuery = "delete from user_account where id = " + id;
                ExecuteQuery(userQuery, "SMU");
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

            ExecuteQuery(query, "SMU");
        }

        /// <summary>
        /// Gets a user based on it's email
        /// </summary>
        /// <param name="incmail">Email of the user</param>
        /// <returns>User object</returns>
        public User getUser(string incmail)
        {
            string query = "select * from user_account where email = '" + incmail + "';";
            SqlDataReader reader = ExecuteReader(query, "SMU");

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

        /// <summary>
        /// Gets all users from the database
        /// </summary>
        /// <param name="group_id">Filter by group_id</param>
        /// <param name="search_string">Filter by search_string</param>
        /// <param name="search_fields">Filter by search_fields</param>
        /// <param name="order_by">Which column to order by</param>
        /// <param name="order">How to order?</param>
        /// <returns>Array of users</returns>
        public User[] getUsers(int group_id, string search_string, string search_fields, string order_by, string order, int limit, int page)
        {
            //MS SHIT to do limit and page...
            string query = "GO WITH UserResult AS (SELECT ROW_NUMBER() AS 'RowNumber', ";
            
            if (group_id != 0 && search_string == null && search_fields == null)
            {
                query += "user_account.id, user_account.email, user_account.password_hash from user_account, (select user_account_id from user_account_in_user_group where user_group_id = " + group_id + ") uid where uid.user_account_id = user_account.id order by user_account."+order_by+" "+order;
            }
            else if(group_id != 0 && search_string != null && search_fields != null)
            {
                query += "* from (select user_account.id, user_account.email, user_account.password_hash from user_account, (select user_account_id from user_account_in_user_group where user_group_id = " + group_id + ") uid where uid.user_account_id = user_account.id order by user_account."+order_by+" "+order+") users where "+search_fields+" = '"+search_string+"'";
            }
            else if(group_id == 0 && search_string != null && search_fields != null)
            {
                query += "* from user_account where "+search_fields+" = '"+search_string+"' order by user_account."+order_by+" "+order;
            }
            else
            {
                query += "* from user_account";
            }

            //MORE MS SHIT to do limit and page..
            query += ") SELECT * FROM UserResult WHERE RowNumber BETWEEN "+(page*limit)+" AND "+((page+1)*limit);

            List<User> groupsUsers = null;
            SqlDataReader reader = ExecuteReader(query, "SMU");
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

        public int getUsersCount(int group_id, string search_string, string search_fields) {
            string query = "SELECT COUNT(*) FROM user_account WHERE group_id="+group_id;
            SqlDataReader reader = ExecuteReader(query, "SmuDatabase");
            reader.Read();
            int result = reader.GetInt32(reader.GetValue(0));
            CloseConnection();
            return result;
        }
//*****************************************************************************************************************************************************
//********************************************************** Media ************************************************************************************

        /// <summary>
        /// Gets a media from the database
        /// </summary>
        /// <param name="id">Id of the media to return</param>
        /// <returns>Media object</returns>
        public Media getMedia(int id)
        {
            string query = "SELECT * FROM media WHERE id = '"+id+"'";
            SqlDataReader reader = ExecuteReader(query, "SMU");

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
            reader = ExecuteReader(query, "SMU");

            List<int> returnMediaTags = new List<int>();
            while (reader.Read())
            {
                returnMediaTags.Add(reader.GetInt32(reader.GetOrdinal("tag_id")));
            }

            returnMedia.tags = returnMediaTags.ToArray();

            return returnMedia;
        }

        public MediaList getMedias(int tag, int mediaCategoryFilter, string nameFilter, int page, int limit)
        {
            return null;
        }

        /// <summary>
        /// Inserts a media into the database.
        /// </summary>
        /// <param name="media">Media object to insert</param>
        /// <returns>The id of the media</returns>
        public int postMedia(Media media)
        {
            // TODO INSERT LENGTH MANUALLY ?????????
            // TODO TYPE?????
            int mediaCategory = media.mediaCategory;
            int user = media.user;
            string fileLocation = media.fileLocation;
            string title = media.title;
            string description = media.description;
            int mediaLength = media.mediaLength;
            string format = media.format;
            int[] tags = media.tags;

            string query = "INSERT INTO media VALUES('', '', '', '" + title + "', '" + description + "', '" + mediaLength + "', '" + format + "', '" + mediaCategory + "', '" + user + "')";
            ExecuteQuery(query, "SMU");

            return getMediaIdByDescription(title, description);
        }

        /// <summary>
        /// Private helper method.
        /// Returns the id of a media based on it's title and description
        /// </summary>
        /// <param name="title">Title of the media</param>
        /// <param name="description">Description of the media</param>
        /// <returns>Id of the media</returns>
        private int getMediaIdByDescription(string title, string description)
        {
            int id = -1;
            string query = "SELECT id FROM media WHERE title = '" + title + "' AND description = '" + description + "'";
            SqlDataReader reader = ExecuteReader(query, "SMU");

            while (reader.Read()) {
                id = reader.GetInt32(reader.GetOrdinal("id"));
            }
            return id;
        }


        /// <summary>
        /// Deletes a media from the database.
        /// </summary>
        /// <param name="id">The id of the media to delete</param>
        public void deleteMedia(int id)
        {
            string query = "DELETE FROM rating WHERE media_id = " + id;
            ExecuteQuery(query, "SMU");

            query = "DELETE FROM media_has_tag WHERE media_id = " + id;
            ExecuteQuery(query, "SMU");

            query = "DELETE FROM media WHERE id = "+id;
            ExecuteQuery(query, "SMU");
            // TODO DELETE MEDIA FILE ?!?!?!?!
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
            ExecuteQuery(query, "SMU");

            query = "DELETE FROM media_category WHERE id = '" + id + "'";
            ExecuteQuery(query, "SMU");
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

            SqlDataReader reader = ExecuteReader(query, "SMU");
            while (reader.Read())
            {
                id = reader.GetInt32(reader.GetOrdinal("id"));
            }
            return id;
        }
        
        
//****************************************** TAGS **************************************************************************************
        /// <summary>
        /// Gets all tags from the database
        /// </summary>
        /// <param name="tagGroupFilter">Id of the tag group to filter by</param>
        /// <param name="limit">How many results per page?</param>
        /// <param name="page">Page offset</param>
        /// <returns>Array of all tags</returns>
        public Tag[] getTags(int tagGroupFilter, int limit, int page)
        {
            string query = "";
            List<Tag> tags = new List<Tag>();
            if (tagGroupFilter < 1 && limit < 1 && page < 1)
            {
                query = "SELECT * FROM tag";
            }
            else if (tagGroupFilter > 0 && limit < 1 && page < 1)
            {
                query = "SELECT * FROM tag WHERE tag_group = '" + tagGroupFilter + "'";
            }
            else if (tagGroupFilter > 0  && limit > 0 && page < 1)
            {
                //query = "SELECT * FROM tag WHERE tag_group = '" + tagGroupFilter + "' LIMIT 0, " + limit + "";
                query = "SELECT * FROM (SELECT row_number() OVER (ORDER BY id) AS rownum, tagGroupTags.* FROM (SELECT * FROM tag WHERE tag_group_id = "+tagGroupFilter+") tagGroupTags) chuck WHERE chuck.rownum BETWEEN 0 AND " + limit;
            }
            else if (tagGroupFilter > 0 && limit > 0 && page > 0)
            { 
                int limitStart = limit * page;
                int limitEnd = limitStart + limit;
                query = "SELECT * FROM tag WHERE tag_group = '" + tagGroupFilter + "' LIMIT " + limitStart + "," + limitEnd;
            }
            else if (tagGroupFilter < 1 && limit > 0 && page > 0)
            {
                int limitStart = limit * page;
                int limitEnd = limitStart + limit;
                query = "SELECT * FROM tag LIMIT " + limitStart + "," + limitEnd;
            }
            else if (tagGroupFilter < 1 && limit > 0 && page < 1)
            {
                query = "SELECT * FROM tag LIMIT 0, " + limit;
            }
            SqlDataReader reader = ExecuteReader(query, "SMU");
            while (reader.Read()) {
                int id = reader.GetInt32(reader.GetOrdinal("id"));
                int tagGroup = reader.GetInt32(reader.GetOrdinal("tag_group_id"));
                string name = reader.GetString(reader.GetOrdinal("name"));
                string shortName = reader.GetString(reader.GetOrdinal("simple_name"));
                tags.Add(new Tag(id, tagGroup, name, shortName));
            }
            return tags.ToArray();
        }

        /// <summary>
        /// Gets a tag from the database
        /// </summary>
        /// <param name="id">The id of the tag to retreive</param>
        /// <returns>The tag</returns>
        public Tag getTag(string id)
        {
            Tag tag = null;
            int tagId = int.Parse(id);
            string query = "SELECT * FROM tag WHERE id = '" + tagId + "'";
            SqlDataReader reader = ExecuteReader(query, "SMU");
            while (reader.Read()) { 
                int tID = reader.GetInt32(reader.GetOrdinal("id"));
                int tagGroup = reader.GetInt32(reader.GetOrdinal("tag_group"));
                string name = reader.GetString(reader.GetOrdinal("name"));
                string shortName = reader.GetString(reader.GetOrdinal("simple_name"));
                tag = new Tag(tID, tagGroup, name, shortName);
            }
            return tag;
        }

        /// <summary>
        /// Inserts a new tag into the database
        /// </summary>
        /// <param name="name">The name of the tag</param>
        /// <param name="simpleName">The simple name of the tag</param>
        /// <param name="tagGroups">Tag group that the tag is member of</param>
        /// <returns>Id of the new tag.</returns>
        public int postTag(string name, string simpleName, int tagGroup)
        {
            string query = "INSERT INTO tag (tag_group_id, name, simple_name) VALUES('" + tagGroup + "', '" + name + "', '" + simpleName + "')";
            ExecuteQuery(query, "SMU");
            return getTagByName(name);
        }

        /// <summary>
        /// Private helper method.
        /// Gets a tag by it's name
        /// </summary>
        /// <param name="name">The name of the tag</param>
        /// <returns>The id of the tag</returns>
        private int getTagByName(string name)
        {
            int id = -1;
            string query = "SELECT * FROM tag WHERE name = '" + name + "'";
            SqlDataReader reader = ExecuteReader(query, "SMU");
            while (reader.Read()) {
                id = reader.GetInt32(reader.GetOrdinal("id"));
            }
            return id;
        }

        /// <summary>
        /// Updates a tag
        /// </summary>
        /// <param name="id">The id of the tag to update</param>
        /// <param name="name">The new name of the tag</param>
        /// <param name="simpleName">The new simple name</param>
        /// <param name="tagGroup">The new tag group</param>
        public void putTag(string id, string name, string simpleName, int tagGroup)
        {
            int tagId = int.Parse(id);
            string query = "";
            if(name != "" && simpleName == "" && tagGroup < 1) {
                query = "UPDATE tag SET name = '" + name + "' WHERE id = '" + tagId + "'";
            }
            else if (name != "" && simpleName != "" && tagGroup < 1) {
                query = "UPDATE tag SET name = '" + name + "', simple_name = '" + simpleName + "' WHERE id = '" + tagId + "'";
            }
            else if (name != "" && simpleName != "" && tagGroup > 0) {
                query = "UPDATE tag SET name = '" + name + "', simple_name = '" + simpleName + "', tag_group = '" + tagGroup + "' WHERE id = '" + tagId + "'";
            }
            ExecuteQuery(query, "SMU");
        }

        /// <summary>
        /// Deletes a tag from the database.
        /// </summary>
        /// <param name="id">The id of the tag to delete</param>
        public void deleteTag(string id)
        {
            int tagId = int.Parse(id);
            string query = "DELETE from tag WHERE id = '" + tagId + "'";
            ExecuteQuery(query, "SMU");
        }

        /// <summary>
        /// Gets a tag group from the database.
        /// </summary>
        /// <param name="id">The id of the tag group</param>
        /// <returns>Tag group object</returns>
        public TagGroup getTagGroup(string id)
        {
            TagGroup tagGroup = null;
            int tgId = int.Parse(id);
            string query = "SELECT * FROM tag_group WHERE id = '" + tgId + "'";
            SqlDataReader reader = ExecuteReader(query, "SMU");
            while (reader.Read()) {
                int tId = reader.GetInt32(reader.GetOrdinal("id"));
                string name = reader.GetString(reader.GetOrdinal("name"));
                tagGroup = new TagGroup(tId, name);
            }
            return tagGroup;
        }

        /// <summary>
        /// Returns all tag groups from the database.
        /// </summary>
        /// <param name="limit">Number of tag groups per page.</param>
        /// <param name="page">Page offset</param>
        /// <returns>Array of tagGroups</returns>
        public TagGroup[] getTagGroups(string limit, string page)
        {
            int nLimit = int.Parse(limit);
            int nPage = int.Parse(page);
            List<TagGroup> tagGroups = new List<TagGroup>();
            int limitStart = nPage * nLimit;
            int limitEnd = limitStart + nLimit;
            string query = "";
            if (nLimit > 0 && nPage < 1) {
                query = "SELECT * FROM tag_group LIMIT 0, " + nLimit;
            }
            else if (nLimit > 0 && nPage > 0)
            {
                query = "SELECT * FROM tag_group LIMIT " + limitStart + ", " + limitEnd;
            }
            else {
                query = "SELECT * FROM tag_group";
            }
            SqlDataReader reader = ExecuteReader(query, "SMU");
            while (reader.Read()) {
                int tgId = reader.GetInt32(reader.GetOrdinal("id"));
                string name = reader.GetString(reader.GetOrdinal("name"));
                tagGroups.Add(new TagGroup(tgId, name));
            }
            return tagGroups.ToArray();
        }

        /// <summary>
        /// Create a new tag group and save it in the database.
        /// </summary>
        /// <param name="name">The name of the tag group</param>
        /// <returns>The id of the newly created tag group</returns>
        public int postTagGroup(string name)
        {
            string query = "INSERT INTO tag_group (name) VALUES('" + name + "')";
            ExecuteQuery(query, "SMU");
            return getTagGroupByName(name);
        }

        /// <summary>
        /// Updates a tag group
        /// </summary>
        /// <param name="id">The id of the tag group to update</param>
        /// <param name="name">The new name of the tag group</param>
        public void putTagGroup(string id, string name)
        {
            int tId = int.Parse(id);
            string query = "UPDATE tag_group SET name = '" + name + "' WHERE id = '" + tId + "'";
            ExecuteQuery(query, "SMU");
        }

        /// <summary>
        /// Deletes a tag group from the database
        /// </summary>
        /// <param name="id">The id of the tag group to delete</param>
        public void deleteTagGroup(string id)
        {
            int tId = int.Parse(id);
            string query = "DELETE FROM tag_group WHERE id = '" + tId + "'";
            ExecuteQuery(query, "SMU");
        }

        /// <summary>
        /// Private helper method.
        /// Returns the id of a tag group based on it's name
        /// </summary>
        /// <param name="name">The name of the tag group</param>
        /// <returns>The id of the tag group</returns>
        private int getTagGroupByName(string name)
        {
            int id = -1;
            string query = "SELECT id FROM tag_group WHERE name = '" + name +"'";
            SqlDataReader reader = ExecuteReader(query, "SMU");
            while (reader.Read()) {
                id = reader.GetInt32(reader.GetOrdinal("id"));
            }
            return id;
        }

        // ============================================ Rating ======================================= //

        /// <summary>
        /// Gets the rating for a media
        /// </summary>
        /// <param name="media">The id of the media</param>
        /// <returns>Rating object</returns>
        public Rating getRating(string media)
        {
            Rating ratingObj = null;
            int mediaId = int.Parse(media);
            string query = "SELECT * FROM rating WHERE media_id = '" + mediaId + "'";
            SqlDataReader reader = ExecuteReader(query, "SMU");
            while (reader.Read()) { 
                int id = reader.GetInt32(reader.GetOrdinal("id"));
                int userAccount = reader.GetInt32(reader.GetOrdinal("user_account"));
                mediaId = reader.GetInt32(reader.GetOrdinal("media_id"));
                short rating = (short) reader.GetInt32(reader.GetOrdinal("rating"));
                string comment = reader.GetString(reader.GetOrdinal("comment"));
                string commentTitle = reader.GetString(reader.GetOrdinal("comment_title"));
                ratingObj = new Rating(id, userAccount, mediaId, rating, commentTitle, comment);
            }
            return ratingObj;
        }

        /// <summary>
        /// Posts a new rating for a media
        /// </summary>
        /// <param name="userId">Id of the user who posted</param>
        /// <param name="mediaId">Id of the media the rating belongs to</param>
        /// <param name="stars">Amount of stars to give</param>
        /// <param name="commentTitle">Title of the comment</param>
        /// <param name="comment">Content of the comment</param>
        public void postRating(int userId, int mediaId, int stars, string commentTitle, string comment)
        {
            string query = "INSERT INTO rating VALUES('', '" + userId + "', '" + mediaId + "', '" + stars + "', '" + comment + "', '" + commentTitle + "')";
            ExecuteQuery(query, "SMU");
        }

        /// <summary>
        /// Updates an already existing comment
        /// </summary>
        /// <param name="id">The id of the comment to edit</param>
        /// <param name="commentTitle">The new title of the comment</param>
        /// <param name="comment">The new content of the comment</param>
        /// <param name="stars">The new amount of stars</param>
        public void putRating(int id,string commentTitle,string comment,int stars)
        {
            string query = "UPDATE rating SET comment_title = '" + commentTitle + "', comment = '" + comment + "', rating = '" + stars + "' WHERE id = '" + id + "'";
            ExecuteQuery(query, "SMU");
        }
    }
}
