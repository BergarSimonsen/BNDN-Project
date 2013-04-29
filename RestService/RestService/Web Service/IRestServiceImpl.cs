﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using RestService.IO_Messages;
using RestService.Entities;

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
        Response<User> getUser(string id);
        // Working

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/user?group_id={group_id}&emailFilter={emailFilter}&limit={limit}&page={page}&order_by={order_by}&order={order}")]
        Response<User> getUsersWithParameter(string group_id, string emailFilter, string limit, string page, string order_by, string order);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/user")]
        Response<User> insertUser(User user);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/user/me")]
        Response<User> getLoggedUser();
        // Bad request .. no user logged in tho

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/user/token/{email}/{password}")]
        Response<string> getToken(string email, string password);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/user/token/renew")]
        Response<string> renewToken();

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/user/{id}/{oldPassword}")]
        Response<User> updateUser(string id, string oldPassword, User newUser);

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/user/{id}")]
        Response<User> deleteUser(string id);

        //========================================= MEDIA AND MEDIA CATEGORY ================================//
        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/media/{id}")]
        Response<Media> getMedia(string id);
        // Working

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/media?tag={tag}&mediaCategoryFilter={mediaCategoryFilter}&nameFilter={nameFilter}&page={page}&limit={limit}")]
        Response<Media> getMedias(string tag, string mediaCategoryFilter, string nameFilter, string page, string limit);
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
        Response<Media> postMedia(Media media);
        // Working

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/media/{id}")]
        Response<Media> putMedia(Media media, string id);
        // No error, but won't update

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/media/{id}")]
        Response<Media> deleteMedia(string id);
        // Working

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/mediaFiles/{id}")]
        Response<string> insertMediaFile(Stream file, string id);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/mediaCategory")]
        Response<MediaCategory> getMediaCategories();
        // Working

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/mediaCategory/{id}")]
        Response<MediaCategory> getMediaCategory(string id);
        // Working

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/mediaCategory")]
        Response<MediaCategory> postMediaCategory(MediaCategory mediaCategory);
        // Working

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/mediaCategory/{id}")]
        Response<MediaCategory> putMediaCategory(string id, MediaCategory mediaCategory);
        // Almost working. Doesn't update description, only name

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/mediaCategory/{id}")]
        Response<MediaCategory> deleteMediaCategory(string id);
        // Working

        

        //========================================= TAGS ===================================================//

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tags?tagGroupFilter={tagGroupFilter}&limit={limit}&page={page}")]
        Response<Tag> getTags(string tagGroupFilter, string limit, string page);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tags/{id}")]
        Response<Tag> getTag(string id);
        // BAD REQUEST

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tags")]
        Response<Tag> postTag(Tag tag);
        // Working

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tags/{id}")]
        Response<Tag> putTag(string id, Tag tag);
        // Bad request

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            UriTemplate = "/tags/{id}")]
        Response<Tag> deleteTag(string id);
        // Bad request

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tagGroups/{id}")]
        Response<TagGroup> getTagGroup(string id);
        // Working

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tagGroups?limit={limit}&page={page}")]
        Response<TagGroup> getTagGroups(string limit, string page); 
        // Bad request

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tagGroups")]
        Response<TagGroup> postTagGroup(TagGroup tagGroup);
        // Working

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tagGroups/{id}")]
        Response<TagGroup> putTagGroup(string id, TagGroup tagGroup);
        // WORKING

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            UriTemplate = "/tagGroups/{id}")]
        Response<TagGroup> deleteTagGroup(string id);
        // WORKING

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tagsByMedia/{mediaId}")]
        Response<Tag> getTagByMedia(string mediaId);
        // Works but doesn't return anything. Are there any entries?
        // Hmm... ??

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tagsByMedia/{mediaId}/{tagId}")]
        Response<Media> mediaHasTag(string mediaId, string tagId);
        // Think it's working :p


        //========================================= RATING ===================================================//

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/rating?media={media}&user={user}&limit={limit}&page={page}")]
        Response<Rating> getRating(string media, string user, string limit, string page);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            UriTemplate = "/rating")]
        Response<Rating> postRating(Rating rating);
        // Bad Request

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            UriTemplate = "/rating/{id}")]
        Response<Rating> putRating(string id, Rating rating);
        // Not tested

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            UriTemplate = "/rating/{id}")]
        Response<Rating> deleteRating(string id);
        // Not tested
    }
}
