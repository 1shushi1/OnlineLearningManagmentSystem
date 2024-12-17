using BYT_Project;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Project_Tests.Relation_Tests
{
    [TestFixture]
    public class AdminUserRelationTests
    {
        [SetUp]
        public void Setup()
        {
            typeof(Admin)
                .GetField("adminsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Admin>());

            typeof(User)
                .GetField("usersList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<User>());
        }

        [Test]
        public void TestAddingAdminToUser()
        {
            var user = new User(1, "John Doe", "john@example.com", "password123");
            var admin = new Admin(1, new List<string> { "Manage Users" });

            user.AddAdmin(admin);

            Assert.That(user.Admins.Contains(admin), "Admin not added to user.");
            Assert.That(admin.ManagedUsers.Contains(user), "User not added to admin.");
        }

        [Test]
        public void TestRemovingAdminFromUser()
        {
            var user = new User(1, "John Doe", "john@example.com", "password123");
            var admin = new Admin(1, new List<string> { "Manage Users" });

            user.AddAdmin(admin);
            user.RemoveAdmin(admin);

            Assert.That(!user.Admins.Contains(admin), "Admin not removed from user.");
            Assert.That(!admin.ManagedUsers.Contains(user), "User not removed from admin.");
        }

        [Test]
        public void TestReverseConnectionIntegrity()
        {
            var user = new User(1, "John Doe", "john@example.com", "password123");
            var admin = new Admin(1, new List<string> { "Manage Users" });

            admin.AddUser(user);

            user.RemoveAdmin(admin);

            Assert.That(!user.Admins.Contains(admin), "Admin still connected to user.");
            Assert.That(!admin.ManagedUsers.Contains(user), "User still connected to admin.");
        }

        [Test]
        public void TestUpdatingAdminForUser()
        {
            var user = new User(1, "John Doe", "john@example.com", "password123");
            var oldAdmin = new Admin(1, new List<string> { "Manage Users" });
            var newAdmin = new Admin(2, new List<string> { "Manage Users" });

            user.AddAdmin(oldAdmin);
            user.UpdateAdmin(oldAdmin, newAdmin);

            Assert.Multiple(() =>
            {
                Assert.That(!user.Admins.Contains(oldAdmin), "Old admin still associated with user.");
                Assert.That(user.Admins.Contains(newAdmin), "New admin not associated with user.");
                Assert.That(!oldAdmin.ManagedUsers.Contains(user), "User still connected to old admin.");
                Assert.That(newAdmin.ManagedUsers.Contains(user), "User not connected to new admin.");
            });
        }

        [Test]
        public void TestUpdatingUserForAdmin()
        {
            var admin = new Admin(1, new List<string> { "Manage Users" });
            var oldUser = new User(1, "John Doe", "john@example.com", "password123");
            var newUser = new User(2, "Alice Doe", "alice@example.com", "password456");

            admin.AddUser(oldUser);
            admin.UpdateUser(oldUser, newUser);

            Assert.Multiple(() =>
            {
                Assert.That(!admin.ManagedUsers.Contains(oldUser), "Old user still associated with admin.");
                Assert.That(admin.ManagedUsers.Contains(newUser), "New user not associated with admin.");
                Assert.That(!oldUser.Admins.Contains(admin), "Admin still connected to old user.");
                Assert.That(newUser.Admins.Contains(admin), "Admin not connected to new user.");
            });
        }

        [Test]
        public void TestErrorHandlingForNullInUpdateMethods()
        {
            var user = new User(1, "John Doe", "john@example.com", "password123");
            var admin = new Admin(1, new List<string> { "Manage Users" });

            var ex1 = Assert.Throws<ArgumentException>(() => user.UpdateAdmin(admin, null));
            Assert.That(ex1.Message, Is.EqualTo("Both old and new admins must be provided."));

            var ex2 = Assert.Throws<ArgumentException>(() => admin.UpdateUser(null, user));
            Assert.That(ex2.Message, Is.EqualTo("Both old and new users must be provided."));
        }

        [Test]
        public void TestRemovingAdminAfterUpdate()
        {
            var user = new User(1, "John Doe", "john@example.com", "password123");
            var oldAdmin = new Admin(1, new List<string> { "Manage Users" });
            var newAdmin = new Admin(2, new List<string> { "Manage Users" });

            user.AddAdmin(oldAdmin);
            user.UpdateAdmin(oldAdmin, newAdmin);
            user.RemoveAdmin(newAdmin);

            Assert.Multiple(() =>
            {
                Assert.That(user.Admins.Count, Is.EqualTo(0), "User still associated with an admin after removal.");
                Assert.That(newAdmin.ManagedUsers.Count, Is.EqualTo(0), "Admin still managing a user after removal.");
            });
        }

        [Test]
        public void TestErrorWhenAddingNullAdminToUser()
        {
            var user = new User(1, "John Doe", "john@example.com", "password123");

            var ex = Assert.Throws<ArgumentException>(() => user.AddAdmin(null));
            Assert.That(ex.Message, Is.EqualTo("Admin cannot be null."));
        }

        [Test]
        public void TestErrorWhenAddingNullUserToAdmin()
        {
            var admin = new Admin(1, new List<string> { "Manage Users" });

            var ex = Assert.Throws<ArgumentException>(() => admin.AddUser(null));
            Assert.That(ex.Message, Is.EqualTo("User cannot be null."));
        }

        [Test]
        public void TestErrorWhenRemovingNonExistingAdminFromUser()
        {
            var user = new User(1, "John Doe", "john@example.com", "password123");
            var admin = new Admin(1, new List<string> { "Manage Users" });

            var ex = Assert.Throws<ArgumentException>(() => user.RemoveAdmin(admin));
            Assert.That(ex.Message, Is.EqualTo("Admin is not associated with this user."));
        }

        [Test]
        public void TestErrorWhenRemovingNonExistingUserFromAdmin()
        {
            var user = new User(1, "John Doe", "john@example.com", "password123");
            var admin = new Admin(1, new List<string> { "Manage Users" });

            var ex = Assert.Throws<ArgumentException>(() => admin.RemoveUser(user));
            Assert.That(ex.Message, Is.EqualTo("User is not managed by this admin."));
        }

        [Test]
        public void TestErrorWhenAddingDuplicateAdminToUser()
        {
            var user = new User(1, "John Doe", "john@example.com", "password123");
            var admin = new Admin(1, new List<string> { "Manage Users" });

            user.AddAdmin(admin);

            var ex = Assert.Throws<ArgumentException>(() => user.AddAdmin(admin));
            Assert.That(ex.Message, Is.EqualTo("Admin is already associated with this user."));

            Assert.That(user.Admins.Count, Is.EqualTo(1), "User's admin list was modified after duplicate addition attempt.");
            Assert.That(admin.ManagedUsers.Count, Is.EqualTo(1), "Admin's user list was modified after duplicate addition attempt.");
        }


        [Test]
        public void TestErrorWhenAddingDuplicateUserToAdmin()
        {
            var user = new User(1, "John Doe", "john@example.com", "password123");
            var admin = new Admin(1, new List<string> { "Manage Users" });

            admin.AddUser(user);

            var ex = Assert.Throws<ArgumentException>(() => admin.AddUser(user));
            Assert.That(ex.Message, Is.EqualTo("User is already managed by this admin."));
        }

        [Test]
        public void TestErrorWhenUpdatingAdminWithNull()
        {
            var user = new User(1, "John Doe", "john@example.com", "password123");
            var admin = new Admin(1, new List<string> { "Manage Users" });

            user.AddAdmin(admin);

            var ex = Assert.Throws<ArgumentException>(() => user.UpdateAdmin(admin, null));
            Assert.That(ex.Message, Is.EqualTo("Both old and new admins must be provided."));
        }

        [Test]
        public void TestErrorWhenUpdatingUserWithNull()
        {
            var admin = new Admin(1, new List<string> { "Manage Users" });
            var user = new User(1, "John Doe", "john@example.com", "password123");

            admin.AddUser(user);

            var ex = Assert.Throws<ArgumentException>(() => admin.UpdateUser(user, null));
            Assert.That(ex.Message, Is.EqualTo("Both old and new users must be provided."));
        }

        [Test]
        public void TestErrorWhenUpdatingNonExistingAdminForUser()
        {
            var user = new User(1, "John Doe", "john@example.com", "password123");
            var oldAdmin = new Admin(1, new List<string> { "Manage Users" });
            var newAdmin = new Admin(2, new List<string> { "Manage Users" });

            var ex = Assert.Throws<ArgumentException>(() => user.UpdateAdmin(oldAdmin, newAdmin));
            Assert.That(ex.Message, Is.EqualTo("Admin is not associated with this user."));
        }

        [Test]
        public void TestErrorWhenUpdatingNonExistingUserForAdmin()
        {
            var admin = new Admin(1, new List<string> { "Manage Users" });
            var oldUser = new User(1, "John Doe", "john@example.com", "password123");
            var newUser = new User(2, "Alice Doe", "alice@example.com", "password456");

            var ex = Assert.Throws<ArgumentException>(() => admin.UpdateUser(oldUser, newUser));
            Assert.That(ex.Message, Is.EqualTo("User is not managed by this admin."));
        }
    }
}
