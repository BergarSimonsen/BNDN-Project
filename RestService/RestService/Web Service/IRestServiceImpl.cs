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
        // Working DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/user?group_id={group_id}&emailFilter={emailFilter}&limit={limit}&page={page}&order_by={order_by}&order={order}")]
        Response<User> getUsersWithParameter(string group_id, string emailFilter, string limit, string page, string order_by, string order);
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/user")]
        Response<User> insertUser(User user);
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/user/me")]
        Response<User> getLoggedUser();
        // Bad request .. no user logged in tho DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/user/token/{email}/{password}")]
        Response<Token> getToken(string email, string password);
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/user/token/renew")]
        Response<Token> renewToken();
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/user/{id}/{oldPassword}")]
        Response<User> updateUser(string id, string oldPassword, User newUser);
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
			ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/user/{id}")]
        Response<User> deleteUser(string id);
		// DOCUMENTED

        //User data
        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/userData/{userId}")]
        Response<User_Data> getUserData(string userId);
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "POST",
			ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/userData")]
        Response<User_Data> postUserData(User_Data userData);
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/userData/{id}")]
        Response<User_Data> putUserData(string id, User_Data userData);
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/userData/{id}")]
        Response<User_Data> deleteUserData(string id);
		// DOCUMENTED

        //========================================= USER GROUPS ================================//

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/userGroup/{id}")]
        Response<UserGroup> getUserGroup(string id);
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/userGroup")]
        Response<UserGroup> postUserGroup(UserGroup userGroup);
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/userGroup/{id}")]
        Response<UserGroup> putUserGroup(UserGroup userGroup, string id);
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/userGroup/{id}")]
        Response<UserGroup> deleteUserGroup(string id);
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/usersInGroup/{groupId}")]
        Response<User> getUsersInGroup(string groupId);
		// TO BE REMOVED

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/groupOfUser/{userId}")]
        Response<UserGroup> getGroupsOfUser(string userId);
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "POST",
			ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/userInGroup/{userId}/{groupId}")]
        Response<UserGroup> postUserInUsergroup(string userId, string groupId);
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/userInGroup/{userid}/{groupid}")]
        Response<UserGroup> removeUserFromGroup(string userId, string groupId);
		// DOCUMENTED

        //========================================= ACTION ================================//

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/action/{id}")]
        Response<RestService.Entities.Action> getAction(string id);
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/action")]
        Response<RestService.Entities.Action> getActions();
		// DOCUMENTED

        // user_account_can_do_action
        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/userActions/{userId}")]
        Response<RestService.Entities.Action> getUserCanDoAction(string userId);
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/userActions/{userId}/{actionId}/{contentId}/{allow}")]
        Response<RestService.Entities.Action> postUserCanDoAction(string userId, string actionId, string contentId, string allow);
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/userActions/{userId}/{actionId}")]
        Response<RestService.Entities.Action> deleteUserCanDoAction(string userId, string actionId);
		// DOCUMENTED

        // User_group_can_do_action
        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/groupActions/{groupId}")]
        Response<RestService.Entities.Action> getGroupCanDoAction(string groupId);
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/groupActions/{groupId}/{actionId}/{contentId}/{allow}")]
        Response<RestService.Entities.Action> postGroupCanDoAction(string groupId, string actionId, string contentId, string allow);
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/groupActions/{groupId}/{actionId}")]
        Response<RestService.Entities.Action> deleteGroupCanDoAction(string groupId, string actionId);
		// DOCUMENTED


        //========================================= MEDIA AND MEDIA CATEGORY ================================//
        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/media/{id}")]
        Response<Media> getMedia(string id);
        // Working DOCUMENTED

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
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/media")]
        Response<Media> postMedia(Media media);
        // Working
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/media/{id}")]
        Response<Media> putMedia(Media media, string id);
        // No error, but won't update
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/media/{id}")]
        Response<Media> deleteMedia(string id);
        // Working
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "POST",
			ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/mediaFiles/{id}")]
        string insertMediaFile(Stream file, string id);
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/mediaCategory")]
        Response<MediaCategory> getMediaCategories();
        // Working
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/mediaCategory/{id}")]
        Response<MediaCategory> getMediaCategory(string id);
        // Working
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "POST",
			ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/mediaCategory")]
        Response<MediaCategory> postMediaCategory(MediaCategory mediaCategory);
        // Working
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/mediaCategory/{id}")]
        Response<MediaCategory> putMediaCategory(string id, MediaCategory mediaCategory);
        // Almost working. Doesn't update description, only name
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/mediaCategory/{id}")]
        Response<MediaCategory> deleteMediaCategory(string id);
        // Working
		// DOCUMENTED

        

        //========================================= TAGS ===================================================//

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tags?tagGroupFilter={tagGroupFilter}&limit={limit}&page={page}")]
        Response<Tag> getTags(string tagGroupFilter, string limit, string page);
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tags/{id}")]
        Response<Tag> getTag(string id);
        // BAD REQUEST
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "POST",
			ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tags")]
        Response<Tag> postTag(Tag tag);
        // Working
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
			ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tags/{id}")]
        Response<Tag> putTag(string id, Tag tag);
        // Bad request
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
			ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/tags/{id}")]
        Response<Tag> deleteTag(string id);
        // Bad request
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tagGroups/{id}")]
        Response<TagGroup> getTagGroup(string id);
        // Working
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tagGroups?limit={limit}&page={page}")]
        Response<TagGroup> getTagGroups(string limit, string page); 
        // Bad request
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tagGroups")]
        Response<TagGroup> postTagGroup(TagGroup tagGroup);
        // Working
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
			ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tagGroups/{id}")]
        Response<TagGroup> putTagGroup(string id, TagGroup tagGroup);
        // WORKING
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
			ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/tagGroups/{id}")]
        Response<TagGroup> deleteTagGroup(string id);
        // WORKING
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tagsByMedia/{mediaId}")]
        Response<Tag> getTagByMedia(string mediaId);
        // Works but doesn't return anything. Are there any entries?
        // Hmm... ??
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "POST",
			ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/tagsByMedia/{mediaId}/{tagId}")]
        Response<Media> mediaHasTag(string mediaId, string tagId);
        // Think it's working :p
		// DOCUMENTED

        // User tag
        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/userTags/{id}")]
        Response<UserAccountTag> getUserTag(string id);
		// DOCUMENTED

        [OperationContract] 
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/userTags")]
        Response<UserAccountTag> getUserTags();	
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "POST",
			ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/userTags")]
        Response<UserAccountTag> postUserTag(UserAccountTag userTag);
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/userTags/{id}")]
        Response<UserAccountTag> deleteUserTag(string id);
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/mediaByUserTag/{userId}/{userTagId}")]
        Response<Media> getMediaUserByTag(string userid, string userTagId);
		// Documented

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/userTagsByUser/{userId}")]
        Response<UserAccountTag> getUserTagsByUser(string userId);
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "POST",
			ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/userAccountTag/{userId}/{mediaId}/{tagId}")]
        Response<UserAccountTag> postUserAccountTag(string userId, string mediaId, string tagId);
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/mediaByUserTag/{mediaId}/{tagId}")]
        Response<UserAccountTag> deleteMediaByUserTag(string mediaId, string tagId);

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/userAccountTag/{tagId}")]
        Response<UserAccountTag> deleteUserAccountTag(string tagId);


        //========================================= RATING ===================================================//

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/rating?media={media}&user={user}&limit={limit}&page={page}")]
        Response<Rating> getRating(string media, string user, string limit, string page);
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "POST",
			ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            UriTemplate = "/rating")]
        Response<Rating> postRating(Rating rating);
        // Bad Request
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
			ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/rating/{id}")]
        Response<Rating> putRating(string id, Rating rating);
        // Not tested
		// DOCUMENTED

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
			ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/rating/{id}")]
        Response<Rating> deleteRating(string id);
        // Not tested
    }
}
