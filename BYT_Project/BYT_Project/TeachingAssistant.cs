using System;
using System.Xml.Serialization;

namespace BYT_Project
{
    public interface IInstructor
    {
        string Expertise { get; set; }
        string? OfficeHours { get; set; }
    }

    public interface IStudent
    {
        int StudentID { get; set; }
        List<string> Courses { get; set; }
    }

    [Serializable]
    public class TeachingAssistant : IInstructor, IStudent
    {
        public string Expertise { get; set; }
        public string? OfficeHours { get; set; }
        public int StudentID { get; set; }
        public List<string> Courses { get; set; } = new List<string>();


        private static List<TeachingAssistant> teachingAssistantsList = new List<TeachingAssistant>();
        private int _teachingAssistantID;
        private int _experience;

        public int TeachingAssistantID
        {
            get => _teachingAssistantID;
            set
            {
                if (value <= 0) throw new ArgumentException("Teaching Assistant ID must be positive.");
                _teachingAssistantID = value;
            }
        }

        public int Experience
        {
            get => _experience;
            set
            {
                if (value < 0) throw new ArgumentException("Experience cannot be negative.");
                _experience = value;
            }
        }
        public TeachingAssistant() { }

        public TeachingAssistant(int teachingAssistantID, int experience, string expertise,
            string? officeHours, int studentID, List<string> courses)
        {
            TeachingAssistantID = teachingAssistantID;
            Experience = experience;
            Expertise = expertise;
            OfficeHours = officeHours;
            StudentID = studentID;
            Courses = courses;
            teachingAssistantsList.Add(this);
        }

        public static void SaveTeachingAssistants(string path = "teachingAssistant.xml")
        {
            try
            {
                using (var writer = new StreamWriter(path))
                {
                    var serializer = new XmlSerializer(typeof(List<TeachingAssistant>));
                    serializer.Serialize(writer, teachingAssistantsList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving teaching assistants: {ex.Message}");
            }
        }

        public static bool LoadTeachingAssistants(string path = "teachingAssistant.xml")
        {
            try
            {
                using (var reader = new StreamReader(path))
                {
                    var serializer = new XmlSerializer(typeof(List<TeachingAssistant>));
                    teachingAssistantsList = (List<TeachingAssistant>)serializer.Deserialize(reader);
                }
                return true;
            }
            catch (FileNotFoundException)
            {
                teachingAssistantsList = new List<TeachingAssistant>();
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading teaching assistants: {ex.Message}");
                teachingAssistantsList = new List<TeachingAssistant>();
                return false;
            }
        }

        public static List<TeachingAssistant> TeachingAssistantsList => new List<TeachingAssistant>(teachingAssistantsList);
    }
}