﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace RestService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IRestServiceImpl" in both code and config file together.
    [ServiceContract]
    public interface IRestServiceImpl
    {
        //================================ USER AND TOKEN ====================================//
        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/user/{id}")]
        User getUser(string id);
        // Working

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/user?group_id={group_id}&emailFilter={emailFilter}&limit={limit}&page={page}&order_by={order_by}&order={order}")]
         UserList getUsersWithParameter(string group_id, string emailFilter, string limit, string page, string order_by, string order);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/user")]
        int insertUser(User user);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/user/me")]
        User getLoggedUser();
        // Bad request .. no user logged in tho

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/user/token/{email}/{password}")]
        Token getToken(string email, string password);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/user/token/renew")]
        Token renewToken();

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/user/{id}/{oldPassword}")]
        void updateUser(string id, string oldPassword, User newUser);

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/user/{id}")]
        void deleteUser(string id);

        //========================================= MEDIA AND MEDIA CATEGORY ================================//
        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/media/{id}")]
        Media getMedia(string id);
        // Working

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/media?tag={tag}&mediaCategoryFilter={mediaCategoryFilter}&nameFilter={nameFilter}&page={page}&limit={limit}")]
        MediaList getMedias(string tag,string mediaCategoryFilter, string nameFilter, string page, string limit);
        // Almost working.
        // Paging doesn't work 100%.
        // Didn't test tag

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/media")]
        int postMedia(Media media);
        // Working

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/media/{id}")]
        void putMedia(Media media, string id);
        // No error, but won't update

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/media/{id}")]
        void deleteMedia(string id);
        // Working

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/mediaFiles/{id}")]
        void insertMediaFile(Stream file, string id);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/mediaCategory")]
        MediaCategory[] getMediaCategories();
        // Working

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/mediaCategory/{id}")]
        MediaCategory getMediaCategory(string id);
        // Working

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/mediaCategory")]
        int postMediaCategory(MediaCategory mediaCategory);
        // Working

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/mediaCategory/{id}")]
        void putMediaCategory(string id, MediaCategory mediaCategory);
        // Almost working. Doesn't update description, only name

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/mediaCategory/{id}")]
        void deleteMediaCategory(string id);
        // Working

        

        //========================================= TAGS ===================================================//

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tags?tagGroupFilter={tagGroupFilter}&limit={limit}&page={page}")]
        Tag[] getTags(string tagGroupFilter, string limit, string page);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tags/{id}")]
        Tag getTag(string id);
        // BAD REQUEST

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tags")]
        int postTag(Tag tag);
        // Working

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tags/{id}")]
        void putTag(string id, Tag tag);
        // Bad request

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            UriTemplate = "/tags/{id}")]
        void deleteTag(string id);
        // Bad request

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tagGroups/{id}")]
        TagGroup getTagGroup(string id);
        // Working

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tagGroups?limit={limit}&page={page}")]
        TagGroup[] getTagGroups(string limit, string page); 
        // Bad request

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tagGroups")]
        int postTagGroup(TagGroup tagGroup);
        // Working

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tagGroups/{id}")]
        void putTagGroup(string id, TagGroup tagGroup);
        // WORKING

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            UriTemplate = "/tagGroups/{id}")]
        void deleteTagGroup(string id);
        // WORKING

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tagsByMedia/{media}")]
        Tag[] getTagByMedia(string media);
        // Works but doesn't return anything. Are there any entries?
        // Hmm... ??

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tagsByMedia/{media}/{tag}")]
        void mediaHasTag(string media, string tag);
        // Think it's working :p


        //========================================= RATING ===================================================//

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/rating?media={media}&user={user}")]
        Rating[] getRating(string media, string user);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            UriTemplate = "/rating")]
        void postRating(Rating rating);
        // Bad Request

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            UriTemplate = "/rating/{id}")]
        void putRating(string id, Rating rating);
        // Not tested

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            UriTemplate = "/rating/{id}")]
        void deleteRating(string id);
        // Not tested
    }
}