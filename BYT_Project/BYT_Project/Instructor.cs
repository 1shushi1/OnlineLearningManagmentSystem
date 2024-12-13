using System;
using System.Xml.Serialization;

namespace BYT_Project
{
    [Serializable]
    public class Instructor
    {
        private static List<Instructor> instructorsList = new List<Instructor>();
        private int _instructorID;
        private string _expertise;
        private string? _officeHours;

        public int InstructorID
        {
            get => _instructorID;
            set
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
            set
            {
                if (value != null && string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Office hours cannot be an empty or whitespace only string.");
                }
                _officeHours = value;
            } 

        }

        public Instructor() { }
        public Instructor(int instructorID, string expertise, string officeHours = null)
        {
            InstructorID = instructorID;
            Expertise = expertise;
            OfficeHours = officeHours;
            instructorsList.Add(this);
        }

        public static void SaveInstructors(string path = "instructor.xml")
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

        public static bool LoadInstructors(string path = "instructor.xml")
        {
            try
            {
                using (var reader = new StreamReader(path))
                {
                    var serializer = new XmlSerializer(typeof(List<Instructor>));
                    instructorsList = (List<Instructor>)serializer.Deserialize(reader);
                }
                return true;
            }
            catch (FileNotFoundException)
            {
                instructorsList = new List<Instructor>();
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading instructors: {ex.Message}");
                instructorsList = new List<Instructor>();
                return false;
            }
        }

        public static List<Instructor> InstructorsList => new List<Instructor>(instructorsList);
    }
}
