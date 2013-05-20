using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestService.IO_Messages;
using RestService.Entities;
using RestService.Security;
using RestService.Handlers;

namespace RestService.Controllers
{
    public class MediaCategoryController : AbstractController<MediaCategory>
    {
        public override Response<MediaCategory> Call(Request request)
        {
            MediaCategory[] mediaCategories = null;
            int errorCode = 0;
            string message = "Your call was succesful";
            Dictionary<string, string> metaData = new Dictionary<string, string>();
            Response<MediaCategory> response = null;

            try
            {
                request = renderAndValidateRequest(request);

                DatabaseConnection db = new DatabaseConnection("SMU");

                //Permissions permissions = getPermissions(request.user, db);
                Permissions permissions = null;

                MediaCategoryHandler handler = new MediaCategoryHandler(db, permissions);

                switch (request.method)
                {
                    case Web_Service.RestMethods.GET:
                        if (request.uri.Count > 1)
                        {
                            mediaCategories = handler.Read(int.Parse(request.data["id"]));
                        }
                        else if (request.uri.Count < 2)
                        {
                            mediaCategories = handler.Search(request.data);
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

                response = createResponse(mediaCategories, errorCode, message, metaData);
            }

            return response;
        }
    }
}