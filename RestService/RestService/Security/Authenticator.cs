using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using RestService.IO_Messages;
using System.Data.SqlClient;

namespace RestService.Security
{
    public class Authenticator
    {
        /**
         * The method will return false if it at any point is unable to verify
         * the hash. This can be due to the hash not being present in the request,
         * or the requestHash not matching the verificationHash.
         */
        public static bool VerifyHash(Request request)
        {
            string requestHash = request.authorization;

            /**
             * Declaring verificationString here as part of creating the correct
             * string using an unknown amount of data.
             */
            string verificationString = "";
            foreach (KeyValuePair<string, string> dataPair in request.data)
            { verificationString += dataPair.Value; }

            string hashKey = "";
            if (request.data.TryGetValue("secret", out hashKey))
            { verificationString += GetSecretKey(hashKey); }
            else
            { throw new Exception("Hashkey not found. Cannot look up secretKey"); }

            string verificationHash = SHAEncrypter.SHAEncrypt(verificationString);

            if (verificationHash.Equals(requestHash)) { return true; }
            return false;             
        }

        private static string GetSecretKey(string clientKey)
        {
            DatabaseConnection dbConnect = new DatabaseConnection("SMU");
            string query = "SELECT * FROM secret_key WHERE clientKey=/'" + clientKey + "/'";
            
            PreparedStatement prepStat = dbConnect.Prepare(query);
            SqlDataReader data = dbConnect.Query(null, prepStat);

            string secretKey;
            if (data.Read())
            {   secretKey = data.GetString(1); }
            else
            {
                data.Close();
                dbConnect.CloseConnection(); 
                throw new Exception("No such clientKey exists");
            }

            data.Close();
            dbConnect.CloseConnection();

            return secretKey;
        }
    }
}