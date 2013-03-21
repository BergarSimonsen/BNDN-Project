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

        public User getLoggedUser(string token)
        {
            return new User(1337,null,null);
        }

        public Token getToken(string email, string password)
        {
            DateTime from = DateTime.Now;
            DateTime to = new DateTime(2013,4,1);

            return new Token("HFjdje33hdHS", from, to);
        }
    }
}
