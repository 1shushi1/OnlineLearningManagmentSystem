using System;
using System.Xml.Serialization;

namespace BYT_Project
{
    [Serializable]
    public class Enrollment
    {
        private static List<Enrollment> enrollmentsList = new List<Enrollment>();
        private int _enrollmentID;
        private DateTime _enrollmentDate;
        private string _status;
        private int _totalScore;
        private string _gradeLetter;

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

        public Enrollment() { }
        public Enrollment(int enrollmentID, DateTime enrollmentDate, string status, int totalScore)
        {
            EnrollmentID = enrollmentID;
            _enrollmentDate = enrollmentDate;
            Status = status;
            TotalScore = totalScore;
            enrollmentsList.Add(this);
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
