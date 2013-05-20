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
    class RatingHandler :AbstractHandler<Rating>
    {
        public RatingHandler(DatabaseConnection incDbCon, Permissions perm) : base(incDbCon, perm) { }

        public override void Create(Dictionary<string, string> data)
        {
            Validate(data);

            PreparedStatement stat = dbCon.Prepare("INSERT INTO rating (user_account_id, media_id, rating, comment, comment_title) VALUES ('" + 
            data["user_account_id"] + "', '" + 
            data["media_id"] + "', '" + 
            data["rating"] + "', '" + 
            data["comment"] + "', '" + 
            data["comment_title"] + "')");
            
            dbCon.Command(data, stat);
        }

        public override Rating[] Read(int id)
        {
            PreparedStatement stat = dbCon.Prepare("SELECT * FROM rating where id = '" + id + "'");

            return ListToArray(CreateRating(dbCon.Query(new Dictionary<string, string>(), stat)));
        }

        public override void Update(int id, Dictionary<string, string> data)
        {
            Validate(data);

            PreparedStatement stat = dbCon.Prepare("UPDATE rating (user_account_id, media_id, rating, comment, comment_title) VALUES ('" +
            data["user_account_id"] + "', '" +
            data["media_id"] + "', '" +
            data["rating"] + "', '" +
            data["comment"] + "', '" +
            data["comment_title"] + "') where id=" + id);

            dbCon.Command(data, stat);
        }

        public override void Delete(int id)
        {
            PreparedStatement stat = dbCon.Prepare("DELETE FROM rating where id = '" + id + "'");

            dbCon.Command(new Dictionary<string, string>(), stat);
        }

        public override Rating[] Search(Dictionary<string, string> data)
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

            PreparedStatement stat = dbCon.Prepare("SELECT * FROM rating" + searchParams);

            return ListToArray(CreateRating(dbCon.Query(data, stat)));
        }

        
        public override void Validate(Dictionary<string, string> data)
        {
            if (!data.ContainsKey("name"))
                throw new Exception("Rating is missing 'name' data");
            if (!data.ContainsKey("description"))
                throw new Exception("Rating is missing 'description' data");
        }

        private List<Rating> CreateRating(SqlDataReader reader)
        {
            List<Rating> returnRatings = new List<Rating>();

            while (reader.Read())
            { 
                int id = reader.GetInt32(reader.GetOrdinal("id"));
                int userAccountId = reader.GetInt32(reader.GetOrdinal("user_account_id"));
                int mediaId = reader.GetInt32(reader.GetOrdinal("id"));
                int rating = reader.GetInt32(reader.GetOrdinal("rating"));
                string comment= reader.GetString(reader.GetOrdinal("comment"));
                string commentTitle = reader.GetString(reader.GetOrdinal("comment_title"));

                returnRatings.Add(new Rating(id, userAccountId, mediaId, (short)rating, comment, commentTitle));
            }

            return returnRatings;
        }

        private Rating[] ListToArray(List<Rating> incList)
        {
            return incList.ToArray();
        }
    }
}
