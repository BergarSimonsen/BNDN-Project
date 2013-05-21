using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using RestService.Entities;
using RestService.Security;

namespace RestService.HelperObjects
{
    public static class MediaFilesHandler
    {
        public static void insertMediaFile(DatabaseConnection db, Stream file, int id, Permissions per)
        {
            PreparedStatement stmt1 = db.Prepare("SELECT * FROM media WHERE id = " + id.ToString());
            SqlDataReader reader = db.Query(null, stmt1);
            Media mediaMeta = null;
            while (reader.Read())
            {
                int mediaId = reader.GetInt32(reader.GetOrdinal("id"));
                int mediaCategory = reader.GetInt32(reader.GetOrdinal("media_category_id"));
                int user = reader.GetInt32(reader.GetOrdinal("user_account_id"));
                string mediaFileLocation = reader.GetString(reader.GetOrdinal("file_location"));
                string title = reader.GetString(reader.GetOrdinal("title"));
                string description = reader.GetString(reader.GetOrdinal("description"));
                int mediaLength = reader.GetInt32(reader.GetOrdinal("minutes"));
                string format = reader.GetString(reader.GetOrdinal("format"));

                mediaMeta = new Media(mediaId, mediaCategory, user, mediaFileLocation, title, description, mediaLength, format);
            }
            reader.Close();

            string fileLocation = @"C:\RentItServices\Rentit26\MediaFiles\" + mediaMeta.id.ToString();
            System.IO.Directory.CreateDirectory(fileLocation);
            string fileDir = fileLocation + @"\" + mediaMeta.title + "." + mediaMeta.format;

            FileStream writer = new FileStream(fileDir, FileMode.Create, FileAccess.Write);

            byte[] bytes = new Byte[4096];

            int bytesRead = 0;

            while ((bytesRead = file.Read(bytes, 0, bytes.Length)) != 0)
            {
                writer.Write(bytes, 0, bytesRead);
            }

            file.Close();
            writer.Close();

            string fileStream = "http://rentit.itu.dk/RentIt26/MediaFiles/" + mediaMeta.id.ToString() + "/" + mediaMeta.title + "." + mediaMeta.format;

            PreparedStatement stmt2 = db.Prepare("UPDATE media SET file_location = '"+fileStream+"' WHERE id = "+mediaMeta.id.ToString());

            db.Command(null, stmt2);
        }

        public static void deleteMediaFile()
        {

        }
    }
}