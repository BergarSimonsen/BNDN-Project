using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RestService.Entities
{
    [DataContract]
    public class User : IEntities
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string email { get; set; }

        [DataMember]
        public string password { get; set; }

        [DataMember]
        public User_Data[] userData { get; set; }

        public User(int id, string email, string password, User_Data[] userData)
        {
            this.id = id;
            this.password = password;
            this.email = email;
            this.userData = userData;
        }
    }
}