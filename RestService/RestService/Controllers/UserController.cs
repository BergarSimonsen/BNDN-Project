using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestService.IO_Messages;
using RestService.Entities;
using RestService.Security;

namespace RestService.Controllers
{
    public class UserController : AbstractController<User>
    {
        public override Response<User> Call(Request request)
        {
            User[] users = null;
            int errorCode = 0;
            string message = "Your call was succesful";
            Dictionary<string, string> metaData = new Dictionary<string, string>();
            Response<User> response = null;

            try
            {
                Request newRequest = renderAndValidateRequest(request);

                DatabaseConnection db = new DatabaseConnection("SMU");

                //Permissions permissions = getPermissions(request.user, db);
                Permissions permissions = null;

                UserHandler handler = new UserHandler(db, permissions);

                switch (request.method)
                {
                    case Web_Service.RestMethods.GET:
                        if (request.uri.Count > 1)
                        {
                            if (request.data.ContainsKey("id"))
                            {
                                users = handler.Read(int.Parse(request.data["id"]));
                            }
                            else
                            {
                                users = new User[] { request.user };
                            }
                        }
                        else if (request.uri.Count < 2)
                        {
                            users = handler.Search(request.data);
                        }
                        break;
                    case Web_Service.RestMethods.POST:
                        handler.Create(request.data);
                        break;
                    case Web_Service.RestMethods.PUT:
                        handler.Update(int.Parse(request.data["id"]), request.data);
                        break;
                    case Web_Service.RestMethods.DELETE:
                        handler.Delete(int.Parse(request.data["id"]));
                        break;
                    default:
                        break;
                }
            }
            catch(Exception ex)
            {
                errorCode = 001;
                message = ex.Message;
            }
            finally
            {
                if (metaData.Count == 0)
                {
                    metaData = null;
                }

                response = createResponse(users, errorCode, message, metaData);
            }

            return response;
        }
    }
}