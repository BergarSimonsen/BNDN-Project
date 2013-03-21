using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RestService
{
    [DataContract]
    public class Token
    {
        [DataMember]
        public string token;

        [DataMember]
        public DateTime issued;

        [DataMember]
        public DateTime expires;

        public Token(string token, DateTime issued, DateTime expires)
        {
            this.issued = issued;
            this.token = token;
            this.expires = expires;
        }
    }
}