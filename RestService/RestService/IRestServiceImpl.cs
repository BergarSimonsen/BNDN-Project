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

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/user?group_id={group_id}&search_string={search_string}&search_fields={search_fields}&limit={limit}&page={page}&order_by={order_by}&order={order}")]
        UserList getUsersWithParameter(string group_id, string search_string, string search_fields, string limit, string page, string order_by, string order);

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

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/media?andTags={andTags}&orTags={orTags}&mediaCategoryFilter={mediaCategoryFilter}&nameFilter={nameFilter}&page={page}&limit={limit}")]
        MediaList getMedias(string andTags, string orTags,string mediaCategoryFilter, string nameFilter, string page, string limit);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/media")]
        int postMedia(Media media);

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/media/{id}")]
        void putMedia(string[] table, string[] value, string id);

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/media/{id}")]
        void deleteMedia(string id);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/mediaFiles")]
        void insertMediaFile(Stream file);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/mediaCategory")]
        MediaCategory[] getMediaCategories();

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/mediaCategory/{id}")]
        MediaCategory getMediaCategory(string id);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/mediaCategory")]
        int postMediaCategory(MediaCategory mediaCategory);

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/mediaCategory/{id}")]
        void putMediaCategory(string id, MediaCategory mediaCategory);

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/mediaCategory/{id}")]
        void deleteMediaCategory(string id);

        //========================================= TAGS ===================================================//

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tags")]
        Tag[] getTags(int tagGroupFilter, int limit, int page);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tags/{id}")]
        Tag getTag(string id);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tags")]
        int postTag(string name, string simpleName, int tagGroup);

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tags/{id}")]
        void putTag(string id, string name, string simpleName, int tagGroup);

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            UriTemplate = "/tags/{id}")]
        void deleteTag(string id);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tagGroups/{id}")]
        TagGroup getTagGroup(string id);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tagGroups?limit={limit}&page={page}")]
        TagGroup[] getTagGroups(string limit, string page); 

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tagGroups")]
        int postTagGroup(string name);

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tagGroups/{id}")]
        void putTagGroup(string id, string name);

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            UriTemplate = "/tagGroups/{id}")]
        void deleteTagGroup(string id);

        //========================================= RATING ===================================================//

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/rating/{media}")]
        Rating getRating(string media);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            UriTemplate = "/rating")]
        void postRating(Rating rating);

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            UriTemplate = "/rating/{id}")]
        void putRating(Rating rating);
    }
}
