using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace BYT_Project
{
    [Serializable]
    public class Admin
    {
        private static List<Admin> adminsList = new List<Admin>();
        private int _adminID;
        private List<string> _permissions = new List<string>();

        public int AdminID
        {
            get => _adminID;
            private set
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

        public Admin(int adminID, List<string> permissions)
        {
            AdminID = adminID;
            Permissions = permissions;
            adminsList.Add(this);
        }

        public static void SaveAdmins(string path = "admins.xml")
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

        public static bool LoadAdmins(string path = "admins.xml")
        {
            try
            {
                if (File.Exists(path))
                {
                    using (var reader = new StreamReader(path))
                    {
                        var serializer = new XmlSerializer(typeof(List<Admin>));
                        adminsList = (List<Admin>)serializer.Deserialize(reader);
                    }
                    return true;
                }
                else
                {
                    Console.WriteLine($"File {path} not found, initializing empty list.");
                    adminsList.Clear();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading admins: {ex.Message}");
                adminsList.Clear();
                return false;
            }
        }

        public static IReadOnlyList<Admin> AdminsList => new ReadOnlyCollection<Admin>(adminsList);
    }
}
