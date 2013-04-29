using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestService.IO_Messages;

namespace RestService.Controllers
{
    public class RatingController : AbstractController<Rating>
    {
        public override Response<Rating> Call(Request request)
        {
            throw new NotImplementedException();
        }
    }
}