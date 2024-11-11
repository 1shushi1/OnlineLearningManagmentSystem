using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace BYT_Project
{
    [Serializable]
    public class Instructor
    {
        private static List<Instructor> instructorsList = new List<Instructor>();
        private int _instructorID;
        private string _expertise;
        private string _officeHours;

        public int InstructorID
        {
            get => _instructorID;
            private set
            {
                if (value <= 0) throw new ArgumentException("Instructor ID must be positive.");
                _instructorID = value;
            }
        }

        public string Expertise
        {
            get => _expertise;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Expertise cannot be empty.");
                _expertise = value;
            }
        }

        public string OfficeHours
        {
            get => _officeHours;
            set => _officeHours = value;  // Optional field
        }

        public Instructor(int instructorID, string expertise, string officeHours = null)
        {
            InstructorID = instructorID;
            Expertise = expertise;
            OfficeHours = officeHours;
            instructorsList.Add(this);
        }

        public static void SaveInstructors(string path = "instructors.xml")
        {
            try
            {
                using (var writer = new StreamWriter(path))
                {
                    var serializer = new XmlSerializer(typeof(List<Instructor>));
                    serializer.Serialize(writer, instructorsList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving instructors: {ex.Message}");
            }
        }

        public static bool LoadInstructors(string path = "instructors.xml")
        {
            try
            {
                if (File.Exists(path))
                {
                    using (var reader = new StreamReader(path))
                    {
                        var serializer = new XmlSerializer(typeof(List<Instructor>));
                        instructorsList = (List<Instructor>)serializer.Deserialize(reader);
                    }
                    return true;
                }
                else
                {
                    Console.WriteLine($"File {path} not found, initializing empty list.");
                    instructorsList.Clear();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading instructors: {ex.Message}");
                instructorsList.Clear();
                return false;
            }
        }

        public static IReadOnlyList<Instructor> InstructorsList => new ReadOnlyCollection<Instructor>(instructorsList);
    }
}
