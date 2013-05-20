using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestService.Entities;
using RestService.IO_Messages;
using RestService.Security;

namespace RestService.Controllers
{
    public class TokenController : AbstractController<Token>
    {
        public override Response<Token> Call(Request request)
        {
            Token[] token = null;
            int errorCode = 0;
            string message = "Your call was succesful";

            try
            {
                token = new Token[] { TokenHandler.getToken(request.data["email"], request.data["password"]) };
            }
            catch(Exception ex)
            {
                errorCode = 001;
                message = ex.Message;
            }

            return createResponse(token, errorCode, message, null);
        }
    }
}