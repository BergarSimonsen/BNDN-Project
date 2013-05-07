using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using RestService.Entities;

namespace RestService
{
    [DataContract]
    public class MediaList
    {
        [DataMember]
        public int pageCount;

        [DataMember]
        public Media[] medias;

        public MediaList(int pageCount, Media[] medias)
        {
            this.pageCount = pageCount;
            this.medias = medias;
        }
    }
}