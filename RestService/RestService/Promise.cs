using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Web;

namespace RestService
{
    [DataContract]
    public class Promise
    {
        [DataMember]
        public int id;

        [DataMember]
        public int action_id;

        [DataMember]
        public DateTime start;

        [DataMember]
        public int hours;

        [DataMember]
        public int order_line_id;

        public Promise(int id, int action_id, DateTime start, int hours, int order_line_id)
        {
            this.id = id;
            this.action_id = action_id;
            this.start = start;
            this.hours = hours;
            this.order_line_id = order_line_id;
        }
    }
}