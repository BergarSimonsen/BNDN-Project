using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace RestService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RestServiceImpl" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select RestServiceImpl.svc or RestServiceImpl.svc.cs at the Solution Explorer and start debugging.
    public class RestServiceImpl : IRestServiceImpl
    {
        public User getUser(string id)
        {
            User user = new User();
            user.id = int.Parse(id);
            return user;
        }

        public User[] getUsersWithParameter(string group_id, string search_string, string search_fields)
        {
            User[] users = new User[3];
            User user1 = new User();
            User user2 = new User();
            User user3 = new User();
            user1.email = "hej";
            user2.email = "hej";
            user3.email = "derp";
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

        public User getLoggedUser(string token)
        {
            User user = new User();

            user.id = 1337;

            return user;
        }


    }
}
