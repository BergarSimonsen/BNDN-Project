using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace RestService.Security
{
    public class SHAEncrypter
    {   
        private static UnicodeEncoding ByteConverter = new UnicodeEncoding();
        private static SHA256CryptoServiceProvider SHACrypto = new SHA256CryptoServiceProvider();

        public static string SHAEncrypt(string target)
        {
            return ByteConverter.GetString(
                SHACrypto.ComputeHash(ByteConverter.GetBytes(target))
                );
        }
    }
}