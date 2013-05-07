using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using RestService.IO_Messages;

namespace RestService.Security
{
    public class Authenticator
    {
        private static SHA256CryptoServiceProvider shaHashProvider = new SHA256CryptoServiceProvider();
        private static UnicodeEncoding byteConverter = new UnicodeEncoding();

        private static bool VerifyHash(Request request)
        {
            /**
             * Setting up the variables.
             * The initialization here is neccessary due to the semantics of the
             * TryGetValue method of the dictionary.
             * 
             * verificationString is the string that is going to be hashed.
             * verificationHash is the hashed value of the verificationString.
             * requestHash is the hashed value in the request.
             * 
             * The method will return false if it at any point is unable to verify
             * the hash. This can be due to the hash not being present in the request,
             * or the requestHash not matching the verificationHash.
             */
            Dictionary<string, string> requestData = request.data;
            string verificationString = "";
            string verificationHash = "";
            string requestHash = "";

            if (!requestData.TryGetValue("autherization", out requestHash))
            { return false; }
            
            foreach (KeyValuePair<string, string> dataPair in requestData)
            {
                if (dataPair.Key.Equals("autherization"))
                { continue; }
                else
                { verificationString += dataPair.Value + "|"; }
            }

            string hashKey = "";
            if (requestData.TryGetValue("secret", out hashKey))
            {
                string secretKey = "";
                if (testSecretKeys.TryGetValue(hashKey, out secretKey))
                { verificationString += secretKey; }
            }

            verificationHash = byteConverter.GetString(
                shaHashProvider.ComputeHash(byteConverter.GetBytes(verificationString))
                );

            if (verificationHash.Equals(requestHash))   { return true; }
            return false;
             
        }
    }
}