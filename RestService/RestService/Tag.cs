using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace RestService
{
    [DataContract]
    public class Tag
    {
        [DataMember]
        public int id;

        [DataMember]
        public int tag_group;

        [DataMember]
        public string name;

        [DataMember]
        public string simple_name;

        public Tag(int id, int tag_group, string name, string simple_name)
        {
            this.id = id;
            this.name = name;
            this.tag_group = tag_group;
            this.simple_name = simple_name;
        }
    }
}
