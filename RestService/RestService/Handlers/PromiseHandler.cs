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
    class PromiseHandler : AbstractHandler<Promise>
    {
        public PromiseHandler(DatabaseConnection incDbCon, Permissions perm) : base(incDbCon, perm) { }

        public override void Create(Dictionary<string, string> data)
        {
            Validate(data);

            PreparedStatement stat = dbCon.Prepare("INSERT INTO promise (action_id, start, hours, single_order_line_id) VALUES ('" + 
            data["action_id"] + "', '" + 
            data["start"] + "', '" +
            data["hours"] + "', '" + 
            data["single_order_line_id"] + "')");
            
            dbCon.Command(data, stat);
        }

        public override Promise[] Read(int id)
        {
            PreparedStatement stat = dbCon.Prepare("SELECT * FROM promise where id = '" + id + "'");

            return ListToArray(CreatePromise(dbCon.Query(new Dictionary<string,string>(), stat)));
        }

        public override void Update(int id, Dictionary<string, string> data)
        {
            Validate(data);

            PreparedStatement stat = dbCon.Prepare("UPDATE promise (action_id, start, hours, single_order_line_id)" +
            " VALUES('"
            + data["action_id"] + "', '"
            + data["start"] + "', '"
            + data["hours"] + "', '"
            + data["action_id, start, hours, single_order_line_id"] + "') where id=" + id);

            dbCon.Command(data, stat);
        }

        public override void Delete(int id)
        {
            PreparedStatement stat = dbCon.Prepare("DELETE FROM promise where id = '" + id + "'");

            dbCon.Command(new Dictionary<string, string>(), stat);
        }

        public override Promise[] Search(Dictionary<string, string> data) 
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

            PreparedStatement stat = dbCon.Prepare("SELECT * FROM promise" + searchParams);

            return ListToArray(CreatePromise(dbCon.Query(data, stat)));
        }
        
        public override void Validate(Dictionary<string, string> data)
        {
            if (!data.ContainsKey("action_id"))
                throw new Exception("Promise is missing 'name' data");
            if (!data.ContainsKey("start"))
                throw new Exception("Promise is missing 'start' data");
            if (!data.ContainsKey("hours"))
                throw new Exception("Promise is missing 'hours' data");
            if (!data.ContainsKey("single_order_line_id"))
                throw new Exception("Promise is missing 'single_order_line_id' data");
        }

        private List<Promise> CreatePromise(SqlDataReader reader)
        { 
            List<Promise> returnMediaCategories = new List<Promise>();

            while (reader.Read())
            { 
                int id = reader.GetInt32(reader.GetOrdinal("id"));
                int actionId = reader.GetInt32(reader.GetOrdinal("action_id"));
                DateTime start = reader.GetDateTime(reader.GetOrdinal("start"));
                int hours= reader.GetInt32(reader.GetOrdinal("description"));
                int SOLid = reader.GetInt32(reader.GetOrdinal("single_order_line_id"));

                //TODO userdata has to be fetched witht he rast of the data
                returnMediaCategories.Add(new Promise(id, actionId, start, hours, SOLid));
            }

            return returnMediaCategories;
        }

        private Promise[] ListToArray(List<Promise> incList)
        {
            return incList.ToArray();
        }
    }
}
