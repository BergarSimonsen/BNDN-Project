using System;
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
        string getToken(string email, string password);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/user/token/renew")]
        string renewToken();

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


        //User data
        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/userData/{id}")]
        Response<User> postUserData(string id, string userData);

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/userData/{id}")]
        Response<User> putUserData(string id, string userData);

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/userData/{id}")]
        Response<User> deleteUserData(string id);

        //========================================= USER GROUPS ================================//

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/userGroup/{id}")]
        Response<UserGroup> getUserGroup(string id);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/userGroup")]
        Response<UserGroup> postUserGroup(UserGroup userGroup);

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/userGroup/{id}")]
        Response<UserGroup> putUserGroup(UserGroup userGroup, string id);

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/userGroup/{id}")]
        Response<UserGroup> deleteUserGroup(string id);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/usersInGroup/{groupId}")]
        Response<User> getUsersInGroup(string groupId);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "groupOfUser/{userId}")]
        Response<UserGroup> getGroupsOfUser(string userId);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "userInGroup/")]
        Response<UserGroup> postUserInUsergroup(string userId, string groupId);

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "removeUserFromGroup/{userid}/{groupid}")]
        Response<UserGroup> removeUserFromGroup(string userId, string groupId);

        //========================================= ACTION ================================//

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/action/{id}")]
        Response<RestService.Entities.Action> getAction(string id);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/action")]
        Response<RestService.Entities.Action> postAction(RestService.Entities.Action action);

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/action/{id}")]
        Response<RestService.Entities.Action> putAction(RestService.Entities.Action action, string id);

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/action/{id}")]
        Response<RestService.Entities.Action> deleteAction(string id);

        // user_account_can_do_action
        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/userActions/{userId}")]
        Response<RestService.Entities.Action> getUserCanDoAction(string userId);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/userCanDoAction/{userId}/{actionId}")]
        Response<RestService.Entities.Action> postUserCanDoAction(string userId, string actionId);

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/removeUserAction/{userId}/{actionId}")]
        Response<RestService.Entities.Action> deleteUserCanDoAction(string userId, string actionId);

        // User_group_can_do_action
        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/groupActions/{groupId}")]
        Response<RestService.Entities.Action> getGroupCanDoAction(string groupId);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/groupCanDoAction/{groupId}/{actionId}")]
        Response<RestService.Entities.Action> postGroupCanDoAction(string groupId, string actionId);

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/removeGroupAction/{groupId}/{actionId}")]
        Response<RestService.Entities.Action> deleteGroupCanDoAction(string groupId, string actionId);


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
        string insertMediaFile(Stream file, string id);

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

        // User tag
        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/userTags/{id}")]
        Response<UserAccountTag> getUserTag(string id);

        [OperationContract] 
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/userTags")]
        Response<UserAccountTag> getUserTag();

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/userTags")]
        Response<UserAccountTag> postUserTag(UserAccountTag userTag);

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/userTags/{id}")]
        Response<UserAccountTag> deleteUserTag(string id);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/mediaByTag/{userId}/{tagId}")]
        Response<Media> getMediaByTag(string userid, string tagId);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tagsByUser/{userId}")]
        Response<UserAccountTag> getTagsByUser(string userId);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "userAccountTag/{userId}/{mediaId}/{tagId}")]
        Response<UserAccountTag> postUserAccountTag(string userId, string mediaId, string tagId);

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "mediaByTag/{mediaId}/{tagId}")]
        Response<UserAccountTag> deleteMediaByTag(string mediaId, string tagId);

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "userAccountTag/{tagId}")]
        Response<UserAccountTag> deleteUserAccountTag(string tagId);


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
