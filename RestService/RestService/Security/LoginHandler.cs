using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestService.Entities;
using RestService.IO_Messages;
using RestService.Security;
using System.Data.SqlClient;

namespace RestService.Security
{
    public class LoginHandler
    {
        public static Token Login(string email, string password)
        {
            string userPassword = GetUserPassword(email);
            
            if (!password.Equals(userPassword))
            { throw new Exception("User not found or password mismatch"); }

            return new Token(SHAEncrypter.SHAEncrypt(email + password));
        }

        private static string GetUserPassword(string email)
        {
            DatabaseConnection dbConnect = new DatabaseConnection("SMU");

            string query = "SELECT password FROM user_account WHERE email=/'" + email + "/'";
            PreparedStatement prepStat = dbConnect.Prepare(query);

            SqlDataReader data = dbConnect.Query(null, prepStat);
            string userPassword = data.GetString(0);

            data.Close();
            dbConnect.CloseConnection();

            return userPassword;
        }
    }
}