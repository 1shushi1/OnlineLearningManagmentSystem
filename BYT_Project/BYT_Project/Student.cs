using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BYT_Project
{
    [Serializable]
    public class Student : User
    {
        private static List<Student> studentsList = new List<Student>();
        private int _studentID;
        private List<Assignment> _assignments = new List<Assignment>(); // zero-to-many relation with Assignment
        private List<Payment> _payments = new List<Payment>(); // zero-to-many relation with Payment, Payment aggregates Student
        private List<Course> _courses = new List<Course>(); // zero-to-many relation with Course
        private List<Enrollment> enrollments = new List<Enrollment>(); // Many-to-Many with Attribute


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
        public IReadOnlyList<Enrollment> Enrollments => enrollments.AsReadOnly();

        public Student() { }

        public Student(int userID, string name, string email, string password, int studentID, List<String> courses)
            : base(userID, name, email, password)
        {
            StudentID = studentID;
            studentsList.Add(this);
        }

        public void AddAssignment(Assignment assignment)
        {
            if (assignment == null) throw new ArgumentException("Assignment cannot be null.");

            // Prevents adding the same assignment multiple times
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

            // Removes the assignment from the student's list, throwing an exception if it doesn't exist
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

            // Prevents duplicate payments from being added
            if (_payments.Contains(payment)) throw new ArgumentException("Payment is already added to this student.");

            // Ensures the payment is not already associated with another student
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

            // Ensures the payment exists in the student's list before removing it
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

            // Prevents duplicate courses from being added to the student
            if (_courses.Contains(course)) throw new ArgumentException("Course is already added to this student.");

            // If there are students already assigned to the course and the first student
            // is not the current student (this), it means this course is already assigned to a different student
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

            // Removes the course from the student's list, throwing an exception if it doesn't exist
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

        public void AddEnrollment(Enrollment enrollment)
        {
            if (enrollment == null) throw new ArgumentNullException(nameof(enrollment));

            if (enrollments.Contains(enrollment))
                throw new ArgumentException("Enrollment is already associated with this student.");

            enrollments.Add(enrollment);
        }

        public void RemoveEnrollment(Enrollment enrollment)
        {
            if (enrollment == null) throw new ArgumentNullException(nameof(enrollment));

            if (!enrollments.Remove(enrollment))
                throw new ArgumentException("Enrollment not found for this student.");
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