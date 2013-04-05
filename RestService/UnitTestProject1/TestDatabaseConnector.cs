using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestService;

namespace UnitTestProject1
{
    [TestClass]
    public class TestDatabaseConnector
    {

        DatabaseConnector dbCon = DatabaseConnector.GetInstance;

        [TestMethod]
        public void TestPostMedia()
        {
            string query = "INSERT";

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
