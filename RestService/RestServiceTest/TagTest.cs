using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestService;

namespace RestServiceTest
{
    [TestClass]
    public class TagTest
    {
        static List<int> idList = new List<int>();
        static List<int> tagGroupIdList = new List<int>();
        RestServiceImpl restService = new RestServiceImpl();

        /// <summary>
        /// Test that the post and get tag methods work.
        /// Post a new tag to the database. 
        /// Get the same tag back.
        /// Check that they are the same.
        /// </summary>
        [TestMethod]
        public void postAndGetTagTest()
        {
            Tag testTag = new Tag(0, 1, "tagName", "tName");
            int id = restService.postTag(testTag);
            idList.Add(id);
            Tag checkTag = restService.getTag(id.ToString());
            Assert.AreEqual(testTag.name, checkTag.name);
        }

        /// <summary>
        /// Test that the putTag method works.
        /// Insert a new tag into the database.
        /// Edit the tag.
        /// Get the same tag back after edit.
        /// Check that they are the same as the edit tag object.
        /// </summary>
        [TestMethod]
        public void putTagTest()
        {
            Tag testTag = new Tag(0, 1, "tagName", "tName");
            int id = restService.postTag(testTag);
            idList.Add(id);
            Tag editTag = new Tag(id, 1, "editName", "eName");
            restService.putTag(id.ToString(), editTag);
            testTag = restService.getTag(id.ToString());
            Assert.AreEqual(editTag.name, testTag.name);
        }
        
        /// <summary>
        /// Test that the delete tag method works.
        /// Insert a tag to the database.
        /// Delete the tag.
        /// Get the tag back using the getTag method.
        /// Since it's deleted it should return null.
        /// </summary>
        [TestMethod]
        public void deleteTagTest()
        {
            Tag testTag = new Tag(0, 1, "tagName", "tName");
            int id = restService.postTag(testTag);
            restService.deleteTag(id.ToString());
            Tag tag = restService.getTag(id.ToString());
            Assert.AreEqual(null, tag);
        }

        /// <summary>
        /// Test that the post and get tagGroup methods work.
        /// Post a new tag group to the database.
        /// Get the tag group back using the get method.
        /// Check that the tag group from the getTagGroup method is the same as the
        /// posted tag group object.
        /// </summary>
        [TestMethod]
        public void PostAndGetTagGroupTest()
        {
            TagGroup testTG = new TagGroup(0, "Tag Group Name");
            int id = restService.postTagGroup(testTG);
            tagGroupIdList.Add(id);
            TagGroup checkTag = restService.getTagGroup(id.ToString());
            Assert.AreEqual(testTG.name, checkTag.name);
        }

        /// <summary>
        /// Test that the putTagGroup method works.
        /// Insert a tagGroup into the database.
        /// Edit the inserted tagGroup.
        /// Get the edited tagGroup back from the database.
        /// Check that the edited tagGroup from the database is the same as the inserted
        /// edited tagGroup object.
        /// </summary>
        [TestMethod]
        public void putTagGroupTest()
        {
            TagGroup testTG = new TagGroup(0, "Tag Group Name");
            int id = restService.postTagGroup(testTG);
            tagGroupIdList.Add(id);
            TagGroup editTG = new TagGroup(id, "edited Name");
            restService.putTagGroup(id.ToString(), editTG);
            TagGroup checkTagGroup = restService.getTagGroup(id.ToString());
            Assert.AreEqual(editTG.name, checkTagGroup.name);
        }

        /// <summary>
        /// Test that the delete TagGroup works.
        /// Insert a tagGroup to the database.
        /// Delete the tagGroup.
        /// Get the tagGroup back from the database.
        /// Since it's deleted, it should return null.
        /// </summary>
        [TestMethod]
        public void DeleteTagGroupTest()
        {
            TagGroup testTG = new TagGroup(0, "Tag Group Name");
            int id = restService.postTagGroup(testTG);
            restService.deleteTagGroup(id.ToString());
            TagGroup tagGroup = restService.getTagGroup(id.ToString());
            Assert.AreEqual(null, tagGroup);
        }

        /// <summary>
        /// Clean up method.
        /// Deletes all entries from the database that are inserted for testing purposes.
        /// </summary>
        [TestMethod]
        public void CleanUp()
        {
            foreach (int id in idList) {
                restService.deleteTag(id.ToString());
            }

            foreach (int tgId in tagGroupIdList) {
                restService.deleteTagGroup(tgId.ToString());
            }
        }
    }
}
