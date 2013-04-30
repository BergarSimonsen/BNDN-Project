using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RestService.Entities
{
    [DataContract]
    public class Action : IEntities
    {
        [DataMember]
        public int id;

        [DataMember]
        public string name;

        [DataMember]
        public string description;

        [DataMember]
        public bool allowed;

        public Action(int id, string name, string description, bool allowed)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.allowed = allowed;
        }

        public override bool Equals(object obj)
        {
            Action action = (Action) obj;

            if (this.id == action.id && this.name.Equals(action.name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}