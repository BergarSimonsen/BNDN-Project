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
    class ActionHandler : AbstractHandler<RestService.Entities.Action>
    {
        public ActionHandler(DatabaseConnection incDbCon, Permissions perm) : base(incDbCon, perm) { }

        public override void Create(Dictionary<string, string> data)
        {
            Validate(data);

            PreparedStatement stat = dbCon.Prepare("INSERT INTO action (name, description) VALUES (@name, @description)"
            , new List<string> {"name", "description"});
            
            dbCon.Command(data, stat);
        }

        public override RestService.Entities.Action[] Read(int id)
        {
            PreparedStatement stat = dbCon.Prepare("SELECT * FROM action where id = '" + id + "'", 
            new List<string> { });

            return ListToArray(CreateAction(dbCon.Query(new Dictionary<string, string>(), stat)));
        }

        public override void Update(int id, Dictionary<string, string> data)
        {
            Validate(data);

            PreparedStatement stat = dbCon.Prepare("UPDATE action (name, description)" +
            "VALUES (@name, @description)", new List<string> { "name", "description"});
            dbCon.Command(data, stat);
        }

        public override void Delete(int id)
        {
            PreparedStatement stat = dbCon.Prepare("DELETE FROM action where id = '"+id+"'",new List<string>());

            dbCon.Command(new Dictionary<string, string>(), stat);
        }

        public override RestService.Entities.Action[] Search(Dictionary<string, string> data) 
        {
            string searchParams = "";
            
            List<string> list = new List<string>();

            foreach (KeyValuePair<string,string> s in data)
            {
                string semiResult = s.Key+" = '"+s.Value+"' and ";
                searchParams += semiResult;
            }

            // removes the last "and" since there are no more params to search for
            searchParams.Remove(searchParams.Length - 4);

            PreparedStatement stat = dbCon.Prepare("SELECT * FROM action where " + searchParams, list);

            return ListToArray(CreateAction(dbCon.Query(data, stat)));
        }

        
        public override void Validate(Dictionary<string, string> data)
        {
            if (!data.ContainsKey("name"))
                throw new Exception("Action is missing 'name' data");
            if (!data.ContainsKey("description"))
                throw new Exception("Action is missing 'description' data");
        }

        private List<RestService.Entities.Action> CreateAction(SqlDataReader reader)
        {
            List<RestService.Entities.Action> returnActions = new List<RestService.Entities.Action>();

            while (reader.Read())
            { 
                int id = reader.GetInt32(reader.GetOrdinal("id"));
                string name = reader.GetString(reader.GetOrdinal("name"));
                string description= reader.GetString(reader.GetOrdinal("description"));
                bool allowed= reader.GetBoolean(reader.GetOrdinal("allowed"));

                returnActions.Add(new RestService.Entities.Action(id, name, description, allowed));
            }

            return returnActions;
        }

        private RestService.Entities.Action[] ListToArray(List<RestService.Entities.Action> incList)
        {
            return incList.ToArray();
        }
    }
}
