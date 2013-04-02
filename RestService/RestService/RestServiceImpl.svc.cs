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
using System.Web.Script.Serialization;

namespace RestService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RestServiceImpl" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select RestServiceImpl.svc or RestServiceImpl.svc.cs at the Solution Explorer and start debugging.
    public class RestServiceImpl : IRestServiceImpl
    {
        public User getUser(string id)
        {
            DatabaseConnector database = DatabaseConnector.GetInstance;

            return database.getUser(int.Parse(id));
        }

        public UserList getUsersWithParameter(string group_id, string search_string, string search_fields, string limit, string page, string order_by, string order)
        {
            int groupId = 0;
            if (group_id != null)
            {
                groupId = int.Parse(group_id);
            }
            string seachString = null;
            if (search_string != null)
            {
                seachString = search_string;
            }
            string searchField = null;
            if (search_fields != null)
            {
                searchField = search_fields;
            }
            string orderBy = "email";
            if (order_by != null)
            {
                orderBy = order_by;
            }
            string orders = "ASC";
            if (order != null)
            {
                orders = order;
            }

            DatabaseConnector database = DatabaseConnector.GetInstance;
            User[] users = database.getUsers(groupId, seachString, searchField, orderBy, orders);

            int pageLimit = users.Length;
            if (limit != null)
            {
                pageLimit = int.Parse(limit);
            }
            int pageCount = (users.Length + pageLimit - 1) / pageLimit;

            int pageNumber = 1;
            if (page != null)
            {
                pageNumber = int.Parse(page);
            }

            User[] returnUsers = new User[pageLimit];
            int pageCounter = 1;
            int userCounter = 0;
            for (int i = 0; i < users.Length; i++)
            {
                if (pageCounter == pageNumber)
                {
                    returnUsers[userCounter] = users[i];
                    userCounter++;
                }
                if ((i+1) % pageLimit == 0)
                {
                    pageCounter++;
                }
            }

            return new UserList(users.Length, pageCount, returnUsers);     
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

        public void insertMediaFile(Stream file)
        {
            StreamReader reader = new StreamReader(file);
            string fileContent = reader.ReadToEnd();
            reader.Close();

            StreamWriter writer = new StreamWriter(@"C:\Users\christian\Documents\RentItTest\derp.mkv");
            writer.Write(fileContent);
            writer.Close();
        }
    }
}
