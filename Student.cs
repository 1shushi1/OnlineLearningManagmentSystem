using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace BYT_Project
{
    [Serializable]
    public class Student
    {
        private static List<Student> studentsList = new List<Student>();
        private int _studentID;

        public int StudentID
        {
            get => _studentID;
            private set
            {
                if (value <= 0) throw new ArgumentException("Student ID must be positive.");
                _studentID = value;
            }
        }

        public Student(int studentID)
        {
            StudentID = studentID;
            studentsList.Add(this);
        }

        public static void SaveStudents(string path = "students.xml")
        {
            try
            {
                using (var writer = new StreamWriter(path))
                {
                    var serializer = new XmlSerializer(typeof(List<Student>));
                    serializer.Serialize(writer, studentsList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving students: {ex.Message}");
            }
        }

        public static bool LoadStudents(string path = "students.xml")
        {
            try
            {
                if (File.Exists(path))
                {
                    using (var reader = new StreamReader(path))
                    {
                        var serializer = new XmlSerializer(typeof(List<Student>));
                        studentsList = (List<Student>)serializer.Deserialize(reader);
                    }
                    return true;
                }
                else
                {
                    Console.WriteLine($"File {path} not found, initializing empty list.");
                    studentsList.Clear();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading students: {ex.Message}");
                studentsList.Clear();
                return false;
            }
        }

        public static IReadOnlyList<Student> StudentsList => new ReadOnlyCollection<Student>(studentsList);
    }
}
