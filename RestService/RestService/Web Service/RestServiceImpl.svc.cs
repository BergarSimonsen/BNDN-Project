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
        private static Request makeRequest(IncomingWebRequestContext request, Dictionary<string,string> data)
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

            return new Request(uri, method, data, null);
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

            data.Add("authorization", authString);

            data.Add("id", id);

            Request request = makeRequest(requestContext, data);

            UserController controller = new UserController();

            return controller.Call(request);
        }

        public Response<User> getUsersWithParameter(string group_id, string emailFilter, string limit, string page, string order_by, string order)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("authorization", authString);
            data.Add("user_group_id", group_id);
            data.Add("email", emailFilter);
            data.Add("limit", limit);
            data.Add("page", page);
            data.Add("order by", order_by);
            data.Add("order", order);

            Request request = makeRequest(requestContext, trimData(data));

            UserController controller = new UserController();

            return controller.Call(request);
        }

        public Response<User> insertUser(User user)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("authorization", authString);
            data.Add("email", user.email);
            data.Add("password", user.password);

            for (int i = 0; i < user.userData.Length; i++)
            {
                data.Add("userData" + i, user.userData[i].ToString());
            }

            Request request = makeRequest(requestContext, trimData(data));

            UserController controller = new UserController();

            return controller.Call(request);
        }

        public Response<User> getLoggedUser()
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("authorization", authString);

            Request request = makeRequest(requestContext, trimData(data));

            UserController controller = new UserController();

            return controller.Call(request);
        }

        public string getToken(string email, string password)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("authorization", authString);
            data.Add("email", email);
            data.Add("password", password);

            Request request = makeRequest(requestContext, trimData(data));

            return null;
        }

        public string renewToken()
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("authorization", authString);

            Request request = makeRequest(requestContext, trimData(data));

            return null;
        }

        public Response<User> updateUser(string id, string oldPassword, User newUser)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("authorization", authString);
            data.Add("id", id);
            data.Add("oldPassword", oldPassword);
            data.Add("newEmail", newUser.email);
            data.Add("newPassword", newUser.password);

            for (int i = 0; i < newUser.userData.Length; i++)
            {
                data.Add("newUserData" + i, newUser.userData[i].ToString());
            }

            Request request = makeRequest(requestContext, trimData(data));

            UserController controller = new UserController();

            return controller.Call(request);
        }

        public Response<User> deleteUser(string id)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("authorization", authString);
            data.Add("id", id);

            Request request = makeRequest(requestContext, trimData(data));

            UserController controller = new UserController();

            return controller.Call(request);
        }

        public Response<Media> getMedia(string id)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("authorization", authString);
            data.Add("id", id);

            Request request = makeRequest(requestContext, trimData(data));

            MediaController controller = new MediaController();

            return controller.Call(request);
        }

        public Response<Media> getMedias(string tag, string mediaCategoryFilter, string nameFilter, string page, string limit)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("authorization", authString);
            data.Add("tag", tag);
            data.Add("mediaCategory", mediaCategoryFilter);
            data.Add("title", nameFilter);
            data.Add("page", page);
            data.Add("limit", limit);

            Request request = makeRequest(requestContext, trimData(data));

            MediaController controller = new MediaController();

            return controller.Call(request);
        }

        public Response<Media> postMedia(Media media)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("authorization", authString);
            data.Add("description", media.description);
            data.Add("fileLocation", media.fileLocation);
            data.Add("format", media.format);
            data.Add("mediaCategory", media.mediaCategory.ToString());
            data.Add("mediaLength", media.mediaLength.ToString());
            data.Add("title", media.title);
            data.Add("user", media.user.ToString());

            Request request = makeRequest(requestContext, trimData(data));

            MediaController controller = new MediaController();

            return controller.Call(request);
        }

        public Response<Media> putMedia(Media media, string id)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("authorization", authString);
            data.Add("description", media.description);
            data.Add("fileLocation", media.fileLocation);
            data.Add("mediaCategory", media.mediaCategory.ToString());
            data.Add("mediaLength", media.mediaLength.ToString());
            data.Add("title", media.title);

            Request request = makeRequest(requestContext, trimData(data));

            MediaController controller = new MediaController();

            return controller.Call(request);
        }

        public Response<Media> deleteMedia(string id)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("authorization", authString);
            data.Add("id", id);

            Request request = makeRequest(requestContext, trimData(data));

            MediaController controller = new MediaController();

            return controller.Call(request);
        }

        public string insertMediaFile(Stream file, string id)
        {
            throw new NotImplementedException();
        }

        public Response<MediaCategory> getMediaCategories()
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("authorization", authString);

            Request request = makeRequest(requestContext, trimData(data));

            MediaCategoryController controller = new MediaCategoryController();

            return controller.Call(request);
        }

        public Response<MediaCategory> getMediaCategory(string id)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("authorization", authString);
            data.Add("id", id);

            Request request = makeRequest(requestContext, trimData(data));

            MediaCategoryController controller = new MediaCategoryController();

            return controller.Call(request);
        }

        public Response<MediaCategory> postMediaCategory(MediaCategory mediaCategory)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("authorization", authString);
            data.Add("description", mediaCategory.description);
            data.Add("name", mediaCategory.name);

            Request request = makeRequest(requestContext, trimData(data));

            MediaCategoryController controller = new MediaCategoryController();

            return controller.Call(request);
        }

        public Response<MediaCategory> putMediaCategory(string id, MediaCategory mediaCategory)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("authorization", authString);
            data.Add("description", mediaCategory.description);
            data.Add("name", mediaCategory.name);
            data.Add("id", id);

            Request request = makeRequest(requestContext, trimData(data));

            MediaCategoryController controller = new MediaCategoryController();

            return controller.Call(request);
        }

        public Response<MediaCategory> deleteMediaCategory(string id)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("authorization", authString);
            data.Add("id", id);

            Request request = makeRequest(requestContext, trimData(data));

            MediaCategoryController controller = new MediaCategoryController();

            return controller.Call(request);
        }

        public Response<Tag> getTags(string tagGroupFilter, string limit, string page)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("authorization", authString);
            data.Add("tagGroup", tagGroupFilter);
            data.Add("limit", limit);
            data.Add("page", page);

            Request request = makeRequest(requestContext, trimData(data));

            TagController controller = new TagController();

            return controller.Call(request);
        }

        public Response<Tag> getTag(string id)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("authorization", authString);
            data.Add("id", id);

            Request request = makeRequest(requestContext, trimData(data));

            TagController controller = new TagController();

            return controller.Call(request);
        }

        public Response<Tag> postTag(Tag tag)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("authorization", authString);
            data.Add("name", tag.name);
            data.Add("simpleName", tag.simple_name);
            data.Add("tagGroup", tag.tag_group.ToString());

            Request request = makeRequest(requestContext, trimData(data));

            TagController controller = new TagController();

            return controller.Call(request);
        }

        public Response<Tag> putTag(string id, Tag tag)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("authorization", authString);
            data.Add("name", tag.name);
            data.Add("simpleName", tag.simple_name);
            data.Add("tagGroup", tag.tag_group.ToString());
            data.Add("id", id);

            Request request = makeRequest(requestContext, trimData(data));

            TagController controller = new TagController();

            return controller.Call(request);
        }

        public Response<Tag> deleteTag(string id)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("authorization", authString);
            data.Add("id", id);

            Request request = makeRequest(requestContext, trimData(data));

            TagController controller = new TagController();

            return controller.Call(request);
        }

        public Response<TagGroup> getTagGroup(string id)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("authorization", authString);
            data.Add("id", id);

            Request request = makeRequest(requestContext, trimData(data));

            TagGroupController controller = new TagGroupController();

            return controller.Call(request);
        }

        public Response<TagGroup> getTagGroups(string limit, string page)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("authorization", authString);
            data.Add("limit", limit);
            data.Add("page", page);

            Request request = makeRequest(requestContext, trimData(data));

            TagGroupController controller = new TagGroupController();

            return controller.Call(request);
        }

        public Response<TagGroup> postTagGroup(TagGroup tagGroup)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("authorization", authString);
            data.Add("name", tagGroup.name);

            Request request = makeRequest(requestContext, trimData(data));

            TagGroupController controller = new TagGroupController();

            return controller.Call(request);
        }

        public Response<TagGroup> putTagGroup(string id, TagGroup tagGroup)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("authorization", authString);
            data.Add("id", id);
            data.Add("name", tagGroup.name);

            Request request = makeRequest(requestContext, trimData(data));

            TagGroupController controller = new TagGroupController();

            return controller.Call(request);
        }

        public Response<TagGroup> deleteTagGroup(string id)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("authorization", authString);
            data.Add("id", id);

            Request request = makeRequest(requestContext, trimData(data));

            TagGroupController controller = new TagGroupController();

            return controller.Call(request);
        }

        public Response<Tag> getTagByMedia(string mediaId)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("authorization", authString);
            data.Add("id", mediaId);

            Request request = makeRequest(requestContext, trimData(data));

            TagController controller = new TagController();

            return controller.Call(request);
        }

        public Response<Media> mediaHasTag(string mediaId, string tagId)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("authorization", authString);
            data.Add("mediaId", mediaId);
            data.Add("tagId", tagId);

            Request request = makeRequest(requestContext, trimData(data));

            MediaController controller = new MediaController();

            return controller.Call(request);
        }

        public Response<Rating> getRating(string mediaId, string userId, string limit, string page)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("authorization", authString);
            data.Add("mediaId", mediaId);
            data.Add("userId", userId);
            data.Add("limit", limit);
            data.Add("page", page);

            Request request = makeRequest(requestContext, trimData(data));

            RatingController controller = new RatingController();

            return controller.Call(request);
        }

        public Response<Rating> postRating(Rating rating)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("authorization", authString);
            data.Add("mediaId", rating.mediaId.ToString());
            data.Add("userId", rating.userId.ToString());
            data.Add("comment", rating.comment);
            data.Add("commentTitle", rating.commentTitle);
            data.Add("rating", rating.rating.ToString());

            Request request = makeRequest(requestContext, trimData(data));

            RatingController controller = new RatingController();

            return controller.Call(request);
        }

        public Response<Rating> putRating(string id, Rating rating)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("authorization", authString);
            data.Add("id", id);
            data.Add("comment", rating.comment);
            data.Add("commentTitle", rating.commentTitle);
            data.Add("rating", rating.rating.ToString());

            Request request = makeRequest(requestContext, trimData(data));

            RatingController controller = new RatingController();

            return controller.Call(request);
        }

        public Response<Rating> deleteRating(string id)
        {
            IncomingWebRequestContext requestContext = WebOperationContext.Current.IncomingRequest;

            Dictionary<string, string> data = new Dictionary<string, string>();

            string authString = requestContext.Headers[HttpRequestHeader.Authorization];

            data.Add("authorization", authString);
            data.Add("id", id);

            Request request = makeRequest(requestContext, trimData(data));

            RatingController controller = new RatingController();

            return controller.Call(request);
        }
    }
}
