using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestService.Entities;
using RestService.IO_Messages;

namespace RestService.Controllers
{
    public class UserGroupController : AbstractController<UserGroup>
    {
        public override Response<UserGroup> Call(Request request)
        {
            throw new NotImplementedException();
        }
    }
}