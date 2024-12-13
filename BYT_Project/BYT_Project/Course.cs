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
        private Instructor? _instructor;
        private Timetable? _timetable;
        public Timetable? Timetable => _timetable;

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
        public Instructor? Instructor => _instructor;

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
            if (_students.Contains(student)) throw new ArgumentException("Student is already enrolled in this course.");
            if (_students.Count >= MaxEnrollment) throw new InvalidOperationException("Course has reached maximum enrollment.");

            _students.Add(student);
            if (!student.Courses.Contains(this))
            {
                student.AddCourse(this); 
            }
        }

        public void RemoveStudent(Student student)
        {
            if (student == null) throw new ArgumentException("Student cannot be null.");
            if (!_students.Remove(student)) throw new ArgumentException("Student is not enrolled in this course.");
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

        public void SetInstructor(Instructor? instructor)
        {
            if (_instructor == instructor) return;

            if (_instructor != null)
            {
                _instructor.RemoveCourse(this);
            }

            _instructor = instructor;

            if (instructor != null && !instructor.Courses.Contains(this))
            {
                instructor.AddCourse(this);
            }
        }



        public void RemoveInstructor()
        {
            if (_instructor == null) return;

            var tempInstructor = _instructor;
            _instructor = null;

            if (tempInstructor.Courses.Contains(this))
            {
                tempInstructor.RemoveCourse(this);
            }
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


        public void SetTimetable(Timetable? timetable)
        {
            if (_timetable == timetable) return;

            if (_timetable != null)
            {
                _timetable.RemoveCourse(this);
            }

            _timetable = timetable;

            if (timetable != null && !timetable.Courses.Contains(this))
            {
                timetable.AddCourse(this);
            }
        }


        public void RemoveTimetable()
        {
            if (_timetable == null) return;

            var tempTimetable = _timetable;
            _timetable = null;

            if (tempTimetable.Courses.Contains(this))
            {
                tempTimetable.RemoveCourse(this);
            }
        }



        public static List<Course> CoursesList => new List<Course>(coursesList);
    }
}
