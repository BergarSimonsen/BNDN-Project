using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;


namespace RestService.Entities
{
    [DataContract]
    public class Media : IEntities 
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

        public Media(int id, int mediaCategory, int user, string fileLocation, string title, string description, int mediaLength, string format)
        {
            this.id = id;
            this.mediaCategory = mediaCategory;
            this.user = user;
            this.fileLocation = fileLocation;
            this.title = title;
            this.description = description;
            this.mediaLength = mediaLength;
            this.format = format;
        }
    }
}