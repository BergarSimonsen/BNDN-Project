using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RestService
{
    [DataContract]
    public class Media
    {
        [DataMember]
        public int id;

        [DataMember]
        public int mediaCategory;

        [DataMember]
        public int user;

        [DataMember]
        public string fileLocation;

        [DataMember]
        public string title;

        [DataMember]
        public string description;

        [DataMember]
        public int mediaLength;

        [DataMember]
        public string format;

        [DataMember]
        public int[] tags;

        public Media(int id, int mediaCategory, int user, string fileLocation, string title, string description, int mediaLength, string format, int[] tags)
        {
            this.id = id;
            this.mediaCategory = mediaCategory;
            this.user = user;
            this.fileLocation = fileLocation;
            this.title = title;
            this.description = description;
            this.mediaLength = mediaLength;
            this.format = format;
            this.tags = tags;
        }
    }
}