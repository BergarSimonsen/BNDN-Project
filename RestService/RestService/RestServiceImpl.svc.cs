using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace RestService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RestServiceImpl" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select RestServiceImpl.svc or RestServiceImpl.svc.cs at the Solution Explorer and start debugging.
    public class RestServiceImpl : IRestServiceImpl
    {
        public User getUser(string id)
        {
            DatabaseConnector database = DatabaseConnector.GetInstance;

            return database.getUser(int.Parse(id));
        }

        public UserList getUsersWithParameter(string group_id, string search_string, string search_fields, string limit, string page, string order_by, string order)
        {
            //Sanitize groupID
            int groupId_s = 0;
            if (group_id != null)
            {
                groupId_s = int.Parse(group_id);
            }

            //Sanitize searchString
            string searchString_s = null;
            //TODO: Implement
            /*if (search_string != null)
            {
                searchString_s = search_string;
            }*/

            //Sanitize searchFields
            //TODO: Implement
            /*string searchFields_s = null;
            if (search_fields != null)
            {
                searchFields_s = search_fields;
            }
            */
 
            //Sanitize orderBy
            string orderBy_s = "email";
            if (order_by != null)
            {
                orderBy_s = order_by;
            }

            //Santize Order
            string order_s = "ASC";
            if (order == "DESC")
            {
                order_s = order;
            }

            //Sanitize limit
            int limit_s = 10; //Default
            if (limit != null)
            {
                limit_s = int.Parse(limit);
            }

            //Sanitize page
            int page_s = 0; //Default
            if (page != null)
            {
                page_s = int.Parse(page);
            }

            //Get result
            DatabaseConnector database = DatabaseConnector.GetInstance;

            User[] returnUsers = database.getUsers(groupId_s, searchString_s, searchFields_s, orderBy_s, order_s, limit_s, page_s);
            int userCount = database.getUsersCount(groupId_s, searchString_s, searchFields_s);
            int pageCount = Math.Ceiling(userCount / limit);

            return new UserList(limit, pageCount, returnUsers);
        }

        public int insertUser(User user)
        {
            DatabaseConnector database = DatabaseConnector.GetInstance;

            return database.PostUser(user.email, user.password, user.userData);
        }

        public User getLoggedUser()
        {
            throw new NotImplementedException();
        }

        public Token getToken(string email, string password)
        {
            throw new NotImplementedException();
        }

        public Token renewToken()
        {
            throw new NotImplementedException();
        }

        public void updateUser(string id, string oldPassword, User newUser)
        {
            DatabaseConnector database = DatabaseConnector.GetInstance;

            User varificationUser = database.getUser(int.Parse(id));
            if (varificationUser.password.Equals(oldPassword)) 
            {
                database.putUser(int.Parse(id), newUser);
            }
        }

        public void deleteUser(string id)
        {
            DatabaseConnector database = DatabaseConnector.GetInstance;

            database.DeleteUser(int.Parse(id));
        }

        // ================================ MEDIA & MEDIACATEGORY ========================== //

        /// <summary>
        /// Gets a media from the database.
        /// </summary>
        /// <param name="id">The id of the media to get.</param>
        /// <returns>The media object.</returns>
        public Media getMedia(string id)
        {
            DatabaseConnector database = DatabaseConnector.GetInstance;

            return database.getMedia(int.Parse(id));
        }

        /// <summary>
        /// Gets all medias in the database.
        /// </summary>
        /// <param name="andTags">Filter by matching all tags.</param>
        /// <param name="orTags">Filter by matching some tags</param>
        /// <param name="mediaCategoryFilter">Filter by mediaCategory</param>
        /// <param name="nameFilter">Filter by name</param>
        /// <param name="page">The page you are on.</param>
        /// <param name="limit">How many medias per page?</param>
        /// <returns>List of all medias</returns>
        public MediaList getMedias(string tag, string mediaCategoryFilter, string nameFilter, string page, string limit)
        {
            int mediaTag = 0;
            if (tag != null)
            {
                mediaTag = int.Parse(tag);
            }
            int mediaCategory = 0;
            if (mediaCategoryFilter != null)
            {
                mediaCategory = int.Parse(mediaCategoryFilter);
            }
            string title = null;
            if (nameFilter != null)
            {
                title = nameFilter;
            }
            return null;
        }

        /// <summary>
        /// Inserts a media into the database.
        /// </summary>
        /// <param name="media">Media object to insert</param>
        /// <returns>The id of the media</returns>
        public int postMedia(Media media)
        {
            DatabaseConnector database = DatabaseConnector.GetInstance;
            return database.postMedia(media);
        }

        /// <summary>
        /// Uploads a media file to the server.
        /// </summary>
        /// <param name="file">The file to upload.</param>
        public void insertMediaFile(Stream file)
        {
            FileStream writer = new FileStream(@"C:\Users\christian\Documents\RentItTest\derp.mkv",FileMode.Create,FileAccess.Write);

            byte[] bytes = new Byte[4096];

            int bytesRead = 0;

            while ((bytesRead = file.Read(bytes, 0, bytes.Length)) != 0)
            {
                writer.Write(bytes,0,bytesRead);
            }

            file.Close();
            writer.Close();
        }

        /// <summary>
        /// Updates a media.
        /// </summary>
        /// <param name="table">The table to update</param>
        /// <param name="value">The values to insert</param>
        /// <param name="id">The id of the media to update</param>
        public void putMedia(Media media, string id)
        { 
            DatabaseConnector database = DatabaseConnector.GetInstance;
            //database.putMedia(table, value, int.Parse(id));
        }

        /// <summary>
        /// Deletes a media from the database.
        /// </summary>
        /// <param name="id">The id of the media to delete.</param>
        public void deleteMedia(string id)
        {
            DatabaseConnector database = DatabaseConnector.GetInstance;
            database.deleteMedia(int.Parse(id));
        }

        /// <summary>
        /// Retreives all media categories from the database.
        /// </summary>
        /// <returns>Array of all media categories</returns>
        public MediaCategory[] getMediaCategories()
        {
            DatabaseConnector database = DatabaseConnector.GetInstance;
            return database.getMediaCategory();
        }
        
        /// <summary>
        /// Gets a media category from the database based on its id
        /// </summary>
        /// <param name="id">The id of the media categoryu</param>
        /// <returns>A media category object.</returns>
        public MediaCategory getMediaCategory(string id)
        {
            DatabaseConnector database = DatabaseConnector.GetInstance;
            return database.getMediaCategory(int.Parse(id));
        }

        /// <summary>
        /// Updates a media category.
        /// </summary>
        /// <param name="id">The id of the media category to update.</param>
        /// <param name="name">The new name of the media category</param>
        public void putMediaCategory(string id, MediaCategory mediaCategory)
        {
            DatabaseConnector database = DatabaseConnector.GetInstance;
            database.putMediaCategory(int.Parse(id), mediaCategory.name);
        }

        /// <summary>
        /// Deletes a media category from the database based on its id
        /// </summary>
        /// <param name="id">Id of the media category to delete.</param>
        public void deleteMediaCategory(string id)
        {
            DatabaseConnector database = DatabaseConnector.GetInstance;
            database.deleteMediaCategory(int.Parse(id));
        }

        /// <summary>
        /// Inserts a new media category into the database.
        /// </summary>
        /// <param name="name">The name of the media category.</param>
        /// <returns>The id of the media category.</returns>
        public int postMediaCategory(MediaCategory mediaCategory)
        {
            DatabaseConnector database = DatabaseConnector.GetInstance;
            return database.postMediaCategory(mediaCategory);
        }

        //==================================== TAGS ===============================================
        /// <summary>
        /// Get all tags from the database
        /// </summary>
        /// <param name="tagGroupFilter">Filter by tag group</param>
        /// <param name="limit">Amount of tags per page.</param>
        /// <param name="page">The page number</param>
        /// <returns></returns>
        public Tag[] getTags(int tagGroupFilter, int limit, int page)
        {
            DatabaseConnector database = DatabaseConnector.GetInstance;
            return database.getTags(tagGroupFilter, limit, page);
        }

        /// <summary>
        /// Gets a tag from the database
        /// </summary>
        /// <param name="id">The id of the tag to retreive</param>
        /// <returns>The tag</returns>
        public Tag getTag(string id)
        {
            DatabaseConnector database = DatabaseConnector.GetInstance;
            return database.getTag(id);
        }

        /// <summary>
        /// Inserts a new tag into the database
        /// </summary>
        /// <param name="name">The name of the tag</param>
        /// <param name="simpleName">The simple name of the tag</param>
        /// <param name="tagGroups">Tag group that the tag is member of</param>
        /// <returns>Id of the new tag.</returns>
        public int postTag(Tag tag)
        {
            DatabaseConnector database = DatabaseConnector.GetInstance;
            return database.postTag(tag.name, tag.simple_name, tag.tag_group);
        }

        /// <summary>
        /// Updates a tag
        /// </summary>
        /// <param name="id">The id of the tag to update</param>
        /// <param name="name">The new name of the tag</param>
        /// <param name="simpleName">The new simple name</param>
        /// <param name="tagGroup">The new tag group</param>
        public void putTag(string id, Tag tag)
        {
            DatabaseConnector database = DatabaseConnector.GetInstance;
            database.putTag(id, tag.name, tag.simple_name, tag.tag_group);
        }

        /// <summary>
        /// Deletes a tag from the database.
        /// </summary>
        /// <param name="id">The id of the tag to delete</param>
        public void deleteTag(string id)
        {
            DatabaseConnector database = DatabaseConnector.GetInstance;
            database.deleteTag(id);
        }

        /// <summary>
        /// Gets a tag group from the database.
        /// </summary>
        /// <param name="id">The id of the tag group</param>
        /// <returns>Tag group object</returns>
        public TagGroup getTagGroup(string id)
        {
            DatabaseConnector database = DatabaseConnector.GetInstance;
            return database.getTagGroup(id);
        }

        /// <summary>
        /// Returns all tag groups from the database.
        /// </summary>
        /// <param name="limit">Number of tag groups per page.</param>
        /// <param name="page">Page offset</param>
        /// <returns>Array of tagGroups</returns>
        public TagGroup[] getTagGroups(string limit, string page)
        {
            DatabaseConnector database = DatabaseConnector.GetInstance;
            return database.getTagGroups(limit, page);
        }

        /// <summary>
        /// Create a new tag group and save it in the database.
        /// </summary>
        /// <param name="name">The name of the tag group</param>
        /// <returns>The id of the newly created tag group</returns>
        public int postTagGroup(TagGroup tagGroup)
        {
            DatabaseConnector database = DatabaseConnector.GetInstance;
            return database.postTagGroup(tagGroup.name);
        }

        /// <summary>
        /// Updates a tag group
        /// </summary>
        /// <param name="id">The id of the tag group to update</param>
        /// <param name="name">The new name of the tag group</param>
        public void putTagGroup(string id, TagGroup tagGroup)
        {            
            DatabaseConnector database = DatabaseConnector.GetInstance;
            database.putTagGroup(id, tagGroup.name);
        }

        /// <summary>
        /// Deletes a tag group from the database
        /// </summary>
        /// <param name="id">The id of the tag group to delete</param>
        public void deleteTagGroup(string id)
        {
            DatabaseConnector database = DatabaseConnector.GetInstance;
            database.deleteTagGroup(id);
        }

        //==================================== Ragting ===============================================

        /// <summary>
        /// Gets the rating for a media
        /// </summary>
        /// <param name="media">The id of the media</param>
        /// <returns>Rating object</returns>
        public Rating getRating(string media)
        {
            DatabaseConnector database = DatabaseConnector.GetInstance;
            return database.getRating(media);
        }

        /// <summary>
        /// Posts a new rating for a media
        /// </summary>
        /// <param name="userId">Id of the user who posted</param>
        /// <param name="mediaId">Id of the media the rating belongs to</param>
        /// <param name="stars">Amount of stars to give</param>
        /// <param name="commentTitle">Title of the comment</param>
        /// <param name="comment">Content of the comment</param>
        public void postRating(Rating rating)
        {
            DatabaseConnector database = DatabaseConnector.GetInstance;
            database.postRating(rating.userId, rating.mediaId, rating.rating, rating.commentTitle, rating.comment);
        }

        /// <summary>
        /// Updates an already existing comment
        /// </summary>
        /// <param name="id">The id of the comment to edit</param>
        /// <param name="commentTitle">The new title of the comment</param>
        /// <param name="comment">The new content of the comment</param>
        /// <param name="stars">The new amount of stars</param>

        public void putRating(string id, Rating rating)
        {
            DatabaseConnector database = DatabaseConnector.GetInstance;
            database.putRating(int.Parse(id), rating.commentTitle, rating.comment, rating.rating);
        }
    }
}
