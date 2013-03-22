using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RestService
{
    [DataContract]
    public class MediaCategory
    {
        [DataMember]
        public int id;

        [DataMember]
        public string name;

        public MediaCategory(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}