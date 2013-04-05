using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RestService
{
    public class UserHandler
    {
        public static User createUser(int id, string email, string password, int[] userData)
        {
            return new User(id,email,password, userData);
        }
    }
}