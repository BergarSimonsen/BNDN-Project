using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RestService
{
    [DataContract]
    public class Rating
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public int userId { get; set; }

        [DataMember]
        public int mediaId { get; set; }

        [DataMember]
        public short rating { get; set; }

        [DataMember]
        public string commentTitle { get; set; }

        [DataMember]
        public string comment { get; set; }

        public Rating(int id, int userId, int mediaId, short rating, string commentTitle, string comment)
        {
            this.id = id;
            this.userId = userId;
            this.mediaId = mediaId;
            this.rating = rating;
            this.commentTitle = commentTitle;
            this.comment = comment;
        }
    }
}