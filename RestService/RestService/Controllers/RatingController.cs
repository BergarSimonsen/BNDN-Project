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
    public class RatingController : AbstractController<Rating>
    {
        public override Response<Rating> Call(Request request)
        {
            Rating[] ratings = null;
            int errorCode = 0;
            string message = "Your call was succesful";
            Dictionary<string, string> metaData = new Dictionary<string, string>();
            Response<Rating> response = null;

            try
            {
                request = renderAndValidateRequest(request);

                DatabaseConnection db = new DatabaseConnection("SMU");

                Permissions permission = null;

                RatingHandler handler = new RatingHandler(db, permission);

                switch (request.method)
                {
                    case Web_Service.RestMethods.GET:
                        if (request.uri.Count > 1)
                        {
                            if (request.data.ContainsKey("id"))
                            {
                                ratings = handler.Read(int.Parse(request.data["id"]));
                            }
                            else
                            {   
                                // Mangler.. Not sure?!
                            }
                        }
                        else if (request.uri.Count < 2)
                        {
                            ratings = handler.Search(request.data);
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

                response = createResponse(ratings, errorCode, message, metaData);
            }
            return response;
        }
    }
}