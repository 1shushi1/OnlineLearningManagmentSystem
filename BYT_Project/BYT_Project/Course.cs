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
        private List<Student> _students = new List<Student>(); // zero-to-many relation with Student
        private List<Lesson> _lessons = new List<Lesson>(); // zero-to-many relation with Lesson, Course composes of Lesson
        private Instructor _instructor; // one-to-one relation with Instructor, Course aggregates Instructor
        private Timetable? _timetable; // zero-to-one relation with Timetable, Course aggregates Timetable
        private List<Payment> _payments = new List<Payment>(); // Aggregation: Course to Payment (one-to-many)
        public IReadOnlyList<Payment> Payments => _payments.AsReadOnly(); // Encapsulated getter for payments


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
        public Instructor Instructor => _instructor;
        public Timetable? Timetable => _timetable;

        public Course() { }

        public Course(int courseID, string title, string description, int maxEnrollment)
        {
            CourseID = courseID;
            Title = title;
            Description = description;
            MaxEnrollment = maxEnrollment;
            coursesList.Add(this);
        }

        public void AddPayment(Payment payment)
        {
            if (payment == null) throw new ArgumentNullException(nameof(payment));
            if (_payments.Contains(payment)) throw new ArgumentException("Payment is already added to this course.");
            if (payment.Course != null && payment.Course != this)
                throw new ArgumentException("Payment is already associated with another course.");

            _payments.Add(payment);
            payment.SetCourse(this); // Reverse connection
        }

        public void RemovePayment(Payment payment)
        {
            if (payment == null) throw new ArgumentNullException(nameof(payment));
            if (!_payments.Contains(payment)) throw new ArgumentException("Payment is not associated with this course.");

            _payments.Remove(payment);
            if (payment.Course == this)
            {
                payment.RemoveCourse(); // Reverse connection
            }
        }

        public void UpdatePayment(Payment oldPayment, Payment newPayment)
        {
            if (oldPayment == null || newPayment == null)
                throw new ArgumentException("Both old and new payments must be provided.");

            RemovePayment(oldPayment);
            AddPayment(newPayment);
        }

        public void AddStudent(Student student)
        {
            if (student == null) throw new ArgumentException("Student cannot be null.");
            if (_students.Contains(student)) throw new ArgumentException("Student is already enrolled in this course.");
            // Prevents duplicate entries by checking if the student is already associated with this course

            if (_students.Count >= MaxEnrollment) throw new InvalidOperationException("Course has reached maximum enrollment.");
            // Ensures that the course does not exceed its maximum enrollment limit

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
            // Ensures the student is in the `_students` list before attempting to remove

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
            // Prevents duplicate entries by checking if the lesson is already associated with this course

            _lessons.Add(lesson);
            if (lesson.Course != this)
            {
                lesson.AssignToCourse(this);
            }
        }

        public void RemoveLesson(Lesson lesson)
        {
            if (lesson == null) throw new ArgumentException("Lesson cannot be null.");

            // Cascade deletion for lessons
            Lesson.DeleteLesson(lesson);

            _lessons.Remove(lesson);
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

        public void SetTimetable(Timetable? timetable)
        {
            if (_timetable == timetable) return;
            // If the timetable is already set, does nothing to prevent redundant operations

            if (_timetable != null)
            {
                _timetable.RemoveCourse(this); // reverse disconnection, remove this course from current Timetable
            }

            _timetable = timetable;
            // sets new Timetable for this Course

            if (timetable != null && !timetable.Courses.Contains(this))
            {
                timetable.AddCourse(this); // new Timetable reference, reverse connection
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
        public static List<Course> CoursesList => new List<Course>(coursesList);
    }
}