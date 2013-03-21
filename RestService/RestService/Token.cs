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
        string token;

        [DataMember]
        DateTime issued;

        [DataMember]
        long lifetime;
    }
}