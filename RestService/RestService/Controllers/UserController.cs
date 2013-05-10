using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestService.IO_Messages;
using RestService.Entities;

namespace RestService.Controllers
{
    public class UserController : AbstractController<User>
    {
        public override Response<User> Call(Request request)
        {
            Request newRequest = renderAndValidateRequest(request);            

            List<User> data = new List<User>();

            data.Add(new User(int.Parse(request.data["id"]), "derpderpderp", "derpderpder", null));

            Dictionary<string,string> metaData = new Dictionary<string,string>();

            int i = 1;

            metaData.Add("count", i.ToString());

            return new Response<User>(data.ToArray(),metaData,null,0);
        }
    }
}