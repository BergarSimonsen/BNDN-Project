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
        public static Request makeRequest(IncomingWebRequestContext request, Dictionary<string,string> data)
        {
            //gets the whole uri
            string pathUri = request.UriTemplateMatch.RequestUri.PathAndQuery;

            //we only want the important part. example user/23
            string[] trimmedUri = pathUri.Split("/RestServiceImpl.svc/".ToCharArray());
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
        public static Dictionary<string, string> trimData(Dictionary<string, string> data)
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
            throw new NotImplementedException();
        }

        public Response<User> getLoggedUser()
        {
            throw new NotImplementedException();
        }

        public Response<string> getToken(string email, string password)
        {
            throw new NotImplementedException();
        }

        public Response<string> renewToken()
        {
            throw new NotImplementedException();
        }

        public Response<User> updateUser(string id, string oldPassword, User newUser)
        {
            throw new NotImplementedException();
        }

        public Response<User> deleteUser(string id)
        {
            throw new NotImplementedException();
        }

        public Response<Media> getMedia(string id)
        {
            throw new NotImplementedException();
        }

        public Response<Media> getMedias(string tag, string mediaCategoryFilter, string nameFilter, string page, string limit)
        {
            throw new NotImplementedException();
        }

        public Response<int> postMedia(Media media)
        {
            throw new NotImplementedException();
        }

        public Response<string> putMedia(Media media, string id)
        {
            throw new NotImplementedException();
        }

        public Response<string> deleteMedia(string id)
        {
            throw new NotImplementedException();
        }

        public Response<string> insertMediaFile(Stream file, string id)
        {
            throw new NotImplementedException();
        }

        public Response<MediaCategory> getMediaCategories()
        {
            throw new NotImplementedException();
        }

        public Response<MediaCategory> getMediaCategory(string id)
        {
            throw new NotImplementedException();
        }

        public Response<int> postMediaCategory(MediaCategory mediaCategory)
        {
            throw new NotImplementedException();
        }

        public Response<string> putMediaCategory(string id, MediaCategory mediaCategory)
        {
            throw new NotImplementedException();
        }

        public Response<string> deleteMediaCategory(string id)
        {
            throw new NotImplementedException();
        }

        public Response<Tag> getTags(string tagGroupFilter, string limit, string page)
        {
            throw new NotImplementedException();
        }

        public Response<Tag> getTag(string id)
        {
            throw new NotImplementedException();
        }

        public Response<int> postTag(Tag tag)
        {
            throw new NotImplementedException();
        }

        public Response<string> putTag(string id, Tag tag)
        {
            throw new NotImplementedException();
        }

        public Response<string> deleteTag(string id)
        {
            throw new NotImplementedException();
        }

        public Response<TagGroup> getTagGroup(string id)
        {
            throw new NotImplementedException();
        }

        public Response<TagGroup> getTagGroups(string limit, string page)
        {
            throw new NotImplementedException();
        }

        public Response<int> postTagGroup(TagGroup tagGroup)
        {
            throw new NotImplementedException();
        }

        public Response<string> putTagGroup(string id, TagGroup tagGroup)
        {
            throw new NotImplementedException();
        }

        public Response<string> deleteTagGroup(string id)
        {
            throw new NotImplementedException();
        }

        public Response<Tag> getTagByMedia(string media)
        {
            throw new NotImplementedException();
        }

        public Response<string> mediaHasTag(string media, string tag)
        {
            throw new NotImplementedException();
        }

        public Response<Rating> getRating(string media, string user)
        {
            throw new NotImplementedException();
        }

        public Response<int> postRating(Rating rating)
        {
            throw new NotImplementedException();
        }

        public Response<string> putRating(string id, Rating rating)
        {
            throw new NotImplementedException();
        }

        public Response<string> deleteRating(string id)
        {
            throw new NotImplementedException();
        }
    }
}
