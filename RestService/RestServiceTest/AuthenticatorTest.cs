using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestService.Security;
using RestService.IO_Messages;

namespace RestServiceTest
{
    [TestClass]
    class AuthenticatorTest
    {
        /*
         * Tests to see whether the Authenticator can correctly assert
         * authenticity of a request.
         * URI and User objects are unneccessary for this check, which is why
         * they are null. RestMethod is also unneccessary, and set to GET as a default.
         */
        [TestMethod]
        public void TestVerifyHash()
        {
            Dictionary<string, string> insertedData = createRequestData();

            String value = "";
            insertedData.TryGetValue("email", out value);
            string authorization = value;
            insertedData.TryGetValue("secret", out value);
            authorization += value + "mg24";

            authorization = SHAEncrypter.SHAEncrypt(authorization);

            Request request = new Request(null, RestService.Web_Service.RestMethods.GET, insertedData, null, authorization);

            Assert.IsTrue(Authenticator.VerifyHash(request));
        }

        /*
         * Tests to see whether the Authenticator can correctly assert
         * the forgery of a request.
         * URI and User objects are unneccessary for this check, which is why
         * they are null. RestMethod is also unneccessary, and set to GET as a default.
         */
        [TestMethod]
        public void TestVerifyFalseHash()
        {
            Dictionary<string, string> insertedData = createRequestData();

            String value = "";
            insertedData.TryGetValue("email", out value);
            string authorization = value;
            insertedData.TryGetValue("secret", out value);
            authorization += value + "mg24";

            authorization = SHAEncrypter.SHAEncrypt(authorization);

            insertedData.Remove("email");
            insertedData.Add("email", "someotheremail@email.com");

            Request request = new Request(null, RestService.Web_Service.RestMethods.GET, insertedData, null, authorization);

            Assert.IsFalse(Authenticator.VerifyHash(request));
        }

        /*
         * Tests to see whether the Authenticator recognizes a faulty
         * clientkey.
         * URI and User objects are unneccessary for this check, which is why
         * they are null. RestMethod is also unneccessary, and set to GET as a default.
         */
        [TestMethod]
        public void TestVerifyFalseKey()
        {
            Dictionary<string, string> insertedData = createRequestData();

            insertedData.Remove("secret");
            insertedData.Add("secret", "shk5");

            String value = "";
            insertedData.TryGetValue("email", out value);
            string authorization = value;
            insertedData.TryGetValue("secret", out value);
            authorization += value + "mg24";

            Request request = new Request(null, RestService.Web_Service.RestMethods.GET, insertedData, null, authorization);

            try
            {
                Authenticator.VerifyHash(request);
                Assert.Fail("No exception was thrown");
            }
            catch (Exception e)
            {
                Assert.AreEqual("No such clientKey exists", e.Message);
            }
        }

        private Dictionary<string, string> createRequestData()
        {
            Dictionary<string, string> requestData = new Dictionary<string, string>();
            requestData.Add("email", "someemail@email.com");
            requestData.Add("secret", "17fh");

            return requestData;
        }
    }
}
