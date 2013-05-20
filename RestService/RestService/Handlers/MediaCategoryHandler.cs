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
    class MediaCategoryHandler : AbstractHandler<MediaCategory>
    {
        public MediaCategoryHandler(DatabaseConnection incDbCon, Permissions perm) : base(incDbCon, perm) { }

        public override void Create(Dictionary<string, string> data)
        {
            Validate(data);

            PreparedStatement stat = dbCon.Prepare("INSERT INTO media_category (name, description) VALUES ('" + 
            data["name"] + "', '" + 
            data["description"] + "')");
            
            dbCon.Command(data, stat);
        }

        public override MediaCategory[] Read(int id)
        {
            PreparedStatement stat = dbCon.Prepare("SELECT * FROM media_category where id = '" + id + "'");

            return ListToArray(CreateMediaCategory(dbCon.Query(new Dictionary<string,string>(), stat)));
        }

        public override void Update(int id, Dictionary<string, string> data)
        {
            Validate(data);

            PreparedStatement stat = dbCon.Prepare("UPDATE media_category (name, description)" +
            " VALUES('"
            + data["name"] + "', '"
            + data["description"] + "') where id=" + id);

            dbCon.Command(data, stat);
        }

        public override void Delete(int id)
        {
            PreparedStatement stat = dbCon.Prepare("DELETE FROM media_category where id = '"+id+"'");

            dbCon.Command(new Dictionary<string, string>(), stat);
        }

        public override MediaCategory[] Search(Dictionary<string, string> data) 
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

            PreparedStatement stat = dbCon.Prepare("SELECT * FROM media_category"+ searchParams);

            return ListToArray(CreateMediaCategory(dbCon.Query(data, stat)));
        }
        
        public override void Validate(Dictionary<string, string> data)
        {
            if (!data.ContainsKey("name"))
                throw new Exception("MediaCategory is missing 'name' data");
            if (!data.ContainsKey("description"))
                throw new Exception("MediaCategory is missing 'description' data");
        }

        private List<MediaCategory> CreateMediaCategory(SqlDataReader reader)
        { 
            List<MediaCategory> returnMediaCategories = new List<MediaCategory>();

            while (reader.Read())
            { 
                int id = reader.GetInt32(reader.GetOrdinal("id"));
                string name = reader.GetString(reader.GetOrdinal("name"));
                string description= reader.GetString(reader.GetOrdinal("description"));

                //TODO userdata has to be fetched witht he rast of the data
                returnMediaCategories.Add(new MediaCategory(id, name, description));
            }

            return returnMediaCategories;
        }

        private MediaCategory[] ListToArray(List<MediaCategory> incList)
        {
            return incList.ToArray();
        }
         
    }
}
