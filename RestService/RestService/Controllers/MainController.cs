using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestService.IO_Messages;
using RestService.Web_Service;

namespace RestService.Controllers
{
    public class MainController : IController
    {
        public Response Call(Request request) 
        {
            request.user = null;

            if (request.uri.StartsWith("/user"))
            {
                IController controller = new UserController();

                return controller.Call(request);
            }
        } 
    }
}