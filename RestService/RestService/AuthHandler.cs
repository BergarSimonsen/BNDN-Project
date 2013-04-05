using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace RestService
{
    public class AuthHandler
    {
        private class TokenHandler
        {
            private readonly long tokenLifetime = 900000000000;
            private static TokenHandler instance;
            public static TokenHandler GetInstance()
            {
                if (instance == null) { instance = new TokenHandler(); }
                return instance;
            }

            public Token CreateToken(string username)
            {
                return CreateToken(username, DateTime.UtcNow.Add(new TimeSpan(tokenLifetime)));
            }

            private Token CreateToken(string email, DateTime expires)
            {
                string hash = (email.GetHashCode() * expires.GetHashCode()).ToString();
                CryptoHandler cryptoHandler = AuthHandler.CryptoHandler.GetInstance();
                return new Token(cryptoHandler.RSAEncrypt(hash, cryptoHandler.GetUserFromCrypto(email).ExportParameters(false)), email, expires);
            }

            public bool VerifyToken(string username, Token token)
            {
                if (DateTime.UtcNow.Ticks > token.expires.Ticks)
                {
                    Token verificationToken = CreateToken(username, token.expires);
                    return token.Equals(verificationToken);
                }
                else return false;
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
            
            private Dictionary<string, RSACryptoServiceProvider> userHashMap = new Dictionary<string,RSACryptoServiceProvider>();
            private UnicodeEncoding ByteConverter = new UnicodeEncoding();

            public void AddUserToCrypto(string email)
            {
                if (!userHashMap.ContainsKey(email)) { 
                    userHashMap.Add(email, new RSACryptoServiceProvider());
                } else {
                    userHashMap.Remove(email);
                    userHashMap.Add(email, new RSACryptoServiceProvider());
                }
            }

            public RSACryptoServiceProvider GetUserFromCrypto(string email)
            {
                RSACryptoServiceProvider KeyInfo;
                if (userHashMap.TryGetValue(email, out KeyInfo))
                    return KeyInfo;

                return null;
            }

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
    }
}