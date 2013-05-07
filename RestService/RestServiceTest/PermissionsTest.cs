using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestService;
using RestService.Security;
using RestService.Entities;

namespace RestServiceTest
{
    [TestClass]
    public class PermissionsTest
    {
        [TestMethod]
        public void TestCreatePermissions()
        {
            Permissions permissions = Permissions.createPermissions(new User(14, null, null, null),new DatabaseConnection("SMU"));
            Assert.IsTrue(permissions.actions.Length > 0);
            for (int i = 0; i < permissions.actions.Length; i++)
            {
                Console.WriteLine(permissions.actions[i].name);
            }
        }

        [TestMethod]
        public void TestCanDo()
        {
            Permissions permissions = Permissions.createPermissions(new User(14, null, null, null), new DatabaseConnection("SMU"));
            Assert.IsTrue(permissions.canDo(new RestService.Entities.Action(5,"watchMovie",null,true)));
            Assert.IsFalse(permissions.canDo(new RestService.Entities.Action(6, "watchMovie", null, true)));
        }
    }
}
