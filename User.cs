using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace BYT_Project
{
    [Serializable]
    public class User
    {
        private static List<User> usersList = new List<User>();
        private int _userID;
        private string _name;
        private string _email;
        private string _password;

        public int UserID
        {
            get => _userID;
            private set
            {
                if (value <= 0) throw new ArgumentException("User ID must be positive.");
                _userID = value;
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Name cannot be empty.");
                _name = value;
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Email cannot be empty.");
                _email = value;
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Password cannot be empty.");
                _password = value;
            }
        }

        public User(int userID, string name, string email, string password)
        {
            UserID = userID;
            Name = name;
            Email = email;
            Password = password;
            usersList.Add(this);
        }

        public static void SaveUsers(string path = "users.xml")
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

        public static bool LoadUsers(string path = "users.xml")
        {
            try
            {
                if (File.Exists(path))
                {
                    using (var reader = new StreamReader(path))
                    {
                        var serializer = new XmlSerializer(typeof(List<User>));
                        usersList = (List<User>)serializer.Deserialize(reader);
                    }
                    return true;
                }
                else
                {
                    Console.WriteLine($"File {path} not found, initializing empty list.");
                    usersList.Clear();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading users: {ex.Message}");
                usersList.Clear();
                return false;
            }
        }

        public static IReadOnlyList<User> UsersList => new ReadOnlyCollection<User>(usersList);
    }
}
