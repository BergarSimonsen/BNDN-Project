using System;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestService;
using RestService.Entities;
using RestService.IO_Messages;
using RestService.Security;

namespace RestServiceTest
{
    [TestClass]
    public class TokenHandlerTest
    {
        [TestMethod]
        public void TestTokenHandler()
        {
            DatabaseConnection db = new DatabaseConnection("SMU");
            PreparedStatement stmt = db.Prepare("SELECT * FROM user_account WHERE id = 14");

            SqlDataReader reader = db.Query(null, stmt); ;

            User user = null;
            while (reader.Read())
            {
                int id = reader.GetInt32(reader.GetOrdinal("id"));
                string userEmail = reader.GetString(reader.GetOrdinal("email"));
                string userPassword = reader.GetString(reader.GetOrdinal("password_hash"));

                //TODO userdata has to be fetched witht he rast of the data
                user = new User(id, userEmail, userPassword, null);
            }

            Token token = TokenHandler.getToken(user.email, user.password);

            Console.WriteLine(token.token);

            Request preRequest = new Request(null, 0, null, null, token.token);

            Request postRequest = TokenHandler.validateTokenAndGetUser(preRequest);

            Assert.IsTrue(user.id == postRequest.user.id);
        }
    }
}
