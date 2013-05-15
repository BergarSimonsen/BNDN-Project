using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestService.Entities;
using RestService.IO_Messages;

namespace RestService.Controllers
{
    public class UserDataController : AbstractController<User_Data>
    {
        public override Response<User_Data> Call(Request request)
        {
            throw new NotImplementedException();
        }
    }
}