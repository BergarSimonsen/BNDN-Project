using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestService.Security;

namespace RestServiceTest
{
    [TestClass]
    class EncrypterTest
    {
        /*
         * Assert that the SHAEncrypter encrypts in a sustainable manner
         * by creating two identical encrypted strings and checking their equality.
         */
        [TestMethod]
        public void shaEncryptEqualsTest()
        {
            string verification1 = SHAEncrypter.SHAEncrypt("alexandermicrosoftvisualstudiosecretkeystring2331");
            string verification2 = SHAEncrypter.SHAEncrypt("alexandermicrosoftvisualstudiosecretkeystring2331");

            Assert.AreEqual(verification1, verification2);
        }

        /*
         * Assert that the SHAEncrypter encrypts different strings differently
         * by creating two different encrypted string and checking their equality.
         */
        [TestMethod]
        public void shaEncryptNotEqualsTest()
        {
            string verification1 = SHAEncrypter.SHAEncrypt("alexandermicrosoftvisualstudiosecretkeystring2331");
            string verification2 = SHAEncrypter.SHAEncrypt("alexandermicrosoftvisualsecretkeystring2341");

            Assert.AreNotEqual(verification1, verification2);
        }
    }
}
