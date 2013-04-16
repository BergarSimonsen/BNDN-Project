﻿using System;
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

        public UserList getUsersWithParameter(string group_id, string emailFilter, string limit, string page, string order_by, string order)
        {
            //Sanitize groupID
            int groupId_s = 0;
            if (group_id != null)
            {
                groupId_s = int.Parse(group_id);
            }

            //Sanitize searchString
            string email = null;
            //TODO: Implement
            if (emailFilter != null)
            {
                email = emailFilter;
            }

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
            int page_s = 1; //Default
            if (page != null)
            {
                page_s = int.Parse(page);
                if (page_s < 1) page_s = 1;
            }

            //Get result
            DatabaseConnector database = DatabaseConnector.GetInstance;

            User[] returnUsers = database.getUsers(groupId_s, emailFilter, orderBy_s, order_s, limit_s, page_s);
            int userCount = database.getUsersCount();
            int pageCount = (userCount + limit_s - 1) / limit_s;

            return new UserList(limit_s, pageCount, returnUsers);
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
            int pageNumber = 1;
            if (page != null)
            {
                pageNumber = int.Parse(page);
            }
            int limitNumber = 10;
            if(limit != null)
            {
                limitNumber = int.Parse(limit);
            }

            DatabaseConnector database = DatabaseConnector.GetInstance;
            return database.getMedias(mediaTag,mediaCategory,nameFilter,pageNumber,limitNumber);
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
        public void insertMediaFile(Stream file, string id)
        {
            DatabaseConnector database = DatabaseConnector.GetInstance;
            Media mediaMeta = database.getMedia(int.Parse(id));

            string fileLocation = @"C:\RentItServices\Rentit26\MediaFiles\"+id;
            System.IO.Directory.CreateDirectory(fileLocation);
            string fileDir = fileLocation + @"\" + mediaMeta.title + "." +mediaMeta.format;
 
            FileStream writer = new FileStream(fileDir,FileMode.Create,FileAccess.Write);

            byte[] bytes = new Byte[4096];

            int bytesRead = 0;

            while ((bytesRead = file.Read(bytes, 0, bytes.Length)) != 0)
            {
                writer.Write(bytes,0,bytesRead);
            }

            file.Close();
            writer.Close();

            string fileStream = "http://rentit.itu.dk/RentIt26/MediaFiles/" + id + "/" + mediaMeta.title + "." + mediaMeta.format;

            database.putMedia(new string[] { "file_location" }, new string[] { fileStream }, int.Parse(id));
        }

        /// <summary>
        /// Updates a media.
        /// </summary>
        /// <param name="table">The table to update</param>
        /// <param name="value">The values to insert</param>
        /// <param name="id">The id of the media to update</param>
        public void putMedia(Media media, string id)
        {   
            List<string> table = new List<string>();
            List<string> value = new List<string>();

            if (media.description != null)
            {
                table.Add("description");
                value.Add(media.description);
            }
            if (media.mediaCategory != 0)
            {
                table.Add("media_category_id");
                value.Add(media.mediaCategory.ToString());
            }
            if (media.title != null)
            {
                table.Add("title");
                value.Add(media.title);
            }

            DatabaseConnector database = DatabaseConnector.GetInstance;
            database.putMedia(table.ToArray(), value.ToArray(), int.Parse(id));
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
            database.putMediaCategory(int.Parse(id), mediaCategory);
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
        public Tag[] getTags(string tagGroupFilter, string limit, string page)
        {
            int tagGroup = 0;
            if(tagGroupFilter != null)
            {
                tagGroup = int.Parse(tagGroupFilter);
            }
            int limitNumber = 10;
            if (limit != null)
            {
                limitNumber = int.Parse(limit);
            }
            int pageNumber = 1;
            if (page != null)
            {
                pageNumber = int.Parse(page);
            }
            
            DatabaseConnector database = DatabaseConnector.GetInstance;
            return database.getTags(tagGroup, limitNumber, pageNumber);
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
        /// Gets all tags that belong to a media
        /// </summary>
        /// <param name="media">The media that the tags belong to</param>
        /// <returns>Array of all tags</returns>
        public Tag[] getTagByMedia(string media)
        {
            DatabaseConnector database = DatabaseConnector.GetInstance;
            return database.getTagByMedia(int.Parse(media));
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
            int limitNumber = 10;
            if (limit != null)
            {
                limitNumber = int.Parse(limit);
            }
            int pageNumber = 1;
            if (page != null)
            {
                pageNumber = int.Parse(page);
            }

            DatabaseConnector database = DatabaseConnector.GetInstance;
            return database.getTagGroups(limitNumber, pageNumber);
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

        /// <summary>
        /// Assigns a tag to a media
        /// </summary>
        /// <param name="media">Id of the media</param>
        /// <param name="tag">Id of the tag</param>
        public void mediaHasTag(string media, string tag)
        {
            DatabaseConnector database = DatabaseConnector.GetInstance;
            database.mediaHasTag(int.Parse(media), int.Parse(tag));
        }

        //==================================== Ragting ===============================================

        /// <summary>
        /// Gets the rating for a media
        /// </summary>
        /// <param name="media">The id of the media</param>
        /// <returns>Rating object</returns>
        public Rating[] getRating(string media, string user)
        {
            int mediaId = 0;
            if (media != null)
            {
                mediaId = int.Parse(media);
            }
            int userId = 0;
            if (user != null)
            {
                userId = int.Parse(user);
            }

            DatabaseConnector database = DatabaseConnector.GetInstance;
            return database.getRating(mediaId, userId);
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

        /// <summary>
        /// Deletes a rating
        /// </summary>
        /// <param name="id">Id of the rating to delete</param>
        public void deleteRating(string id)
        {
            DatabaseConnector database = DatabaseConnector.GetInstance;
            database.deleteRating(int.Parse(id));
        }
    }
}