using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace RestService
{
    public class UserHandler : AbstractHandler
    {
        public static User createUser(int id, string email, string password, int[] userData)
        {
            DatabaseConnection db = DatabaseConnection.GetInstance;
            SqlCommand s = db.Prepare("INSERT INTO user (name, email, password) VALUES (@name, @email, @password)");
            db.Command(new Dictionary<string,string>{
                {"email", email},
                {"password", password}
            }, s, "SMU");

            return new User(id,email,password, userData);
        }



        public override void Create(Dictionary<string, string> data)
        {
            throw new NotImplementedException();
        }

        public override void Read(int id)
        {
            throw new NotImplementedException();
        }

        public override void Update(int id, Dictionary<string, string> data)
        {
            throw new NotImplementedException();
        }

        public override void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}