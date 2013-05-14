using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RestService.Entities
{
    [DataContract]
    public class User_Data : IEntities
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public int userId { get; set; }

        [DataMember]
        public string userDataType { get; set; }

        [DataMember]
        public string value { get; set; }

        public User_Data(int id, int userId, string dataType, string value)
        {
            this.id = id;
            this.userId = userId;
            this.userDataType = dataType;
            this.value = value;
        }
    }
}