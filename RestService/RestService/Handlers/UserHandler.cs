using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using RestService.Security;
using RestService.Entities;

namespace RestService
{
    public class UserHandler : AbstractHandler
    {
        public UserHandler(DatabaseConnection incDbCon, Permissions perm) : base(incDbCon, perm) { }

        /*public User createUser(int id, string email, string password, int[] userData)
        {
            
            PreparedStatement s = dbCon.Prepare("INSERT INTO user (name, email, password) VALUES (@name, @email, @password)");
            dbCon.Command(new Dictionary<string,string>{
                {"email", email},
                {"password", password}
            }, s);

            return new User(id,email,password, userData);
        }*/

        public override void Create(Dictionary<string, string> data)
        {
            Validate(data);

            PreparedStatement stat = dbCon.Prepare("INSERT INTO user_account (email, password_hash, created, modified) " +
            "VALUES (@email, @password, @created, @modified)", new List<string> { "email", "password", "created", "modified" });
            dbCon.Command(data, stat);
        }

        public override List<IEntities> Read(int id)
        {
            PreparedStatement stat = dbCon.Prepare("SELECT * FROM user_account where id = '" + id + "'", 
            new List<string> { });

            return CreateUser(dbCon.Query(new Dictionary<string,string>(), stat));
        }

        public override void Update(int id, Dictionary<string, string> data)
        {
            Validate(data);

            PreparedStatement stat = dbCon.Prepare("UPDATE user_account (email, password_hash, created, modified)" +
            "VALUES (@email, @password, @created, @modified)", new List<string> { "email", "password", "created", "modified"});
            dbCon.Command(data, stat);
        }

        public override void Delete(int id)
        {
            PreparedStatement stat = dbCon.Prepare("DELETE FROM user_account where id = '"+id+"'",new List<string>());

            dbCon.Command(new Dictionary<string, string>(), stat);
        }

        public override List<IEntities> Search(Dictionary<string, string> data) 
        {
            Validate(data);

            string searchParams = "";
            
            List<string> list = new List<string>();

            foreach (KeyValuePair<string,string> s in data)
            {
                string semiResult = s.Key+" = '"+s.Value+"' and ";
                searchParams += semiResult;
            }

            // removes the last "and" since there are no more params to search for
            searchParams.Remove(searchParams.Length - 4);

            PreparedStatement stat = dbCon.Prepare("SELECT * FROM user_account where " + searchParams, list);

            return CreateUser(dbCon.Query(data, stat));
        }

        
        public override void Validate(Dictionary<string, string> data)
        {
            if (!data.ContainsKey("email"))
                throw new Exception("User is missing 'email' data");
            if (!data.ContainsKey("password"))
                throw new Exception("User is missing 'password' data");
            if (!data.ContainsKey("created"))
                throw new Exception("User is missing 'created' data");
            if (!data.ContainsKey("modified"))
                throw new Exception("User is missing 'modified' data");
        }

        private List<User> CreateUser(SqlDataReader reader)
        { 
            List<User> returnUsers = new List<User>();

            while (reader.Read())
            { 
                int id = reader.GetInt32(reader.GetOrdinal("id"));
                string email = reader.GetString(reader.GetOrdinal("email"));
                string password = reader.GetString(reader.GetOrdinal("password_hash"));

                //TODO userdata has to be fetched witht he rast of the data
                returnUsers.Add(new User(id, email, password, null));
            }

            return returnUsers;
        }
         
    }
}