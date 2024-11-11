using System;
using System.IO;
using System.Xml.Serialization;

namespace BYT_Project
{
    [Serializable]
    public class Lesson
    {
        private static List<Lesson> lessonsList = new List<Lesson>();
        private int _lessonID;
        private string _lessonTitle;
        private string _videoURL;
        private string _lessonDescription;

        public int LessonID
        {
            get => _lessonID;
            private set
            {
                if (value <= 0) throw new ArgumentException("Lesson ID must be positive.");
                _lessonID = value;
            }
        }

        public string LessonTitle
        {
            get => _lessonTitle;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Lesson title cannot be empty.");
                _lessonTitle = value;
            }
        }

        public string VideoURL
        {
            get => _videoURL;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Video URL cannot be empty.");
                _videoURL = value;
            }
        }

        public string LessonDescription
        {
            get => _lessonDescription;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Lesson description cannot be empty.");
                _lessonDescription = value;
            }
        }

        public Lesson(int lessonID, string lessonTitle, string videoURL, string lessonDescription)
        {
            LessonID = lessonID;
            LessonTitle = lessonTitle;
            VideoURL = videoURL;
            LessonDescription = lessonDescription;
            lessonsList.Add(this);
        }

        public static void SaveLessons(string path = "lessons.xml")
        {
            try
            {
                using (var writer = new StreamWriter(path))
                {
                    var serializer = new XmlSerializer(typeof(List<Lesson>));
                    serializer.Serialize(writer, lessonsList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving lessons: {ex.Message}");
            }
        }

        public static bool LoadLessons(string path = "lessons.xml")
        {
            try
            {
                using (var reader = new StreamReader(path))
                {
                    var serializer = new XmlSerializer(typeof(List<Lesson>));
                    lessonsList = (List<Lesson>)serializer.Deserialize(reader);
                }
                return true;
            }
            catch (FileNotFoundException)
            {
                lessonsList.Clear();
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading lessons: {ex.Message}");
                lessonsList.Clear();
                return false;
            }
        }

        // Public static property to expose the lessonsList for testing purposes
        public static List<Lesson> LessonsList => lessonsList;
    }
}
