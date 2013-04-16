using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RestService.Entities
{
    [DataContract]
    public abstract class AbstractEntity
    {
        [DataMember]
        public int id { get; set; }
    }
}