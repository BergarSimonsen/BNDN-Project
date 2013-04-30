using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace RestService.Entities
{
    [DataContract]
    public class UserAccountTag : IEntities
    {
        [DataMember]
        public int id;

        [DataMember]
        public string name;

        public UserAccountTag(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
