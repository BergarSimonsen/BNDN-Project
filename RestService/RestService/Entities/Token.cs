using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RestService.Entities
{
    [DataContract]
    public class Token : IEntities
    {
        [DataMember]
        public string token { get; set; }

        public Token(string token)
        {
            this.token = token;
        }
    }
}