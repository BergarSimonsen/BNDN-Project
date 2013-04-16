using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RestService
{
    public abstract class AbstractHandler
    {

        DatabaseConnection dbCon;
        //Permission permission;

        public AbstractHandler( DatabaseConnection incDbCon/*, Permission perm*/)
        {
            dbCon = incDbCon;
            //permission = perm;
        }

        public abstract void Create(Dictionary<string, string> data){ }

        public abstract void Read(int id) { }

        public abstract void Update(int id, Dictionary<string, string> data) { }

        public abstract void Delete(int id) { }
    }
}
