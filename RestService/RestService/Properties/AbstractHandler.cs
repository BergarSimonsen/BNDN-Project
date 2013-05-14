using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using RestService.Security;

namespace RestService
{
    public abstract class AbstractHandler
    {
        public DatabaseConnection dbCon;
        public Permissions permission;

        public AbstractHandler(DatabaseConnection incDbCon, Permissions perm)
        {
            this.dbCon = incDbCon;
            this.permission = perm;
        }

        public abstract void Create(Dictionary<string, string> data);

        public abstract SqlDataReader Read(int id);

        public abstract void Update(int id, Dictionary<string, string> data);

        public abstract void Delete(int id);

        public abstract SqlDataReader Search(Dictionary<string, string> data);

        public abstract void Validate(Dictionary<string, string> data);
    }
}
