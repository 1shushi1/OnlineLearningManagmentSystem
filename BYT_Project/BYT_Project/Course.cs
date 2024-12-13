using System;
using System.Collections.Generic;
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
        private List<Student> _students = new List<Student>();
        private List<Lesson> _lessons = new List<Lesson>();

        public int CourseID
        {
            get => _courseID;
            set
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

        public IReadOnlyList<Student> Students => _students.AsReadOnly();
        public IReadOnlyList<Lesson> Lessons => _lessons.AsReadOnly();

        public Course() { }

        public Course(int courseID, string title, string description, int maxEnrollment)
        {
            CourseID = courseID;
            Title = title;
            Description = description;
            MaxEnrollment = maxEnrollment;
            coursesList.Add(this);
        }
        public void AddStudent(Student student)
        {
            if (student == null) throw new ArgumentException("Student cannot be null.");
            if (_students.Contains(student)) throw new ArgumentException("Student is already added to this course.");

            _students.Add(student);
            if (!student.Courses.Contains(this))
            {
                student.AddCourse(this); 
            }
        }

        public void RemoveStudent(Student student)
        {
            if (student == null) throw new ArgumentException("Student cannot be null.");
            if (!_students.Remove(student)) throw new ArgumentException("Student is not added to this course.");

            if (student.Courses.Contains(this))
            {
                student.RemoveCourse(this); 
            }
        }
        public void UpdateStudent(Student oldStudent, Student newStudent)
        {
            if (oldStudent == null || newStudent == null)
                throw new ArgumentException("Both old and new students must be provided.");

            RemoveStudent(oldStudent);
            AddStudent(newStudent);
        }
        public void AddLesson(Lesson lesson)
        {
            if (lesson == null) throw new ArgumentException("Lesson cannot be null.");
            if (_lessons.Contains(lesson)) throw new ArgumentException("Lesson is already added to this course.");

            _lessons.Add(lesson);
            if (lesson.Course != this) 
            {
                lesson.AssignToCourse(this);
            }
        }

        public void RemoveLesson(Lesson lesson)
        {
            if (lesson == null) throw new ArgumentException("Lesson cannot be null.");
            if (!_lessons.Remove(lesson)) throw new ArgumentException("Lesson is not added to this course.");

            if (lesson.Course == this) 
            {
                lesson.RemoveCourse();
            }
        }

        public void UpdateLesson(Lesson oldLesson, Lesson newLesson)
        {
            if (oldLesson == null || newLesson == null)
                throw new ArgumentException("Both old and new lessons must be provided.");

            RemoveLesson(oldLesson);
            AddLesson(newLesson);
        }

        public static void SaveCourses(string path = "course.xml")
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

        public static bool LoadCourses(string path = "course.xml")
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
                coursesList = new List<Course>();
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading courses: {ex.Message}");
                coursesList = new List<Course>();
                return false;
            }
        }

        public static List<Course> CoursesList => new List<Course>(coursesList);
    }
}
