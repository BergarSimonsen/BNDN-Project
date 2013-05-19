using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using RestService.Security;
using RestService.Entities;

namespace RestService.Handlers
{
    class UserGroupHandler : AbstractHandler<UserGroup>
    {
        public UserGroupHandler(DatabaseConnection incDbCon, Permissions perm) : base(incDbCon, perm) { }

        public override void Create(Dictionary<string, string> data)
        {
            Validate(data);

            PreparedStatement stat = dbCon.Prepare("INSERT INTO user_group (name, description) VALUES ('" + data["name"] + "', '" + data["description"] + "')");
            
            dbCon.Command(data, stat);
        }

        public override UserGroup[] Read(int id)
        {
            PreparedStatement stat = dbCon.Prepare("SELECT * FROM user_group where id = '" + id + "'");

            return ListToArray(CreateUserGroup(dbCon.Query(new Dictionary<string, string>(), stat)));
        }

        public override void Update(int id, Dictionary<string, string> data)
        {
            Validate(data);

            PreparedStatement stat = dbCon.Prepare("UPDATE user_group (name, description)" +
            "VALUES ('" + data["name"] + "', '" + data["description"] + "')");
            dbCon.Command(data, stat);
        }

        public override void Delete(int id)
        {
            PreparedStatement stat = dbCon.Prepare("DELETE FROM user_group where id = '"+id+"'");

            dbCon.Command(new Dictionary<string, string>(), stat);
        }

        public override UserGroup[] Search(Dictionary<string, string> data)
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

            PreparedStatement stat = dbCon.Prepare("SELECT * FROM user_group" + searchParams);

            return ListToArray(CreateUserGroup(dbCon.Query(data, stat)));
        }

        
        public override void Validate(Dictionary<string, string> data)
        {
            if (!data.ContainsKey("name"))
                throw new Exception("UserGroup is missing 'name' data");
            if (!data.ContainsKey("description"))
                throw new Exception("UserGroup is missing 'description' data");
        }

        private List<UserGroup> CreateUserGroup(SqlDataReader reader)
        {
            List<UserGroup> returnUserGroups = new List<UserGroup>();

            while (reader.Read())
            { 
                int id = reader.GetInt32(reader.GetOrdinal("id"));
                string name = reader.GetString(reader.GetOrdinal("name"));
                string description= reader.GetString(reader.GetOrdinal("description"));

                returnUserGroups.Add(new UserGroup(id, name, description));
            }

            return returnUserGroups;
        }

        private UserGroup[] ListToArray(List<UserGroup> incList)
        {
            return incList.ToArray();
        }
    }
}
