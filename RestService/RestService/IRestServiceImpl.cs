using System;
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
            UriTemplate = "/user?group_id={group_id}&search_string={search_string}&search_fields={search_fields}")]
        User[] getUsersWithParameter(string group_id, string search_string, string search_fields);

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
        int insertMedia(Media media);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/mediaFiles")]
        void insertMediaFile(FileStream file);
    }
}
