using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestService.IO_Messages;
using RestService.Web_Service;
using RestService.Entities;
using RestService.Security;

namespace RestService.Controllers
{
    public abstract class AbstractController<T> where T : IEntities
    {
        public abstract Response<T> Call(Request request);

        protected Request renderAndValidateRequest(Request request)
        {
            return request;
        }

        protected Permissions getPermissions(User user, DatabaseConnection db)
        {
            return Permissions.createPermissions(user, db);
        }

        protected Response<T> createResponse(T[] data, int errorCode, string message, Dictionary<string, string> metaData)
        {
            return new Response<T>(data, metaData, message, errorCode);
        }
    }
}