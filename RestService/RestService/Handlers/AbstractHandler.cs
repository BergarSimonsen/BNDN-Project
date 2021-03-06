﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using RestService.Security;
using RestService.Entities;

namespace RestService.Handlers
{
    public abstract class AbstractHandler<T> where T : IEntities
    {
        public DatabaseConnection dbCon;
        public Permissions permission;

        public AbstractHandler(DatabaseConnection incDbCon, Permissions perm)
        {
            this.dbCon = incDbCon;
            this.permission = perm;
        }

        public abstract void Create(Dictionary<string, string> data);

        public abstract T[] Read(int id);

        public abstract void Update(int id, Dictionary<string, string> data);

        public abstract void Delete(int id);

        public abstract T[] Search(Dictionary<string, string> data);

        public abstract void Validate(Dictionary<string, string> data);
    }
}
