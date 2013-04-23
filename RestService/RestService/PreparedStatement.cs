using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace RestService
{
    class PreparedStatement
    {
        SqlCommand myCommand;
        int mySecret;

        public PreparedStatement(SqlCommand cmd, int secret)
        {
            myCommand = cmd;
            mySecret = secret;
        }

        public SqlCommand GetCmd() { return myCommand; }

        public bool CheckSecret(int i)
        {
            return mySecret == i;
            else return false;
        }
    }
}
