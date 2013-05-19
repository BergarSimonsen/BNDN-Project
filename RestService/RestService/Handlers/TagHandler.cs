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
    class TagHandler : AbstractHandler<Tag>
    {
        public TagHandler (DatabaseConnection incDbCon, Permissions perm) : base(incDbCon, perm) { }

        public override void Create(Dictionary<string, string> data)
        {
            Validate(data);

            PreparedStatement stat = dbCon.Prepare("INSERT INTO tag (name, tag_group_id, simple_name) VALUES ('" + 
            data["name"] + "','" + 
            data["tag_group_id"] + "', '" + 
            data["simple_name"] + "')");
            
            dbCon.Command(data, stat);
        }

        public override Tag[] Read(int id)
        {
            PreparedStatement stat = dbCon.Prepare("SELECT * FROM tag where id = '" + id + "'");

            return ListToArray(CreateTag(dbCon.Query(new Dictionary<string, string>(), stat)));
        }

        public override void Update(int id, Dictionary<string, string> data)
        {
            Validate(data);

            PreparedStatement stat = dbCon.Prepare("UPDATE tag (name, simple_name) VALUES ('" + 
            data["name"] + "','" + 
            data["tag_group_id"] + "', '" +
            data["simple_name"] + "') where id=" + id);

            dbCon.Command(data, stat);
        }

        public override void Delete(int id)
        {
            PreparedStatement stat = dbCon.Prepare("DELETE FROM tag where id = '" + id + "'");

            dbCon.Command(new Dictionary<string, string>(), stat);
        }

        public override Tag[] Search(Dictionary<string, string> data)
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

            PreparedStatement stat = dbCon.Prepare("SELECT * FROM tag" + searchParams);

            return ListToArray(CreateTag(dbCon.Query(data, stat)));
        }

        
        public override void Validate(Dictionary<string, string> data)
        {
            if (!data.ContainsKey("name"))
                throw new Exception("Tag is missing 'name' data");
            if (!data.ContainsKey("tag_group_id"))
                throw new Exception("Tag is missing 'name' data");
            if (!data.ContainsKey("simple_name"))
                throw new Exception("Tag is missing 'simple_name' data");
        }

        private List<Tag> CreateTag(SqlDataReader reader)
        {
            List<Tag> returnTags = new List<Tag>();

            while (reader.Read())
            { 
                int id = reader.GetInt32(reader.GetOrdinal("id"));
                int tag_group_id = reader.GetInt32(reader.GetOrdinal("tag_group_id"));
                string name = reader.GetString(reader.GetOrdinal("name"));
                string simple_name= reader.GetString(reader.GetOrdinal("simple_name"));

                returnTags.Add(new Tag(id, tag_group_id, name, simple_name));
            }

            return returnTags;
        }

        private Tag[] ListToArray(List<Tag> incList)
        {
            return incList.ToArray();
        }
    }
}
