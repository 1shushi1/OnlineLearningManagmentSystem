using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BYT_Project
{
    [Serializable]
    public class Student
    {
        private static List<Student> studentsList = new List<Student>();
        private int _studentID;
        private List<Assignment> _assignments = new List<Assignment>();
        private List<Payment> _payments = new List<Payment>();
        private List<Course> _courses = new List<Course>();

        public int StudentID
        {
            get => _studentID;
            set
            {
                if (value <= 0) throw new ArgumentException("Student ID must be positive.");
                _studentID = value;
            }
        }

        public IReadOnlyList<Assignment> Assignments => _assignments.AsReadOnly();
        public IReadOnlyList<Payment> Payments => _payments.AsReadOnly();
        public IReadOnlyList<Course> Courses => _courses.AsReadOnly();

        public Student() { }

        public Student(int studentID)
        {
            StudentID = studentID;
            studentsList.Add(this);
        }

        public void AddAssignment(Assignment assignment)
        {
            if (assignment == null) throw new ArgumentException("Assignment cannot be null.");
            if (_assignments.Contains(assignment)) throw new ArgumentException("Assignment is already added to this student.");
            _assignments.Add(assignment);
            if (!assignment.Students.Contains(this))
            {
                assignment.AddStudent(this); 
            }
        }

        public void RemoveAssignment(Assignment assignment)
        {
            if (assignment == null) throw new ArgumentException("Assignment cannot be null.");
            if (!_assignments.Remove(assignment)) throw new ArgumentException("Assignment is not added to this student.");
            if (assignment.Students.Contains(this))
            {
                assignment.RemoveStudent(this); 
            }
        }

        public void UpdateAssignment(Assignment oldAssignment, Assignment newAssignment)
        {
            if (oldAssignment == null || newAssignment == null)
                throw new ArgumentException("Both old and new assignments must be provided.");

            RemoveAssignment(oldAssignment);
            AddAssignment(newAssignment);
        }

        public void AddPayment(Payment payment)
        {
            if (payment == null) throw new ArgumentException("Payment cannot be null.");
            if (_payments.Contains(payment)) throw new ArgumentException("Payment is already added to this student.");

            if (payment.Student != null && payment.Student != this)
                throw new ArgumentException("Payment is already assigned to a student.");

            _payments.Add(payment);

            if (payment.Student != this)
            {
                payment.AssignToStudent(this);
            }
        }


        public void RemovePayment(Payment payment)
        {
            if (payment == null) throw new ArgumentException("Payment cannot be null.");
            if (!_payments.Remove(payment)) throw new ArgumentException("Payment is not added to this student.");

            if (payment.Student == this)
            {
                payment.RemoveStudent();
            }
        }

        public void UpdatePayment(Payment oldPayment, Payment newPayment)
        {
            if (oldPayment == null || newPayment == null)
                throw new ArgumentException("Both old and new payments must be provided.");

            RemovePayment(oldPayment);
            AddPayment(newPayment);
        }
        public void AddCourse(Course course)
        {
            if (course == null) throw new ArgumentException("Course cannot be null.");
            if (_courses.Contains(course)) throw new ArgumentException("Course is already added to this student.");

          
            if (course.Students.Count > 0 && course.Students[0] != this)
                throw new ArgumentException("Course is already assigned to a student.");

            _courses.Add(course);
            if (!course.Students.Contains(this))
            {
                course.AddStudent(this);  
            }
        }

    public void RemoveCourse(Course course)
        {
            if (course == null) throw new ArgumentException("Course cannot be null.");
            if (!_courses.Remove(course)) throw new ArgumentException("Course is not added to this student.");

            if (course.Students.Contains(this))
            {
                course.RemoveStudent(this); 
            }
        }

        public void UpdateCourse(Course oldCourse, Course newCourse)
        {
            if (oldCourse == null || newCourse == null)
                throw new ArgumentException("Both old and new courses must be provided.");

            RemoveCourse(oldCourse);
            AddCourse(newCourse);
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
                using (var reader = new StreamReader(path))
                {
                    var serializer = new XmlSerializer(typeof(List<Student>));
                    studentsList = (List<Student>)serializer.Deserialize(reader);
                }
                return true;
            }
            catch (FileNotFoundException)
            {
                studentsList = new List<Student>();
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading students: {ex.Message}");
                studentsList = new List<Student>();
                return false;
            }
        }

        public static List<Student> StudentsList => new List<Student>(studentsList);
    }
}
