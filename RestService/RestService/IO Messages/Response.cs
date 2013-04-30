using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using RestService.Entities;

namespace RestService.IO_Messages
{
    [DataContract]
    public class Response
    {
        [DataMember]
        public List<AbstractEntity> data;

        [DataMember]
        public Dictionary<string, string> metaData;

        [DataMember]
        public string errorMessage;

        [DataMember]
        public int errorCode;

        public Response(List<AbstractEntity> data, Dictionary<string,string> metaData, string errorMessage, int errorCode)
        {

        }
    }
}