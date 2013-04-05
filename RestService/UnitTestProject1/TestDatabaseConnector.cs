using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestService;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;

namespace UnitTestProject1
{
    [TestClass]
    public class TestDatabaseConnector
    {

        DatabaseConnector dbCon = DatabaseConnector.GetInstance;

        [TestMethod]
        public void TestPostMedia()
        {
            string query = "INSERT INTO media (type, file_location, title, description, minutes, format, media_category, user_account) "+
            "VALUES('test','test','test','test', 0,'test', 0, 0)";

            dbCon.ExecuteQuery(query, "SMU");
            dbCon.ExecuteQuery(query, "SMU");

            query = "INSERT INTO media (type, file_location, title, description, minutes, format, media_category, user_account) " +
            "VALUES('test','alsoTest','alsoTest','alsoTest', 0,'alsoTest', 0, 0)";

            dbCon.ExecuteQuery(query, "SMU");

            SqlDataReader reader = dbCon.ExecuteReader("SELECT title FROM media WHERE type = 'test'", "SMU");

            List<string> result = new List<string>();
            while (reader.Read())
            {
                result.Add(reader.GetString(reader.GetOrdinal("title")));
            }

            Assert.AreEqual(2, result.Count);
        }
        
        [TestMethod]
        public void TestGetMedia()
        {

        }

        [TestMethod]
        public void TestPutMedia()
        {

        }

        [TestMethod]
        public void TestDeleteMedia()
        {

        }
    }
}
