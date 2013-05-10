using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using RestService;
using RestService.Controllers;
using RestService.Entities;
using RestService.IO_Messages;
using RestService.Security;
using RestService.Web_Service;

namespace RestServiceTest
{
    [TestClass]
    public class UserTest
    {
        RestServiceImpl restService = new RestServiceImpl();
        static List<int> idList = new List<int>();

        /// <summary>
        /// Test that the getUser method works.
        /// Insert a user into the database using the insertUser method, and try to
        /// get the same user back from the database. 
        /// Check if the id's are identical.
        /// </summary>
        [TestMethod]
        public void getUserTest()
        {
            User user = new User(0, "test@testmail.com", "testpassword", null);
            int userId = restService.insertUser(user);
            idList.Add(userId);

            User checkUser = restService.getUser(userId.ToString());
            Assert.AreEqual(userId, checkUser.id);
        }

        /// <summary>
        /// Check if the deleteUser method works.
        /// Insert a user into the database. Delete the user, and then try to get the user back 
        /// from the database. The returning user should be null.
        /// </summary>
        [TestMethod]
        public void deleteUserTest()
        {
            User sampleUser = new User(0, "delete@thisUser.com", "samplePassword", null);
            int id = restService.insertUser(sampleUser);

            restService.deleteUser(id.ToString());

            User checkUser = restService.getUser(id.ToString());

            Assert.AreEqual(null, checkUser);
        }

        /// <summary>
        /// Test that the update user method works.
        /// Insert a user into the database. Edit the inserted user.
        /// Retreive the edited user and check that it matches with the edited user object. 
        /// </summary>
        [TestMethod]
        public void putUserTest()
        {
            User editUser = new User(0, "some@email.com", "pass123", null);
            int id = restService.insertUser(editUser);
            idList.Add(id);
            User checkUser = new User(id, "email@edited.com", "lolpass", null);

            restService.updateUser(id.ToString(), "pass123", checkUser);

            User finalUser = restService.getUser(id.ToString());

            Assert.AreEqual(checkUser.email, finalUser.email);
        }

        /// <summary>
        /// Clean up method.
        /// Deletes all users inserted into the database for testing purposes.
        /// </summary>
        [TestMethod]
        public void cleanUp()
        {
            foreach (int id in idList) {
                restService.deleteUser(id.ToString());
            }
            Assert.AreEqual(1, 1);
        }
    }
}
