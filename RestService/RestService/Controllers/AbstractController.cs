using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestService.IO_Messages;
using RestService.Web_Service;
using RestService.Entities;

namespace RestService.Controllers
{
    public abstract class AbstractController<T>
    {
        public abstract Response<T> Call(Request request);

        protected Request renderAndValidateRequest(Request request)
        {
            return request;
        }
    }
}