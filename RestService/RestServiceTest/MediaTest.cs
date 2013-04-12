using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using RestService;

namespace RestServiceTest
{
    [TestClass]
    public class MediaTest
    {
        RestServiceImpl restService = new RestServiceImpl();
        static List<int> idList = new List<int>();
        static List<int> mediaCatIdList = new List<int>();

        /// <summary>
        /// Test if the insertMedia and getMedia methods work.
        /// Insert a media object into the database. Get the media back using the getMedia
        /// method. Check if the titles of the both media objects match.
        /// </summary>
        [TestMethod]
        public void insertAndGetMediaTest()
        {
            Media testMedia = new Media(0, 2, 18, "someFileLocation", "someTitle", "someDescription", 22, "avi");
            int id = restService.postMedia(testMedia);
            idList.Add(id);
            Media checkMedia = restService.getMedia(id.ToString());
            Assert.AreEqual(testMedia.title, checkMedia.title);
        }

        /// <summary>
        /// Test if the update media method works.
        /// Insert a media, update the inserted media. Get the media back from the database.
        /// Check if the title matches with the edited title.
        /// </summary>
        [TestMethod]
        public void updateMediaTest()
        {
            Media testMedia = new Media(0, 2, 18, "someFileLocation", "titleTest", "desc", 22, "mpg");
            int id = restService.postMedia(testMedia);
            idList.Add(id);
            Media editMedia = new Media(id, 3, 18, "lol", "editedTitle", "editedDesc", 22, "mpg");
            restService.putMedia(editMedia, id.ToString());
            testMedia = restService.getMedia(id.ToString());
            Assert.AreEqual(editMedia.title, testMedia.title);
        }

        /// <summary>
        /// Check if the delete media method works.
        /// Insert a media into the database. Delete the media. Get the media back
        /// from the database. Since it's deleted the method should return null.
        /// </summary>
        [TestMethod]
        public void deleteMediaTest()
        {
            Media testMedia = new Media(0, 2, 18, "fileLocation", "title", "desc", 22, "avi");
            int id = restService.postMedia(testMedia);
            restService.deleteMedia(id.ToString());
            Media checkMedia = restService.getMedia(id.ToString());
            Assert.AreEqual(null, checkMedia);
        }

        /// <summary>
        /// Test that post and get media category methods work.
        /// Insert a new mediaCategory into the database. 
        /// Get mediacategory with the same id.
        /// Check that they are the same.
        /// </summary>
        [TestMethod]
        public void InsertAndGetMediaCategoryTest()
        {
            MediaCategory mediaCat = new MediaCategory(0, "testCategory", "some Description");
            int id = restService.postMediaCategory(mediaCat);
            mediaCatIdList.Add(id);

            MediaCategory checkMediaCat = restService.getMediaCategory(id.ToString());
            Assert.AreEqual(mediaCat.name, checkMediaCat.name);
        }

        /// <summary>
        /// Test that putMediaCategory method works.
        /// Insert a media category into the database.
        /// Edit the inserted media category. Get the edited version back from the database.
        /// Check to make sure that the data has been edited.
        /// </summary>
        [TestMethod]
        public void putMediaCategoryTest()
        {
            MediaCategory mediaCat = new MediaCategory(0, "testCategory", "some Description");
            int id = restService.postMediaCategory(mediaCat);
            mediaCatIdList.Add(id);

            MediaCategory mediaCatEdit = new MediaCategory(id, "testCategoryEdited", "desc Edited");
            restService.putMediaCategory(id.ToString(), mediaCatEdit);

            MediaCategory checkMediaCat = restService.getMediaCategory(id.ToString());
            Assert.AreEqual(mediaCatEdit, mediaCatEdit);
        }

        /// <summary>
        /// Test that the delete media category method works.
        /// Insert a media category into the database. 
        /// Delete the inserted media category.
        /// Get the media category back from the database.
        /// Since it's deleted, the get method should return null.
        /// </summary>
        [TestMethod]
        public void deleteMediaCategoryTest()
        {
            MediaCategory mediaCat = new MediaCategory(0, "testCategory", "some Description");
            int id = restService.postMediaCategory(mediaCat);

            restService.deleteMediaCategory(id.ToString());

            MediaCategory checkMediaCat = restService.getMediaCategory(id.ToString());

            Assert.AreEqual(null, checkMediaCat);
        }

        /// <summary>
        /// Clean up method.
        /// Deletes all media objects that are inserted into the database for
        /// testing purposes.
        /// </summary>
        [TestMethod]
        public void CleanUp()
        {
            foreach (int id in idList) {
                restService.deleteMedia(id.ToString());
            }

            foreach (int mcid in mediaCatIdList) {
                restService.deleteMediaCategory(mcid.ToString());
            }
        }
    }
}
