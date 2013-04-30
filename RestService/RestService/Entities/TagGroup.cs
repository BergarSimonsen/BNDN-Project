using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace RestService.Entities
{
    [DataContract]
    public class TagGroup : IEntities
    {
        [DataMember]
        public int id;

        [DataMember]
        public string name;

        public TagGroup(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
