using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RestService
{
    [DataContract]
    public class OrderLine
    {
        [DataMember]
        public int id;

        [DataMember]
        public string title;

        [DataMember]
        public string text;

        [DataMember]
        public float price;

        [DataMember]
        public int order_id;

        public OrderLine(int id, string title, string text, float price, int order_id)
        {
            this.id = id;
            this.title = title;
            this.text = text;
            this.price = price;
            this.order_id = order_id;
        }
    }
}