using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using RestService;
using RestService.Entities;

namespace RestServiceTest
{
    [TestClass]
    public class RatingTest
    {
        RestServiceImpl restService = new RestServiceImpl();

        static List<int> userIdList = new List<int>();
        static List<int> mediaIdList = new List<int>();
        static List<int> ratingIdList = new List<int>();

        /// <summary>
        /// Test that the postRating and getRating methods work.
        /// Insert a new user and a new media into the database.
        /// Insert a new rating for the media and user.
        /// Check that only one rating returns and it matches the inserted rating.
        /// </summary>
        [TestMethod]
        public void getAndPostRatingTest()
        {
            bool success = false;

            // Insert test user
            User testUser = new User(0, "testtest@test.com", "testpass", null);
            int userId = restService.insertUser(testUser);
            userIdList.Add(userId);

            // Insert test media
            Media testMedia = new Media(0, 2, userId, "FileLocation", "test title", "some description", 223, "avi");
            int mediaId = restService.postMedia(testMedia);
            mediaIdList.Add(mediaId);

            // Insert new rating
            Rating testRating = new Rating(0, userId, mediaId, 3, "title of test comment", "content of test comment");
            restService.postRating(testRating);

            Rating[] checkRating = restService.getRating(mediaId.ToString(), userId.ToString());

            // Check that there is only one rating and that it matches the inserted rating.
            if (checkRating.Length == 1 && checkRating[0].commentTitle == testRating.commentTitle) {
                success = true;
            }
            Assert.IsTrue(success);
        }

        /// <summary>
        /// Clean up method.
        /// Deletes all entries inserted into the database for testing purposes.
        /// </summary>
        [TestMethod]
        public void cleanUp()
        {
            foreach (int userid in userIdList) {
                restService.deleteUser(userid.ToString());
            }

            foreach (int mediaid in mediaIdList) {
                restService.deleteMedia(mediaid.ToString());
            }
        }
    }
}
