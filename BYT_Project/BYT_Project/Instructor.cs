using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace BYT_Project
{
    [Serializable]
    public class Instructor
    {
        private static List<Instructor> instructorsList = new List<Instructor>();
        private List<Course> courses = new List<Course>();

        public IReadOnlyList<Course> Courses => courses.AsReadOnly();

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

        public string? OfficeHours
        {
            get => _officeHours;
            set
            {
                if (value != null && string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Office hours cannot be an empty or whitespace-only string.");
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

        public void AddCourse(Course course)
        {
            if (course == null) throw new ArgumentNullException(nameof(course));
            if (courses.Contains(course))
            {
                throw new ArgumentException("Course is already added to this instructor.");
            }

            courses.Add(course);

            // Establish reverse connection if not already set
            if (course.Instructor != this)
            {
                course.SetInstructor(this);
            }
        }



        public void RemoveCourse(Course course)
        {
            if (course == null) throw new ArgumentNullException(nameof(course));
            if (!courses.Remove(course)) // Attempt to remove and check if it exists
            {
                throw new ArgumentException("Course is not added to this instructor.");
            }

            // Remove reverse connection only if the course is still pointing to this instructor
            if (course.Instructor == this)
            {
                course.RemoveInstructor();
            }
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
