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
        public string email;

        [DataMember]
        public DateTime expires;

        public Token(string token, string email, DateTime expires)
        {
            this.token = token;
            this.email = email;
            this.expires = expires;
        }
    }
}