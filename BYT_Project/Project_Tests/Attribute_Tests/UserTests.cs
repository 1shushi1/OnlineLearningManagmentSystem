using NUnit.Framework;
using System;
using BYT_Project;

namespace BYT_Project.Tests
{
    [TestFixture]
    public class UserTests
    {
        [SetUp]
        public void Setup()
        {
            typeof(User)
               .GetField("usersList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
               ?.SetValue(null, new List<User>());

            if (File.Exists("user.xml"))
            {
                File.Delete("user.xml");
            }
        }
        [Test]
        public void TestGetCorrectUserInformation()
        {
            var user = new User(1, "John Doe", "john.doe@example.com", "password123");
            Assert.That(user.UserID, Is.EqualTo(1));
            Assert.That(user.Name, Is.EqualTo("John Doe"));
            Assert.That(user.Email, Is.EqualTo("john.doe@example.com"));
            Assert.That(user.Password, Is.EqualTo("password123"));
        }

        [Test]
        public void TestExceptionForInvalidUserID()
        {
            var ex = Assert.Throws<ArgumentException>(() => new User(-1, "John Doe", "john.doe@example.com", "password123"));
            Assert.That(ex.Message, Is.EqualTo("User ID must be positive."));
        }

        [Test]
        public void TestExceptionForEmptyName()
        {
            var ex = Assert.Throws<ArgumentException>(() => new User(1, "", "john.doe@example.com", "password123"));
            Assert.That(ex.Message, Is.EqualTo("Name cannot be empty."));
        }

        [Test]
        public void TestExceptionForEmptyEmail()
        {
            var ex = Assert.Throws<ArgumentException>(() => new User(1, "John Doe", "", "password123"));
            Assert.That(ex.Message, Is.EqualTo("Email cannot be empty."));
        }

        [Test]
        public void TestExceptionForEmptyPassword()
        {
            var ex = Assert.Throws<ArgumentException>(() => new User(1, "John Doe", "john.doe@example.com", ""));
            Assert.That(ex.Message, Is.EqualTo("Password cannot be empty."));
        }

        [Test]
        public void TestSaveAndLoadUsers()
        {
            var user1 = new User(1, "John Doe", "john.doe@example.com", "password123");
            var user2 = new User(2, "Jane Smith", "jane.smith@example.com", "password456");

            Assert.That(User.UsersList.Count, Is.EqualTo(2));

            User.SaveUsers("user.xml");

            typeof(User)
               .GetField("usersList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
               ?.SetValue(null, new List<User>());

            var success = User.LoadUsers("user.xml");

            Assert.That(success, Is.True);
            Assert.That(User.UsersList.Count, Is.EqualTo(2)); 
        }
    }
}
