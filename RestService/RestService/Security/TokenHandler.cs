using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestService.Entities;
using RestService.IO_Messages;
using RestService.Security;
using System.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace RestService.Security
{
    public static class TokenHandler
    {
        private const string initVector = "tu89geji340t89u2";

        private const int keysize = 256;

        private const string key = "SuperKey1337";

        public static Token getToken(string email, string password)
        {
            User user = GetUser(email, password);

            if (user == null)
            { 
                throw new Exception("User not found or password mismatch");
            }

            return new Token(EncryptToken(email + ":::" + password + ":::" + DateTime.Now.AddMinutes(30).ToString()));
        }

        public static Request validateTokenAndGetUser(Request request)
        {
            string emailAndPassword = DecryptToken(request.authorization);

            string [] emailAndPasswordArray = emailAndPassword.Split(new string[] { ":::" }, StringSplitOptions.None);

            string email = emailAndPasswordArray[0];
            string password = emailAndPasswordArray[1];
            DateTime expires;
            try
            {
                expires = DateTime.Parse(emailAndPasswordArray[2]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception("Token is invalid");
            }

            if (DateTime.Now.CompareTo(expires) > 0)
            {
                Exception ex = new Exception("The token is expired");
                ex.Data.Add("errorCode", 201);

                throw ex;
            }
            else
            {
                User user = GetUser(email, password);

                if (user == null)
                {
                    throw new Exception("Token is invalid");
                }
                else
                {
                    request.user = user;
                    return request;
                }
            }
        }

        private static User GetUser(string email, string password)
        {
            DatabaseConnection dbConnect = new DatabaseConnection("SMU");

            string query = @"SELECT * FROM user_account WHERE email='" + email + "' AND password_hash='" + password + "'";
            PreparedStatement prepStat = dbConnect.Prepare(query);

            SqlDataReader reader = dbConnect.Query(null, prepStat);

            User user = null;
            while (reader.Read())
            {
                int id = reader.GetInt32(reader.GetOrdinal("id"));
                string userEmail = reader.GetString(reader.GetOrdinal("email"));
                string userPassword = reader.GetString(reader.GetOrdinal("password_hash"));

                //TODO userdata has to be fetched witht he rast of the data
                user = new User(id, userEmail, userPassword, null);
            }

            reader.Close();
            dbConnect.CloseConnection();

            return user;
        }

        private static string EncryptToken(string str)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(str);
            PasswordDeriveBytes password = new PasswordDeriveBytes(key, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);    
        }

        private static string DecryptToken(string token)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] cipherTextBytes = Convert.FromBase64String(token);
            PasswordDeriveBytes password = new PasswordDeriveBytes(key, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            }
        }
}