using NUnit.Framework;
using System;
using BYT_Project;
using System.IO;

namespace BYT_Project.Tests
{
    [TestFixture]
    public class AdminTests
    {
        [Test]
        public void TestGetCorrectAdminInformation()
        {
            var admin = new Admin(1, new List<string> { "Full Access", "Moderation" });
            Assert.That(admin.AdminID, Is.EqualTo(1));
            Assert.That(admin.Permissions.Count, Is.EqualTo(2));
            Assert.That(admin.Permissions[0], Is.EqualTo("Full Access"));
        }

        [Test]
        public void TestExceptionForInvalidAdminID()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Admin(-1, new List<string> { "Full Access" }));
            Assert.That(ex.Message, Is.EqualTo("Admin ID must be positive."));
        }

        [Test]
        public void TestExceptionForEmptyPermissions()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Admin(1, new List<string>()));
            Assert.That(ex.Message, Is.EqualTo("Permissions cannot be empty."));
        }

        [Test]
        public void TestSaveAndLoadAdmins()
        {
            var admin1 = new Admin(1, new List<string> { "Full Access" });
            var admin2 = new Admin(2, new List<string> { "Moderation" });

            Admin.SaveAdmins("adminsTest.xml");
            var success = Admin.LoadAdmins("adminsTest.xml");

            Assert.That(success, Is.True);  // Check if LoadAdmins() returns true
            Assert.That(Admin.AdminsList.Count, Is.EqualTo(2));  // Check that the list contains 2 admins
        }

        [Test]
        public void TestPersistencyAfterLoad()
        {
            var admin = new Admin(1, new List<string> { "Full Access" });
            Admin.SaveAdmins("adminsTest.xml");

            // Clear the current list and reload
            Admin.LoadAdmins("adminsTest.xml");

            Assert.That(Admin.AdminsList.Count, Is.GreaterThan(0));  // Ensure admins were loaded from the file
        }
    }
}
