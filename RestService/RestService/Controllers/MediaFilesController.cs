using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using RestService.Entities;
using RestService.HelperObjects;
using RestService.IO_Messages;
using RestService.Security;

namespace RestService.Controllers
{
    public class MediaFilesController : AbstractController<Media>
    {
        public override Response<Media> Call(Request request)
        {
            throw new NotImplementedException();
        }

        public Response<Media> InsertMediaFile(Request request, int id, Stream file)
        {
            int errorCode = 0;
            string message = "Your call was succesful";
            Dictionary<string, string> metaData = new Dictionary<string, string>();
            Response<Media> response = null;
            try
            {
                request = renderAndValidateRequest(request);

                DatabaseConnection db = new DatabaseConnection("SMU");

                Permissions per = null;

                MediaFilesHandler.insertMediaFile(db, file, id, per);
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

                response = createResponse(null, errorCode, message, metaData);
            }

            return response;
        }
    }
}