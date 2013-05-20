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
    public class ActionController : AbstractController<Entities.Action>
    {
        public override Response<Entities.Action> Call(Request request)
        {
            Entities.Action[] actions = null;
            int errorCode = 0;
            string message = "Your call was succesful";
            Dictionary<string, string> metaData = new Dictionary<string, string>();
            Response<Entities.Action> response = null;

            try
            {
                request = renderAndValidateRequest(request);

                DatabaseConnection db = new DatabaseConnection("SMU");

                //Permissions permissions = getPermissions(request.user, db);
                Permissions permissions = null;

                ActionHandler handler = new ActionHandler(db, permissions);

                switch (request.method)
                {
                    case Web_Service.RestMethods.GET:
                        if (request.uri.Count > 1)
                        {
                            actions = handler.Read(int.Parse(request.data["id"]));
                        }
                        break;
                    case Web_Service.RestMethods.POST:
                        handler.Create(request.data);
                        request.data["limit"] = "1";
                        request.data["page"] = "1";
                        actions = handler.Search(request.data);
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

                response = createResponse(actions, errorCode, message, metaData);
            }

            return response;
        }
    }
}