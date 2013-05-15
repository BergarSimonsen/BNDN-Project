using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace RestService
{
    public class AuthHandler
    {
        private static Dictionary<string, UserTokenData> UserTokenMap = new Dictionary<string, UserTokenData>();

        public static int AddUserToMap(string token, UserTokenData user)
        {
            try
            {
                if (!UserTokenMap.ContainsKey(token))
                {
                    UserTokenMap.Add(token, user);
                    return 0;
                }
                else
                {
                    UserTokenMap.Remove(token);
                    UserTokenMap.Add(token, user);
                    return 1;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return -1;
            }
        }

        public static UserTokenData GetUserFromMap(string token)
        {
            UserTokenData value;
            if (UserTokenMap.TryGetValue(token, out value))
            { return value; }
            else
            {
                return null;
            }
        }

        private class TokenHandler
        {
            private readonly long tokenLifetime = 900000000000;
            private static TokenHandler instance;
            public static TokenHandler GetInstance()
            {
                if (instance == null) { instance = new TokenHandler(); }
                return instance;
            }

            public string CreateUserToken(string email)
            {
                CryptoHandler crypto = AuthHandler.CryptoHandler.GetInstance();
                DateTime now = DateTime.UtcNow.Add(new TimeSpan(tokenLifetime));
                string encryptedTarget = crypto.RSAEncrypt(
                    email + "|" + now.ToString(),
                    crypto.GetKey().ExportParameters(true)
                    );
                AuthHandler.AddUserToMap(encryptedTarget, new UserTokenData(email, now)); 

                return encryptedTarget;
            }

            public string ReturnUser(string token)
            {
                try
                {
                    UserTokenData userToken = AuthHandler.GetUserFromMap(token);
                    if (userToken.expires.Ticks > DateTime.UtcNow.Ticks)
                    { return userToken.email; }
                    else
                    { return null; }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    return null;
                }
            }
        }

        private class CryptoHandler
        {
            private static CryptoHandler instance;
            public static CryptoHandler GetInstance()
            {
                if (instance == null) { instance = new CryptoHandler(); }
                return instance;
            }

            private UnicodeEncoding ByteConverter = new UnicodeEncoding();

            public RSACryptoServiceProvider GetKey()
            { return new RSACryptoServiceProvider(); }

            public string RSAEncrypt(string target, RSAParameters RSAKeyInfo)
            {
                byte[] byteTarget = ByteConverter.GetBytes(target);
                return ByteConverter.GetString(RSAEncrypt(byteTarget, RSAKeyInfo, false));
            }

            private byte[] RSAEncrypt(byte[] target, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
            {
                try
                {
                    byte[] encryptedData;
                    using (RSACryptoServiceProvider crypto = new RSACryptoServiceProvider())
                    {
                        crypto.ImportParameters(RSAKeyInfo);

                        encryptedData = crypto.Encrypt(target, DoOAEPPadding);
                    }
                    return encryptedData;
                }
                catch (CryptographicException e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }

            public string RSADecrypt(string target, RSAParameters RSAKeyInfo)
            {
                byte[] byteTarget = ByteConverter.GetBytes(target);
                return ByteConverter.GetString(RSADecrypt(byteTarget, RSAKeyInfo, false));
            }
            
            private byte[] RSADecrypt(byte[] target, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
            {
                try
                {
                    byte[] decryptedData;
                    using (RSACryptoServiceProvider crypto = new RSACryptoServiceProvider())
                    {
                        crypto.ImportParameters(RSAKeyInfo);

                        decryptedData = crypto.Decrypt(target, DoOAEPPadding);
                    }
                    return decryptedData;
                }
                catch (CryptographicException e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }
        }

        public class UserTokenData
        {
            public string email;
            public DateTime expires;

            public UserTokenData(string email, DateTime expires)
            {
                this.email = email;
                this.expires = expires;
            }
        }
    }
}