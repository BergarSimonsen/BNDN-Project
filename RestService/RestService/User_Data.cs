using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RestService
{
    [DataContract]
    public class User_Data
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public int userId { get; set; }

        [DataMember]
        public int userDataTypeId { get; set; }

        [DataMember]
        public string value { get; set; }

        public User_Data(int id, int userId, int dataTypeId, string value)
        {
            this.id = id;
            this.userId = userId;
            this.userDataTypeId = dataTypeId;
            this.value = value;
        }
    }

    [DataContract]
    public class User_Data_Type
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string name { get; set; }

        public User_Data_Type(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}