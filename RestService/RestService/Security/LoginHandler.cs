using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestService.IO_Messages;

namespace RestService.Security
{
    public class LoginHandler
    {
        public static Request Login(string email, string password)
        {
            //Insert getting user logic here.
            string userPassword = "somestring";
            
            if (password.Equals(userPassword))
            {
                return true;
            }
            
            return false;
        }

    }
}