using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RestService
{
    class MediaHandler
    {
        public static Media createMedia(int id, int mediaCategory, int user, string fileLocation, string title, string description, int mediaLength, string format)
        {
            return new Media(id, mediaCategory, user, fileLocation, title, description, mediaLength, format);
        }
    }
}
