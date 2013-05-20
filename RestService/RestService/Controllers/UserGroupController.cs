using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestService.Entities;
using RestService.IO_Messages;
using RestService.Security;
using RestService.Handlers;

namespace RestService.Controllers
{
    public class UserGroupController : AbstractController<UserGroup>
    {
        public override Response<UserGroup> Call(Request request)
        {
            UserGroup[] userGroups = null;
            int errorCode = 0;
            string message = "Your call was succesful";
            Dictionary<string, string> metaData = new Dictionary<string, string>();
            Response<UserGroup> response = null;

            try
            {
                request = renderAndValidateRequest(request);

                DatabaseConnection db = new DatabaseConnection("SMU");

                //Permissions permissions = getPermissions(request.user, db);
                Permissions permissions = null;

                UserGroupHandler handler = new UserGroupHandler(db, permissions);

                switch (request.method)
                {
                    case Web_Service.RestMethods.GET:
                        if (request.uri.Count > 1)
                        {
                            userGroups = handler.Read(int.Parse(request.data["id"]));
                        }
                        break;
                    case Web_Service.RestMethods.POST:
                        handler.Create(request.data);
                        request.data["limit"] = "1";
                        request.data["page"] = "1";
                        userGroups = handler.Search(request.data);
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
            catch (Exception ex)
            {
                if (ex.Data.Contains("errorCode"))
                {
                    response.errorCode = (int)ex.Data["errorCode"];
                    response.message = ex.Message;
                }
                else
                {
                    errorCode = 001;
                    message = ex.Message;
                }
            }
            finally
            {
                if (metaData.Count == 0)
                {
                    metaData = null;
                }

                response = createResponse(userGroups, errorCode, message, metaData);
            }

            return response;
        }
    }
}