using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Web;

namespace RestService.Entities
{
    [DataContract]
    public class UserGroup : IEntities
    {
        [DataMember]
        public int id;

        [DataMember]
        public string name;

        [DataMember]
        public string description;

        public UserGroup(int id, string name, string description)
        {
            this.id = id;
            this.name = name;
            this.description = description;
        }
    }
}
