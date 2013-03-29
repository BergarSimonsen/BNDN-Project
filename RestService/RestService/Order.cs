using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RestService
{
    [DataContract]
    public class Order
    {
        [DataMember]
        public int id;

        [DataMember]
        public string title;

        [DataMember]
        public int user_id;

        [DataMember]
        public string status;

        public Order(int id, string title, int user_id, string status)
        {
            this.id = id;
            this.title = title;
            this.user_id = user_id;
            this.status = status;
        }
    }
}