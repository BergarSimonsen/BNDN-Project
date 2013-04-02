using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
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
            return new User(int.Parse(id), null,null);
        }

        public User[] getUsersWithParameter(string group_id, string search_string, string search_fields)
        {
            User[] users = new User[3];
            User user1 = new User(1,"hej",null);
            User user2 = new User(2, "hej", null);
            User user3 = new User(3,"derp",null);
            users[0] = user1;
            users[1] = user2;
            users[2] = user3;

            User[] returnUsers = new User[3];
            for (int i = 0; i < 3; i++)
            {
                if (users[i].email == search_string)
                {
                    returnUsers[i] = users[i];
                }
            }

            return returnUsers;
        }

        public int insertUser(User user)
        {
            return user.id;
        }

        public User getLoggedUser()
        {
            WebHeaderCollection headers = WebOperationContext.Current.IncomingRequest.Headers;

            string token = headers[HttpRequestHeader.Authorization];

            return new User(1337, "derpderpdillz", token);
        }

        public Token getToken(string email, string password)
        {
            DateTime from = DateTime.Now;
            DateTime to = new DateTime(2013,4,1);

            return new Token("Token Made", from, to);
        }

        public Token renewToken()
        {
            WebHeaderCollection headers = WebOperationContext.Current.IncomingRequest.Headers;

            string token = headers[HttpRequestHeader.Authorization];

            return new Token(token +" has been renewed", DateTime.Now, new DateTime(2100, 1, 1));
        }

        public void updateUser(string id, string oldPassword, User newUser)
        {

        }

        public void deleteUser(string id)
        {

        }

        public Media getMedia(string id)
        {
            return new Media(int.Parse(id), 1, 2, "derp", "dero", "der", 32, "jgp", new int[] { 2, 3, 4, 5 });
        }

        public MediaList getMedias(string andTags, string orTags, string mediaCategoryFilter, string nameFilter, string page, string limit)
        {
            Media[] mediaList = new Media[] {new Media(2, 1, 2, "derp", "dero", "der", 32, "jgp", new int[] { 2, 3, 4, 5 }),
                new Media(3, 1, 2, "derp", "dero", "der", 32, "jgp", new int[] { 2, 3, 4, 5 }),
                new Media(4, 1, 2, "derp", "dero", "der", 32, "jgp", new int[] { 2, 3, 4, 5 }),
                new Media(5, 1, 2, "derp", "dero", "der", 32, "jgp", new int[] { 2, 3, 4, 5 }),
                new Media(5, 1, 2, "derp", "dero", "der", 32, "jgp", new int[] { 2, 3, 4, 5 })};

            int pageCount;

            if (limit != null)
            {
                pageCount = (mediaList.Length + int.Parse(limit) - 1) / int.Parse(limit);
            }
            else
            {
                pageCount = -1;
            }

            return new MediaList(pageCount, mediaList);                                
        }

        public int insertMedia(Media media)
        {
            return media.id;
        }

        public void insertMediaFile(FileStream file)
        {

        }
    }
}
