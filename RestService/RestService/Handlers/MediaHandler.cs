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
    class MediaHandler : AbstractHandler
    {

        public MediaHandler(DatabaseConnection incDbCon, Permissions perm) : base(incDbCon, perm){ }
        
        /*public static Media createMedia(int id, int mediaCategory, int user, string fileLocation, string title, string description, int mediaLength, string format)
        {
            return new Media(id, mediaCategory, user, fileLocation, title, description, mediaLength, format);
        }*/

        public override void Create(Dictionary<string, string> data)
        {
            Validate(data);

            PreparedStatement stat = dbCon.Prepare("INSERT INTO media (type, file_location, title, description, minutes, format, media_category_id, user_account_id)" +
            " VALUES(@type, @file_location, @title , @description, @minutes, @format, @media_category_id, @user_account_id)"
            , new List<string> {"@type", "@file_location", "@title", "@description", "@minutes", "@format", "@media_category_id", "@user_account_id"});
            dbCon.Command(data, stat);
        }

        public override SqlDataReader Read(int id)
        {
            PreparedStatement stat = dbCon.Prepare("SELECT * FROM media where id = '" + id + "'",
            new List<string> { });

            return dbCon.Query(new Dictionary<string, string>(), stat);
        }

        public override void Update(int id, Dictionary<string, string> data)
        {
            Validate(data);

            PreparedStatement stat = dbCon.Prepare("UPDATE media (type, file_location, title, description, minutes, format, media_category_id, user_account_id)" +
            " VALUES(@type, @file_location, @title , @description, @minutes, @format, @media_category_id, @user_account_id)"
            , new List<string> { "@type", "@file_location", "@title", "@description", "@minutes", "@format", "@media_category_id", "@user_account_id" });
            dbCon.Command(data, stat);
        }

        public override void Delete(int id)
        {
            PreparedStatement stat = dbCon.Prepare("DELETE FROM media where id = '" + id + "'", new List<string>());

            dbCon.Command(new Dictionary<string, string>(), stat);
        }

        public override SqlDataReader Search(Dictionary<string, string> data)
        {
            Validate(data);

            string searchParams = "";

            List<string> list = new List<string>();

            foreach (KeyValuePair<string, string> s in data)
            {
                string semiResult = s.Key + " = '" + s.Value + "' and ";
                searchParams += semiResult;
            }

            // removes the last "and" since there are no more params to search for
            searchParams.Remove(searchParams.Length - 4);

            PreparedStatement stat = dbCon.Prepare("SELECT * FROM media where " + searchParams, list);

            return dbCon.Query(data, stat);
        }


        public override void Validate(Dictionary<string, string> data)
        {
            if (!data.ContainsKey("type"))
                throw new Exception("Media is missing 'type' data");
            if (!data.ContainsKey("file_location"))
                throw new Exception("Media is missing 'file location' data");
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

        private Media createMedia(SqlDataReader reader)
        {



            return null;
        }
    }
}
