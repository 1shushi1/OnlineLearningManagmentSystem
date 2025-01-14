using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace BYT_Project
{
    [Serializable]
    public class Admin : User
    {
        private static List<Admin> adminsList = new List<Admin>();
        private int _adminID;
        private List<string> _permissions = new List<string>();
        private List<User> _managedUsers = new List<User>(); // one-to-many relation with User

        public int AdminID
        {
            get => _adminID;
            set
            {
                if (value <= 0) throw new ArgumentException("Admin ID must be positive.");
                _adminID = value;
            }
        }

        public List<string> Permissions
        {
            get => _permissions;
            set
            {
                if (value == null || value.Count == 0) throw new ArgumentException("Permissions cannot be empty.");
                _permissions = value;
            }
        }

        public IReadOnlyList<User> ManagedUsers => _managedUsers.AsReadOnly();

        public Admin() { }

        public Admin(int userID, string name, string email, string password, int adminID, List<string> permissions, List<User> managedUsers = null)
            : base(userID, name, email, password)
        {
            AdminID = adminID;
            Permissions = permissions;

            // If managedUsers is provided, associate the users with this admin
            if (managedUsers != null)
            {
                foreach (var user in managedUsers)
                {
                    AddUser(user);
                }
            }
            adminsList.Add(this);
        }


        public void AddUser(User user)
        {
            if (user == null) throw new ArgumentException("User cannot be null.");

            // Check for duplicate relationship
            if (_managedUsers.Contains(user)) throw new ArgumentException("User is already managed by this admin.");

            _managedUsers.Add(user);

            // Ensure the relationship is two-way by checking if this admin is already in the user's admin list
            if (!user.Admins.Contains(this))
            {
                user.AddAdmin(this); // Reverse connection
            }
        }

        public void RemoveUser(User user)
        {
            if (user == null) throw new ArgumentException("User cannot be null.");

            // Attempt to remove the user; if not found, throw an exception
            if (!_managedUsers.Remove(user)) throw new ArgumentException("User is not managed by this admin.");

            // Ensure the relationship is removed from the user's side as well
            if (user.Admins.Contains(this))
            {
                user.RemoveAdmin(this);
            }
        }

        public void UpdateUser(User oldUser, User newUser)
        {
            if (oldUser == null || newUser == null)
                throw new ArgumentException("Both old and new users must be provided.");

            RemoveUser(oldUser);
            AddUser(newUser);
        }

        public static void SaveAdmins(string path = "admin.xml")
        {
            try
            {
                using (var writer = new StreamWriter(path))
                {
                    var serializer = new XmlSerializer(typeof(List<Admin>));
                    serializer.Serialize(writer, adminsList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving admins: {ex.Message}");
            }
        }

        public static bool LoadAdmins(string path = "admin.xml")
        {
            try
            {
                using (var reader = new StreamReader(path))
                {
                    var serializer = new XmlSerializer(typeof(List<Admin>));
                    adminsList = (List<Admin>)serializer.Deserialize(reader);
                }
                return true;
            }
            catch (FileNotFoundException)
            {
                adminsList = new List<Admin>();
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading admins: {ex.Message}");
                adminsList = new List<Admin>();
                return false;
            }
        }

        public static List<Admin> AdminsList => new List<Admin>(adminsList);
    }
}