using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using RestService.Entities;

namespace RestService
{
    [DataContract]
    public class UserList
    {
        [DataMember]
        public int count;

        [DataMember]
        public int pageCount;

        [DataMember]
        public User[] users;

        public UserList(int count, int pageCount, User[] users)
        {
            this.count = count;
            this.pageCount = pageCount;
            this.users = users;
        }
    }
}