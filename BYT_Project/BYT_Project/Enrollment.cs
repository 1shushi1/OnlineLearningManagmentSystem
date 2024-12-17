using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BYT_Project
{
    [Serializable]
    public class Enrollment
    {
        private static List<Enrollment> enrollmentsList = new List<Enrollment>();
        private int _enrollmentID;
        private readonly DateTime _enrollmentDate;
        private string _status;
        private int _totalScore;
        private string _gradeLetter;
        private Certificate? _certificate; // zero-to-one relation with Certificate
        private readonly Student _student;
        private readonly Course _course;

        public int EnrollmentID
        {
            get => _enrollmentID;
            set
            {
                if (value <= 0) throw new ArgumentException("Enrollment ID must be positive.");
                _enrollmentID = value;
            }
        }

        public DateTime EnrollmentDate => _enrollmentDate;

        public string Status
        {
            get => _status;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Status cannot be empty.");
                _status = value;
            }
        }

        public int TotalScore
        {
            get => _totalScore;
            set
            {
                if (value < 0 || value > 100) throw new ArgumentException("Total score must be between 0 and 100.");
                _totalScore = value;
                CalculateGrade();
            }
        }

        public string GradeLetter => _gradeLetter;

        public Certificate? Certificate => _certificate; // Getter for Certificate
        public Student Student => _student;
        public Course Course => _course;    

        public Enrollment() { }

        public Enrollment(Student student, Course course,int enrollmentID , DateTime enrollmentDate, string status, int totalScore)
        {
            if (student == null) throw new ArgumentNullException(nameof(student), "Student cannot be null.");
            if (course == null) throw new ArgumentNullException(nameof(course), "Course cannot be null.");
            if (totalScore < 0 || totalScore > 100) throw new ArgumentException("Total score must be between 0 and 100.");

            // Prevent duplicate enrollments
            if (enrollmentsList.Exists(e => e.Student == student && e.Course == course))
            {
                throw new ArgumentException("This student is already enrolled in the course.");
            }

            _enrollmentID = enrollmentID;
            _enrollmentDate = enrollmentDate;
            Status = status;
            TotalScore = totalScore;
            _student = student;
            _course = course;

            enrollmentsList.Add(this);

            if (!student.Courses.Contains(course))
            {
                student.AddCourse(course);
            }
            if (!course.Students.Contains(student))
            {
                course.AddStudent(student);
            }
        }

        public static void RemoveEnrollment(Enrollment enrollment)
        {
            if (enrollment == null) throw new ArgumentNullException(nameof(enrollment), "Enrollment cannot be null.");
            if (!enrollmentsList.Remove(enrollment))
            {
                throw new ArgumentException("Enrollment does not exist in the global list.");
            }

            // Reverse disconnection
            if (enrollment.Student.Courses.Contains(enrollment.Course))
            {
                enrollment.Student.RemoveCourse(enrollment.Course);
            }
            if (enrollment.Course.Students.Contains(enrollment.Student))
            {
                enrollment.Course.RemoveStudent(enrollment.Student);
            }
        }


        private void CalculateGrade()
        {
            if (_totalScore >= 90) _gradeLetter = "A";
            else if (_totalScore >= 80) _gradeLetter = "B";
            else if (_totalScore >= 70) _gradeLetter = "C";
            else if (_totalScore >= 60) _gradeLetter = "D";
            else if (_totalScore >= 50) _gradeLetter = "E";
            else _gradeLetter = "F";
        }

        public void SetCertificate(Certificate certificate)
        {
            if (_certificate == certificate) return;

            // Remove from the old certificate only if it is truly associated
            if (_certificate != null && _certificate.Enrollments.Contains(this))
            {
                _certificate.RemoveEnrollment(this);
            }

            _certificate = certificate;

            if (certificate != null && !certificate.Enrollments.Contains(this))
            {
                certificate.AddEnrollment(this);
            }
        }

        public void RemoveCertificate()
        {
            if (_certificate == null) return;

            var tempCertificate = _certificate;
            _certificate = null;

            // Reverse connection removal
            if (tempCertificate.Enrollments.Contains(this))
            {
                tempCertificate.RemoveEnrollment(this);
            }
        }

        public static void SaveEnrollments(string path = "enrollment.xml")
        {
            try
            {
                using (var writer = new StreamWriter(path))
                {
                    var serializer = new XmlSerializer(typeof(List<Enrollment>));
                    serializer.Serialize(writer, enrollmentsList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving enrollments: {ex.Message}");
            }
        }

        public static bool LoadEnrollments(string path = "enrollment.xml")
        {
            try
            {
                using (var reader = new StreamReader(path))
                {
                    var serializer = new XmlSerializer(typeof(List<Enrollment>));
                    enrollmentsList = (List<Enrollment>)serializer.Deserialize(reader);
                }
                return true;
            }
            catch (FileNotFoundException)
            {
                enrollmentsList = new List<Enrollment>();
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading enrollments: {ex.Message}");
                enrollmentsList = new List<Enrollment>();
                return false;
            }
        }

        public static List<Enrollment> EnrollmentsList => new List<Enrollment>(enrollmentsList);
    }
}