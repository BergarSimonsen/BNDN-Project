using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Web;

namespace RestService
{
    public class AuthHandler
    {
        private static readonly long tokenLifetime = 900000000000;

        public static Token CreateToken(string username)
        {
            return CreateToken(username, DateTime.UtcNow);
        }

        private static Token CreateToken(string username, DateTime issued)
        {
            return new Token(BigInteger.ModPow(new BigInteger(username.GetHashCode()), BigInteger.Abs(new BigInteger(issued.Ticks.GetHashCode())), 10000000000).ToString(),
                issued, issued.Add(new TimeSpan(tokenLifetime))
                );
        }

        public static bool VerifyToken(string username, Token token)
        {
            if (DateTime.UtcNow.Ticks > token.expires.Ticks)
            {
                Token verificationToken = CreateToken(username, token.issued);
                return token.Equals(verificationToken);
            } else return false;
        }

    }
}