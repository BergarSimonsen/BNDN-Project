using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using RestService.Security;
using RestService.Entities;
using RestService.Handlers;

namespace RestService
{
    class MediaHandler : AbstractHandler<Media>
    {

        public MediaHandler(DatabaseConnection incDbCon, Permissions perm) : base(incDbCon, perm){ }

        public override void Create(Dictionary<string, string> data)
        {
            Validate(data);

            PreparedStatement stat = dbCon.Prepare("INSERT INTO media (type, title, description, minutes, format, media_category_id, user_account_id)" +
            " VALUES('" 
            + data["type"] + "', '" 
            + data["title"] + "', '" 
            + data["description"] + "', '" 
            + data["minutes"] + "', '"
            + data["format"] + "',  '"
            + data["media_category_id"] + "',  '" 
            + data["user_account_id"] + "')");

            dbCon.Command(data, stat);
        }

        public override Media[] Read(int id)
        {
            PreparedStatement stat = dbCon.Prepare("SELECT * FROM media where id = '" + id + "'");

            return ListToArray(CreateMedia(dbCon.Query(new Dictionary<string, string>(), stat)));
        }

        public override void Update(int id, Dictionary<string, string> data)
        {
            Validate(data);

            PreparedStatement stat = dbCon.Prepare("UPDATE media (type, title, description, minutes, media_category_id, user_account_id)" +
            " VALUES('"
            + data["type"] + "', '"
            + data["title"] + "', '"
            + data["description"] + "', '"
            + data["minutes"] + "', '"
            + data["media_category_id"] + "',  '"
            + data["user_account_id"] + "')");

            dbCon.Command(data, stat);
        }

        public override void Delete(int id)
        {
            PreparedStatement stat = dbCon.Prepare("DELETE FROM media where id = '" + id + "'");

            dbCon.Command(new Dictionary<string, string>(), stat);
        }

        public override Media[] Search(Dictionary<string, string> data)
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

            PreparedStatement stat = dbCon.Prepare("SELECT * FROM media" + searchParams);

            Media[] tempArray = ListToArray(CreateMedia(dbCon.Query(data, stat)));

            foreach (Media m in tempArray)
            {
                m.fileLocation = "";
            }

            return tempArray;
        }


        public override void Validate(Dictionary<string, string> data)
        {
            if (!data.ContainsKey("type"))
                throw new Exception("Media is missing 'type' data");
            /*if (!data.ContainsKey("file_location"))
                throw new Exception("Media is missing 'file location' data");*/
            if (!data.ContainsKey("title"))
                throw new Exception("Media is missing 'title' data");
            if (!data.ContainsKey("description"))
                throw new Exception("Media is missing 'description' data");
            if (!data.ContainsKey("minutes"))
                throw new Exception("Media is missing 'length (minutes)' data");
            if (!data.ContainsKey("format"))
                throw new Exception("Media is missing 'format' data");
            if (!data.ContainsKey("description"))
                throw new Exception("Media is missing 'media category' data");
            if (!data.ContainsKey("description"))
                throw new Exception("Media is missing 'user' data");
        }

        private List<Media> CreateMedia(SqlDataReader reader)
        {

            List<Media> returnMedia = new List<Media>(); ;

            while (reader.Read())
            { 
                int id = reader.GetInt32(reader.GetOrdinal("id"));
                int mediaCategory = reader.GetInt32(reader.GetOrdinal("media_category_id"));
                int user = reader.GetInt32(reader.GetOrdinal("user_account_id"));
                string fileLocation = reader.GetString(reader.GetOrdinal("file_location"));
                string title = reader.GetString(reader.GetOrdinal("title"));
                string description = reader.GetString(reader.GetOrdinal("description"));
                int mediaLength = reader.GetInt32(reader.GetOrdinal("minutes"));
                string format = reader.GetString(reader.GetOrdinal("format"));

                returnMedia.Add(new Media(id, mediaCategory, user, fileLocation, title, description, mediaLength, format));
            }

            return returnMedia;
        }

        private Media[] ListToArray(List<Media> incList)
        {
            return incList.ToArray();
        }
    }
}
