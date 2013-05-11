using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestService;
using RestService.Security;
using RestService.Entities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace RestServiceTest
{
    [TestClass]
    public class PermissionsTest
    {
        DatabaseConnection dbCon;
        Permissions permission;
        UserHandler userHandler;

        public PermissionsTest()
        {
            dbCon = new DatabaseConnection("SMU");
            permission = Permissions.createPermissions(new User(14, null, null, null), dbCon);
            userHandler = new UserHandler(dbCon, permission);
        }


        [TestMethod]
        public void TestCreatePermissions()
        {
            Permissions permissions = Permissions.createPermissions(new User(14, null, null, null),new DatabaseConnection("SMU"));
            Assert.IsTrue(permissions.actions.Length > 0);
            for (int i = 0; i < permissions.actions.Length; i++)
            {
                Console.WriteLine(permissions.actions[i].name);
            }
        }

        [TestMethod]
        public void TestCanDo()
        {
            Permissions permissions = Permissions.createPermissions(new User(14, null, null, null), new DatabaseConnection("SMU"));
            Assert.IsTrue(permissions.canDo(new RestService.Entities.Action(5,"watchMovie",null,true)));
            Assert.IsFalse(permissions.canDo(new RestService.Entities.Action(6, "watchMovie", null, true)));
        }

/**************************************************************** Handler ***************************************************************/

        [TestMethod]
        public void TestPreparedStatement()
        {
            PreparedStatement statement = dbCon.Prepare("SELECT * FROM user_account", new List<string>());

            dbCon.ValidateStatement(statement);
        }

        [TestMethod]
        public void TestSearch()
        {
            Dictionary<string, string> test = new Dictionary<string, string>();
            test.Add("id", "14");
            test.Add("password_hash", "myPass");
            SqlDataReader reader = userHandler.Search(test);

            string result = "";

            while (reader.Read())
            {
                result = reader.GetString(reader.GetOrdinal("email"));
            }

            Assert.AreEqual("Someguy@hotmail.com", result);
        }

        [TestMethod]
        public void TestRead()
        {
            SqlDataReader reader = userHandler.Read(14);
            string result =" ";
            while (reader.Read())
            {
                result = reader.GetString(reader.GetOrdinal("password_hash"));
            }

            Assert.AreEqual("myPass", result);
        }
    }
}
