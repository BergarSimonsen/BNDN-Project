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
    public class UserHandler : AbstractHandler<User>
    {
        public UserHandler(DatabaseConnection incDbCon, Permissions perm) : base(incDbCon, perm) { }

        public override void Create(Dictionary<string, string> data)
        {
            Validate(data);

            DateTime created = DateTime.Now;

            data.Add("created", created.Year + "-" + created.Month + "-" + created.Day + " " + created.Hour + ":" + created.Minute + ":" + created.Second);
            data.Add("modified", created.Year + "-" + created.Month + "-" + created.Day + " " + created.Hour + ":" + created.Minute + ":" + created.Second);

            PreparedStatement stat = dbCon.Prepare("INSERT INTO user_account (email, password_hash, created, modified) " +
            "VALUES ('" + data["email"] + "', '" + data["password"] + "', '" + data["created"] + "', '" + data["modified"] + "')");
            dbCon.Command(data, stat);
        }

        public override User[] Read(int id)
        {
            PreparedStatement stat = dbCon.Prepare("SELECT * FROM user_account where id = '" + id + "'");

            return ListToArray(CreateUser(dbCon.Query(new Dictionary<string,string>(), stat)));
        }

        public override void Update(int id, Dictionary<string, string> data)
        {
            Validate(data);

            DateTime now = DateTime.Now;

            data.Add("modified", now.Year + "-" + now.Month + "-" + now.Day + " " + now.Hour + ":" + now.Minute + ":" + now.Second);

            PreparedStatement stat = dbCon.Prepare("UPDATE user_account SET email = '"+data["email"]+"', password_hash = '"+data["password"]+"', modified = '"+data["modified"]+"') " +
            "WHERE id = '"+id+"'");
            dbCon.Command(data, stat);
        }

        public override void Delete(int id)
        {
            PreparedStatement stat = dbCon.Prepare("DELETE FROM user_account where id = '"+id+"'");

            dbCon.Command(new Dictionary<string, string>(), stat);
        }

        public override User[] Search(Dictionary<string, string> data) 
        {
            string searchParams = "";

            if (data.Count != 0)
            {
                searchParams += " where ";

                foreach (KeyValuePair<string, string> s in data)
                {
                    string semiResult = s.Key + " = '" + s.Value + "' and ";
                    searchParams += semiResult;
                }

                // removes the last "and" since there are no more params to search for
                searchParams = searchParams.Remove(searchParams.Length - 4);
            }

            PreparedStatement stat = dbCon.Prepare("SELECT * FROM user_account"+ searchParams);

            return ListToArray(CreateUser(dbCon.Query(data, stat)));
        }
        
        public override void Validate(Dictionary<string, string> data)
        {
            if (!data.ContainsKey("email"))
                throw new Exception("User is missing 'email' data");
            if (!data.ContainsKey("password"))
                throw new Exception("User is missing 'password' data");
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

        private User[] ListToArray(List<User> incList)
        {
            return incList.ToArray();
        }
         
    }
}