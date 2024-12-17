using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace BYT_Project
{
    [Serializable]
    public class User
    {
        private static List<User> usersList = new List<User>();
        private List<Admin> _admins = new List<Admin>(); // one-to-many relation with Admin

        public int UserID { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Name cannot be empty.");
                _name = value;
            }
        }
        private string _name;

        public string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Email cannot be empty.");
                _email = value;
            }
        }
        private string _email;

        public string Password
        {
            get => _password;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Password cannot be empty.");
                _password = value;
            }
        }
        private string _password;

        public IReadOnlyList<Admin> Admins => _admins.AsReadOnly();

        public User() { }

        public User(int userID, string name, string email, string password)
        {
            if (userID <= 0) throw new ArgumentException("User ID must be positive.");
            UserID = userID;
            Name = name;
            Email = email;
            Password = password;
            usersList.Add(this);
        }

        public void AddAdmin(Admin admin)
        {
            if (admin == null) throw new ArgumentException("Admin cannot be null.");

            // Check for duplicate relationship
            if (_admins.Contains(admin)) throw new ArgumentException("Admin is already associated with this user.");

            _admins.Add(admin);

            // Ensure the relationship is two-way by checking if this user is already in the admin's managed users list
            if (!admin.ManagedUsers.Contains(this))
            {
                admin.AddUser(this);
            }
        }

        public void RemoveAdmin(Admin admin)
        {
            if (admin == null) throw new ArgumentException("Admin cannot be null.");

            // Attempt to remove the admin; if not found, throw an exception
            if (!_admins.Remove(admin)) throw new ArgumentException("Admin is not associated with this user.");

            // Ensure the relationship is removed from the admin's side as well
            if (admin.ManagedUsers.Contains(this))
            {
                admin.RemoveUser(this);
            }
        }

        public void UpdateAdmin(Admin oldAdmin, Admin newAdmin)
        {
            if (oldAdmin == null || newAdmin == null)
                throw new ArgumentException("Both old and new admins must be provided.");

            RemoveAdmin(oldAdmin);
            AddAdmin(newAdmin);
        }

        public static void SaveUsers(string path = "user.xml")
        {
            try
            {
                using (var writer = new StreamWriter(path))
                {
                    var serializer = new XmlSerializer(typeof(List<User>));
                    serializer.Serialize(writer, usersList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving users: {ex.Message}");
            }
        }

        public static bool LoadUsers(string path = "user.xml")
        {
            try
            {
                using (var reader = new StreamReader(path))
                {
                    var serializer = new XmlSerializer(typeof(List<User>));
                    usersList = (List<User>)serializer.Deserialize(reader);
                }
                return true;
            }
            catch (FileNotFoundException)
            {
                usersList = new List<User>();
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading users: {ex.Message}");
                usersList = new List<User>();
                return false;
            }
        }

        public static List<User> UsersList => new List<User>(usersList);
    }
}
