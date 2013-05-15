using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestService.Entities;
using RestService.IO_Messages;

namespace RestService.Controllers
{
    public class TokenController : AbstractController<Token>
    {
        public override Response<Token> Call(Request request)
        {
            return new Response<Token>(new Token[] {new Token("DILLZHUNTER")},null,"All Good",0);
        }
    }
}