using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using RestService.Entities;

namespace RestService.IO_Messages
{
    [DataContract]
    public class Response<T>
    {
        [DataMember]
        public T[] data;

        [DataMember]
        public Dictionary<string, string> metaData;

        [DataMember]
        public string errorMessage;

        [DataMember]
        public int errorCode;

        public Response(T[] data, Dictionary<string,string> metaData, string errorMessage, int errorCode)
        {
            this.data = data;
            this.metaData = metaData;
            this.errorMessage = errorMessage;
            this.errorCode = errorCode;
        }
    }
}