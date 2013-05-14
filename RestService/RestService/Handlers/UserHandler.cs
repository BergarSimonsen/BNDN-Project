using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using RestService.Security;

namespace RestService
{
    public class UserHandler : AbstractHandler
    {
        public UserHandler(DatabaseConnection incDbCon, Permissions perm) : base(incDbCon, perm) { }

        public override void Create(Dictionary<string, string> data)
        {
            Validate(data);

            PreparedStatement stat = dbCon.Prepare("INSERT INTO user_account (email, password_hash, created, modified) " +
            "VALUES (@email, @password, @created, @modified)", new List<string> { "email", "password", "created", "modified" });
            dbCon.Command(data, stat);
        }

        public override SqlDataReader Read(int id)
        {
            PreparedStatement stat = dbCon.Prepare("SELECT * FROM user_account where id = '" + id + "'", 
            new List<string> { });

            return dbCon.Query(new Dictionary<string,string>(), stat);
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

        public override SqlDataReader Search(Dictionary<string, string> data) 
        {
            Validate(data);

            string insertTo = "";
            string valueString = "";
            List<string> list = new List<string>();

            foreach (KeyValuePair<string,string> s in data)
            {
                list.Add(s.Key);
                insertTo += " " + s.Key + ",";
                valueString += " @" + s.Key + ",";
            }

            insertTo.Remove(valueString.Length - 1);
            valueString.Remove(valueString.Length - 1);

            PreparedStatement stat = dbCon.Prepare("SELECT * FROM user_account (" + insertTo + ") VALUES (" + valueString + ")", list);

            return dbCon.Query(data, stat);
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
         
    }
}