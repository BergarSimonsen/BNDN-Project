using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using RestService.Security;

namespace RestService
{
    public class UserHandler : AbstractHandler
    {
        DatabaseConnection dbCon;
        Permissions permission;

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

        public void Create(Dictionary<string, string> data)
        {
            Validate(data);

            PreparedStatement stat = dbCon.Prepare("INSERT INTO user_account (email, password_hash, created, modified) " +
            "VALUES (@email, @password, @created, @modified)", new List<string> { "email", "password", "created", "modified" });
            dbCon.Command(data, stat);
        }

        public SqlDataReader Read(int id)
        {
            PreparedStatement stat = dbCon.Prepare("SELECT * FROM user_account where id = '" + id + "'", 
            new List<string> { });

            return null;
        }

        public void Update(int id, Dictionary<string, string> data)
        {
            Validate(data);

            PreparedStatement stat = dbCon.Prepare("UPDATE user_account (email, password_hash, created, modified)" +
            "VALUES (@email, @password, @created, @modified)", new List<string> { "email", "password", "created", "modified"});
            dbCon.Command(data, stat);
        }

        public void Delete(int id)
        {
            PreparedStatement stat = dbCon.Prepare("DELETE FROM user_account where id = '"+id+"'",new List<string>());

            dbCon.Command(new Dictionary<string, string>(), stat);
        }

        public void Search(Dictionary<string, string> data);

        public void Validate(Dictionary<string, string> data)
        {
            if (!data.ContainsKey("email"))
                throw new Exception("User is missing 'email' data");
            if (!data.ContainsKey("password"))
                throw new Exception("User is missing 'password' data");
        }
    }
}