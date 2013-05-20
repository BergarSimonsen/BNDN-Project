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
using System.Web;
using RestService.Controllers;
using RestService.IO_Messages;
using RestService.Web_Service;
using RestService.Entities;


namespace RestService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RestServiceImpl" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select RestServiceImpl.svc or RestServiceImpl.svc.cs at the Solution Explorer and start debugging.
    public class RestServiceImpl : IRestServiceImpl
    {
        /// <summary>
        /// Makes the request object and fills it with everything except the user
        /// </summary>
        /// <param name="request">The incoming request context</param>
        /// <param name="data">The data dictionary</param>
        /// <returns></returns>
        private static Request makeRequest(IncomingWebRequestContext request, Dictionary<string, string> data, string authorization)
        {
            //gets the whole uri
            string pathUri = request.UriTemplateMatch.RequestUri.PathAndQuery;

            //we only want the important part. example user/23
            string[] trimmedUri = pathUri.Split(new string[] {"/RestServiceImpl.svc/"},StringSplitOptions.None);
            string importantUri = trimmedUri[1];

            //breaks up the important part of the uri and puts them in a linked list
            string[] uriParts = importantUri.Split('/');
            LinkedList<string> uri = new LinkedList<string>(uriParts);

            //gets the request method
            RestMethods method = (RestMethods) Enum.Parse(typeof(RestMethods), request.Method);

            return new Request(uri, method, data, null, authorization);
        }

        /// <summary>
        /// Trims a dictionary, removing everything that has a null value
        /// </summary>
        /// <param name="data">The dictionary</param>
        /// <returns></returns>
        private static Dictionary<string, string> trimData(Dictionary<string, string> data)
        {
            Dictionary<string, string> trimmedData = new Dictionary<string, string>();

            foreach(KeyValuePair<string,string> pair in data)
            {
                if (pair.Value != null)
                {
                    trimmedData.Add(pair.Key, pair.Value);
                }
            }

            return trimmedData;
        }

        public Response<User> getUser(string id)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("id", id);

            Request request = makeRequest(requestContext, data, authString);

            UserController controller = new UserController();

            return controller.Call(request);
        }

        public Response<User> getUsersWithParameter(string group_id, string emailFilter, string limit, string page, string order_by, string order)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("user_group_id", group_id);
            data.Add("email", emailFilter);
            data.Add("limit", limit);
            data.Add("page", page);
            data.Add("order by", order_by);
            data.Add("order", order);

            Request request = makeRequest(requestContext, trimData(data), authString);

            UserController controller = new UserController();

            return controller.Call(request);
        }

        public Response<User> insertUser(User user)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("email", user.email);
            data.Add("password", user.password);

            Request request = makeRequest(requestContext, data, authString);

            UserController controller = new UserController();

            return controller.Call(request);
        }

        public Response<User> getLoggedUser()
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            Request request = makeRequest(requestContext, trimData(data), authString);

            UserController controller = new UserController();

            return controller.Call(request);
        }

        public Response<Token> getToken(string email, string password)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("email", email);
            data.Add("password", password);

            Request request = makeRequest(requestContext, trimData(data), authString);

            TokenController controller = new TokenController();

            return controller.Call(request);
        }

        public Response<Token> renewToken()
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            Request request = makeRequest(requestContext, trimData(data), authString);

            TokenController controller = new TokenController();

            return controller.Call(request);
        }

        public Response<User> updateUser(string id, string oldPassword, User newUser)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("id", id);
            data.Add("oldPassword", oldPassword);
            data.Add("email", newUser.email);
            data.Add("password", newUser.password);

            Request request = makeRequest(requestContext, data, authString);

            UserController controller = new UserController();

            return controller.Call(request);
        }

        public Response<User> deleteUser(string id)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("id", id);

            Request request = makeRequest(requestContext, trimData(data), authString);

            UserController controller = new UserController();

            return controller.Call(request);
        }

        public Response<Media> getMedia(string id)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("id", id);

            Request request = makeRequest(requestContext, trimData(data), authString);

            MediaController controller = new MediaController();

            return controller.Call(request);
        }

        public Response<Media> getMedias(string tag, string mediaCategoryFilter, string nameFilter, string page, string limit)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("tag", tag);
            data.Add("media_category_id", mediaCategoryFilter);
            data.Add("title", nameFilter);
            if (limit == null)
            {
                limit = "10";
            }
            data.Add("limit", limit);
            if (page == null)
            {
                page = "1";
            }
            data.Add("page", page);

            Request request = makeRequest(requestContext, trimData(data), authString);

            MediaController controller = new MediaController();

            return controller.Call(request);
        }

        public Response<Media> postMedia(Media media)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("type", "");
            data.Add("description", media.description);
            data.Add("format", media.format);
            data.Add("media_category_id", media.mediaCategory.ToString());
            data.Add("minutes", media.mediaLength.ToString());
            data.Add("title", media.title);
            data.Add("user_account_id", media.user.ToString());

            Request request = makeRequest(requestContext, trimData(data), authString);

            MediaController controller = new MediaController();

            return controller.Call(request);
        }

        public Response<Media> putMedia(Media media, string id)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("id", id);
            data.Add("type", "");
            data.Add("description", media.description);
            data.Add("format", media.format);
            data.Add("media_category_id", media.mediaCategory.ToString());
            data.Add("minutes", media.mediaLength.ToString());
            data.Add("title", media.title);
            data.Add("user_account_id", media.user.ToString()); ;

            Request request = makeRequest(requestContext, trimData(data), authString);

            MediaController controller = new MediaController();

            return controller.Call(request);
        }

        public Response<Media> deleteMedia(string id)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("id", id);

            Request request = makeRequest(requestContext, trimData(data), authString);

            MediaController controller = new MediaController();

            return controller.Call(request);
        }

        public Response<Media> mediaHasTag(string mediaId, string tagId)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("mediaId", mediaId);
            data.Add("tagId", tagId);

            Request request = makeRequest(requestContext, trimData(data), authString);

            MediaController controller = new MediaController();

            return controller.Call(request);
        }

        public Response<Media> getMediaUserByTag(string userid, string userTagId)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("user_account_id", userid);
            data.Add("user_account_tag_id", userTagId);

            Request request = makeRequest(requestContext, trimData(data), authString);

            MediaController controller = new MediaController();

            return controller.Call(request);
        }

        public Response<Media> insertMediaFile(Stream file, string id)
        {
            throw new NotImplementedException();
        }

        public Response<MediaCategory> getMediaCategories()
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            Request request = makeRequest(requestContext, trimData(data), authString);

            MediaCategoryController controller = new MediaCategoryController();

            return controller.Call(request);
        }

        public Response<MediaCategory> getMediaCategory(string id)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("id", id);

            Request request = makeRequest(requestContext, trimData(data), authString);

            MediaCategoryController controller = new MediaCategoryController();

            return controller.Call(request);
        }

        public Response<MediaCategory> postMediaCategory(MediaCategory mediaCategory)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("description", mediaCategory.description);
            data.Add("name", mediaCategory.name);

            Request request = makeRequest(requestContext, trimData(data), authString);

            MediaCategoryController controller = new MediaCategoryController();

            return controller.Call(request);
        }

        public Response<MediaCategory> putMediaCategory(string id, MediaCategory mediaCategory)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("description", mediaCategory.description);
            data.Add("name", mediaCategory.name);
            data.Add("id", id);

            Request request = makeRequest(requestContext, trimData(data), authString);

            MediaCategoryController controller = new MediaCategoryController();

            return controller.Call(request);
        }

        public Response<MediaCategory> deleteMediaCategory(string id)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("id", id);

            Request request = makeRequest(requestContext, trimData(data), authString);

            MediaCategoryController controller = new MediaCategoryController();

            return controller.Call(request);
        }

        public Response<Tag> getTags(string tagGroupFilter, string limit, string page)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("tagGroup", tagGroupFilter);
            data.Add("limit", limit);
            data.Add("page", page);

            Request request = makeRequest(requestContext, trimData(data),authString);

            TagController controller = new TagController();

            return controller.Call(request);
        }

        public Response<Tag> getTag(string id)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("id", id);

            Request request = makeRequest(requestContext, trimData(data), authString);

            TagController controller = new TagController();

            return controller.Call(request);
        }

        public Response<Tag> postTag(Tag tag)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("name", tag.name);
            data.Add("simpleName", tag.simple_name);
            data.Add("tagGroup", tag.tag_group.ToString());

            Request request = makeRequest(requestContext, trimData(data), authString);

            TagController controller = new TagController();

            return controller.Call(request);
        }

        public Response<Tag> putTag(string id, Tag tag)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("name", tag.name);
            data.Add("simpleName", tag.simple_name);
            data.Add("tagGroup", tag.tag_group.ToString());
            data.Add("id", id);

            Request request = makeRequest(requestContext, trimData(data), authString);

            TagController controller = new TagController();

            return controller.Call(request);
        }

        public Response<Tag> deleteTag(string id)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("id", id);

            Request request = makeRequest(requestContext, trimData(data), authString);

            TagController controller = new TagController();

            return controller.Call(request);
        }

        public Response<TagGroup> getTagGroup(string id)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("id", id);

            Request request = makeRequest(requestContext, trimData(data), authString);

            TagGroupController controller = new TagGroupController();

            return controller.Call(request);
        }

        public Response<TagGroup> getTagGroups(string limit, string page)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("limit", limit);
            data.Add("page", page);

            Request request = makeRequest(requestContext, trimData(data), authString);

            TagGroupController controller = new TagGroupController();

            return controller.Call(request);
        }

        public Response<TagGroup> postTagGroup(TagGroup tagGroup)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("name", tagGroup.name);

            Request request = makeRequest(requestContext, trimData(data), authString);

            TagGroupController controller = new TagGroupController();

            return controller.Call(request);
        }

        public Response<TagGroup> putTagGroup(string id, TagGroup tagGroup)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("id", id);
            data.Add("name", tagGroup.name);

            Request request = makeRequest(requestContext, trimData(data), authString);

            TagGroupController controller = new TagGroupController();

            return controller.Call(request);
        }

        public Response<TagGroup> deleteTagGroup(string id)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("id", id);

            Request request = makeRequest(requestContext, trimData(data), authString);

            TagGroupController controller = new TagGroupController();

            return controller.Call(request);
        }

        public Response<Tag> getTagByMedia(string mediaId)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("id", mediaId);

            Request request = makeRequest(requestContext, trimData(data), authString);

            TagController controller = new TagController();

            return controller.Call(request);
        }

        public Response<Rating> getRating(string mediaId, string userId, string limit, string page)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("media_id", mediaId);
            data.Add("user_account_id", userId);
            data.Add("limit", limit);
            data.Add("page", page);

            Request request = makeRequest(requestContext, trimData(data), authString);

            RatingController controller = new RatingController();

            return controller.Call(request);
        }

        public Response<Rating> postRating(Rating rating)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("media_id", rating.mediaId.ToString());
            data.Add("user_account_id", rating.userId.ToString());
            data.Add("comment", rating.comment);
            data.Add("comment_title", rating.commentTitle);
            data.Add("rating", rating.rating.ToString());

            Request request = makeRequest(requestContext, trimData(data), authString);

            RatingController controller = new RatingController();

            return controller.Call(request);
        }

        public Response<Rating> putRating(string id, Rating rating)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("id", id);
            data.Add("comment", rating.comment);
            data.Add("comment_title", rating.commentTitle);
            data.Add("rating", rating.rating.ToString());

            Request request = makeRequest(requestContext, trimData(data), authString);

            RatingController controller = new RatingController();

            return controller.Call(request);
        }

        public Response<Rating> deleteRating(string id)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("id", id);

            Request request = makeRequest(requestContext, trimData(data), authString);

            RatingController controller = new RatingController();

            return controller.Call(request);
        }

        public Response<User_Data> getUserData(string userId)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("user_account_id", userId);

            Request request = makeRequest(requestContext, trimData(data), authString);

            UserDataController controller = new UserDataController();

            return controller.Call(request);
        }

        public Response<User_Data> postUserData(User_Data userData)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("user_account_id", userData.userId.ToString());
            data.Add("name", userData.userDataType);
            data.Add("value", userData.value);

            Request request = makeRequest(requestContext, trimData(data), authString);

            UserDataController controller = new UserDataController();

            return controller.Call(request);
        }

        public Response<User_Data> putUserData(string id, User_Data userData)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("id", id);
            data.Add("name", userData.userDataType);
            data.Add("value", userData.value);

            Request request = makeRequest(requestContext, trimData(data), authString);

            UserDataController controller = new UserDataController();

            return controller.Call(request);
        }

        public Response<User_Data> deleteUserData(string id)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("id", id);

            Request request = makeRequest(requestContext, trimData(data), authString);

            UserDataController controller = new UserDataController();

            return controller.Call(request);
        }

        public Response<UserGroup> getUserGroup(string id)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("id", id);

            Request request = makeRequest(requestContext, trimData(data), authString);

            UserGroupController controller = new UserGroupController();

            return controller.Call(request);
        }

        public Response<UserGroup> postUserGroup(UserGroup userGroup)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("name", userGroup.name);
            data.Add("description", userGroup.description);

            Request request = makeRequest(requestContext, trimData(data),authString);

            UserGroupController controller = new UserGroupController();

            return controller.Call(request);
        }

        public Response<UserGroup> putUserGroup(UserGroup userGroup, string id)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("id", id);
            data.Add("name", userGroup.name);
            data.Add("description", userGroup.description);

            Request request = makeRequest(requestContext, trimData(data), authString);

            UserGroupController controller = new UserGroupController();

            return controller.Call(request);
        }

        public Response<UserGroup> deleteUserGroup(string id)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("id", id);

            Request request = makeRequest(requestContext, trimData(data), authString);

            UserGroupController controller = new UserGroupController();

            return controller.Call(request);
        }

        public Response<User> getUsersInGroup(string groupId)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("user_group_id", groupId);

            Request request = makeRequest(requestContext, trimData(data), authString);

            UserController controller = new UserController();

            return controller.Call(request);
        }

        public Response<UserGroup> getGroupsOfUser(string userId)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("user_account_id", userId);

            Request request = makeRequest(requestContext, trimData(data), authString);

            UserGroupController controller = new UserGroupController();

            return controller.Call(request);
        }

        public Response<UserGroup> postUserInUsergroup(string userId, string groupId)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("user_account_id", userId);
            data.Add("user_group_id", userId);

            Request request = makeRequest(requestContext, trimData(data), authString);

            UserGroupController controller = new UserGroupController();

            return controller.Call(request);
        }

        public Response<UserGroup> removeUserFromGroup(string userId, string groupId)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("user_account_id", userId);
            data.Add("user_group_id", userId);

            Request request = makeRequest(requestContext, trimData(data), authString);

            UserGroupController controller = new UserGroupController();

            return controller.Call(request);
        }

        public Response<Entities.Action> getAction(string id)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("id", id);

            Request request = makeRequest(requestContext, trimData(data), authString);

            ActionController controller = new ActionController();

            return controller.Call(request);
        }

        public Response<RestService.Entities.Action> getActions()
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            Request request = makeRequest(requestContext, trimData(data), authString);

            ActionController controller = new ActionController();

            return controller.Call(request);
        }

        public Response<Entities.Action> getUserCanDoAction(string userId)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("user_account_id", userId);

            Request request = makeRequest(requestContext, trimData(data), authString);

            ActionController controller = new ActionController();

            return controller.Call(request);
        }

        public Response<Entities.Action> postUserCanDoAction(string userId, string actionId, string contentId, string allow)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("user_account_id", userId);
            data.Add("action_id", actionId);
            data.Add("content_id", contentId);
            data.Add("allow", allow);

            Request request = makeRequest(requestContext, trimData(data), authString);

            ActionController controller = new ActionController();

            return controller.Call(request);
        }

        public Response<Entities.Action> deleteUserCanDoAction(string userId, string actionId)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("user_account_id", userId);
            data.Add("action_id", actionId);

            Request request = makeRequest(requestContext, trimData(data), authString);

            ActionController controller = new ActionController();

            return controller.Call(request);
        }

        public Response<Entities.Action> getGroupCanDoAction(string groupId)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("user_group_id", groupId);

            Request request = makeRequest(requestContext, trimData(data), authString);

            ActionController controller = new ActionController();

            return controller.Call(request);
        }

        public Response<Entities.Action> postGroupCanDoAction(string groupId, string actionId, string contentId, string allow)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("user_group_id", groupId);
            data.Add("action_id", actionId);
            data.Add("content_id", contentId);
            data.Add("allow", allow);

            Request request = makeRequest(requestContext, trimData(data), authString);

            ActionController controller = new ActionController();

            return controller.Call(request);
        }

        public Response<Entities.Action> deleteGroupCanDoAction(string groupId, string actionId)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("user_account_id", groupId);
            data.Add("action_id", actionId);

            Request request = makeRequest(requestContext, trimData(data), authString);

            ActionController controller = new ActionController();

            return controller.Call(request);
        }

        public Response<UserAccountTag> getUserTag(string id)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("id", id);

            Request request = makeRequest(requestContext, trimData(data), authString);

            UserAccountTagController controller = new UserAccountTagController();

            return controller.Call(request);
        }

        public Response<UserAccountTag> getUserTags()
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            Request request = makeRequest(requestContext, trimData(data), authString);

            UserAccountTagController controller = new UserAccountTagController();

            return controller.Call(request);
        }

        public Response<UserAccountTag> postUserTag(UserAccountTag userTag)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("name", userTag.name);

            Request request = makeRequest(requestContext, trimData(data), authString);

            UserAccountTagController controller = new UserAccountTagController();

            return controller.Call(request);
        }

        public Response<UserAccountTag> deleteUserTag(string id)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("id", id);

            Request request = makeRequest(requestContext, trimData(data), authString);

            UserAccountTagController controller = new UserAccountTagController();

            return controller.Call(request);
        }

        public Response<UserAccountTag> getUserTagsByUser(string userId)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("user_account_id", userId);

            Request request = makeRequest(requestContext, trimData(data), authString);

            UserAccountTagController controller = new UserAccountTagController();

            return controller.Call(request);
        }

        public Response<UserAccountTag> postUserAccountTag(string userId, string mediaId, string tagId)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("user_account_id", userId);
            data.Add("user_account_tag_id", tagId);
            data.Add("media_id", mediaId);

            Request request = makeRequest(requestContext, trimData(data), authString);

            UserAccountTagController controller = new UserAccountTagController();

            return controller.Call(request);
        }

        public Response<UserAccountTag> deleteMediaByUserTag(string mediaId, string tagId)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("user_account_tag_id", tagId);
            data.Add("media_id", mediaId);

            Request request = makeRequest(requestContext, trimData(data), authString);

            UserAccountTagController controller = new UserAccountTagController();

            return controller.Call(request);
        }

        public Response<UserAccountTag> deleteUserAccountTag(string tagId)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("user_account_tag_id", tagId);

            Request request = makeRequest(requestContext, trimData(data), authString);

            UserAccountTagController controller = new UserAccountTagController();

            return controller.Call(request);
        }
    }
}
