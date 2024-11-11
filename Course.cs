using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace BYT_Project
{
    [Serializable]
    public class Course
    {
        private static List<Course> coursesList = new List<Course>();
        private int _courseID;
        private string _title;
        private string _description;
        private int _maxEnrollment;

        public int CourseID
        {
            get => _courseID;
            private set
            {
                if (value <= 0) throw new ArgumentException("Course ID must be positive.");
                _courseID = value;
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Title cannot be empty.");
                _title = value;
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Description cannot be empty.");
                _description = value;
            }
        }

        public int MaxEnrollment
        {
            get => _maxEnrollment;
            set
            {
                if (value <= 0) throw new ArgumentException("MaxEnrollment must be positive.");
                _maxEnrollment = value;
            }
        }

        public Course(int courseID, string title, string description, int maxEnrollment)
        {
            CourseID = courseID;
            Title = title;
            Description = description;
            MaxEnrollment = maxEnrollment;
            coursesList.Add(this);
        }

        public static void SaveCourses(string path = "courses.xml")
        {
            try
            {
                using (var writer = new StreamWriter(path))
                {
                    var serializer = new XmlSerializer(typeof(List<Course>));
                    serializer.Serialize(writer, coursesList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving courses: {ex.Message}");
            }
        }

        public static bool LoadCourses(string path = "courses.xml")
        {
            try
            {
                using (var reader = new StreamReader(path))
                {
                    var serializer = new XmlSerializer(typeof(List<Course>));
                    coursesList = (List<Course>)serializer.Deserialize(reader);
                }
                return true;
            }
            catch (FileNotFoundException)
            {
                coursesList.Clear();
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading courses: {ex.Message}");
                coursesList.Clear();
                return false;
            }
        }

        // Public static property to expose the coursesList for testing purposes
        public static List<Course> CoursesList => coursesList;
    }
}
