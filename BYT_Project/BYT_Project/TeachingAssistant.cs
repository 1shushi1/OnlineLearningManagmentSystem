using System;
using System.Xml.Serialization;

namespace BYT_Project
{

    public interface IStudent
    {
        int StudentID { get; set; }
        List<string> Courses { get; set; }
    }

    [Serializable]
    public class TeachingAssistant : Instructor, IStudent
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
        /*
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
        */

        // Copy constructor for multi-inheritance
        public TeachingAssistant(Instructor instructor, int studentID, List<string> courses)
            : base(instructor.UserID, instructor.Name, instructor.Email, instructor.Password, instructor.InstructorID, instructor.Expertise, instructor.OfficeHours)
        {
            if (instructor == null) throw new ArgumentNullException(nameof(instructor));

            // Copy attributes from Instructor
            Expertise = instructor.Expertise;
            OfficeHours = instructor.OfficeHours;

            // Assign IStudent attributes
            StudentID = studentID;
            Courses = new List<string>(courses);

            // Set TeachingAssistant-specific attributes
            TeachingAssistantID = GenerateUniqueTeachingAssistantID();
            Experience = 0; // Default value; can be updated later

            teachingAssistantsList.Add(this);
        }

        // Generate a unique TeachingAssistantID
        private int GenerateUniqueTeachingAssistantID()
        {
            return teachingAssistantsList.Count + 1; // Example logic for ID generation
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
