using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestService.Entities;
using RestService.IO_Messages;

namespace RestService.Controllers
{
    public class ActionController : AbstractController<Entities.Action>
    {
        public override Response<Entities.Action> Call(Request request)
        {
            throw new NotImplementedException();
        }
    }
}