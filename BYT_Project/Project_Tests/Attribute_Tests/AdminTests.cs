using NUnit.Framework;
using System;
using BYT_Project;

namespace BYT_Project.Tests
{
    [TestFixture]
    public class AdminTests
    {
        [SetUp]
        public void Setup()
        {
            typeof(Admin)
                   .GetField("adminsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                   ?.SetValue(null, new List<Admin>());

            if (File.Exists("admin.xml"))
            {
                File.Delete("admin.xml");
            }
        }

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

            Assert.That(Admin.AdminsList.Count, Is.EqualTo(2));

            Admin.SaveAdmins("admin.xml");

            typeof(Admin)
                   .GetField("adminsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                   ?.SetValue(null, new List<Admin>());

            var success = Admin.LoadAdmins("admin.xml");

            Assert.That(success, Is.True); 
            
            Assert.That(Admin.AdminsList.Count, Is.EqualTo(2));  
        }
    }
}
