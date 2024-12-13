using System;
using System.Xml.Serialization;

namespace BYT_Project
{
    [Serializable]
    public class TeachingAssistant
    {
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

        public TeachingAssistant(int teachingAssistantID, int experience)
        {
            TeachingAssistantID = teachingAssistantID;
            Experience = experience;
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
