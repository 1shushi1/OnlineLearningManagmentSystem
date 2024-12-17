using System;
using System.Xml.Serialization;

namespace BYT_Project
{
    [Serializable]
    public class SubmittedAssignment
    {
        private static List<SubmittedAssignment> submissionsList = new List<SubmittedAssignment>();
        private int _submissionID;
        private DateTime _submissionDate;
        private readonly Student _student;
        private readonly Assignment _assignment;

        public int SubmissionID
        {
            get => _submissionID;
            set
            {
                if (value <= 0) throw new ArgumentException("Submission ID must be positive.");
                _submissionID = value;
            }
        }

        public DateTime SubmissionDate
        {
            get => _submissionDate;
            set
            {
                if (value > DateTime.Now) throw new ArgumentException("Submission date cannot be in the future.");
                _submissionDate = value;
            }
        }

        public Student Student => _student;
        public Assignment Assignment => _assignment;

        public SubmittedAssignment() { }

        public SubmittedAssignment(Student student, Assignment assignment, int submissionID, DateTime submissionDate)
        {
            if (student == null) throw new ArgumentNullException(nameof(student), "Student cannot be null.");
            if (assignment == null) throw new ArgumentNullException(nameof(assignment), "Assignment cannot be null.");

            if (submissionsList.Exists(s => s.Student == student && s.Assignment == assignment))
            {
                throw new ArgumentException("This student has already assigned this task");
            }

            SubmissionID = submissionID;
            SubmissionDate = submissionDate;
            _student = student;
            _assignment = assignment;

            submissionsList.Add(this);

            if (!student.Assignments.Contains(assignment))
            {
                student.AddAssignment(assignment);
            }
            if (!assignment.Students.Contains(student)) {
                assignment.AddStudent(student);
            }
        }

        public static void RemoveSubmittedAssignment(SubmittedAssignment submittedAssignment)
        {
            if (submittedAssignment == null) throw new ArgumentNullException(nameof(submittedAssignment), "Submitted Assignment cannot be null.");
            if (!submissionsList.Remove(submittedAssignment))
            {
                throw new ArgumentException("Submitted Assignment does not exist in the global list.");
            }

            // Reverse disconnection
            if (submittedAssignment.Student.Assignments.Contains(submittedAssignment.Assignment))
            {
                submittedAssignment.Student.RemoveAssignment(submittedAssignment.Assignment);
            }
            if (submittedAssignment.Assignment.Students.Contains(submittedAssignment.Student))
            {
                submittedAssignment.Assignment.RemoveStudent(submittedAssignment.Student);
            }
        }

        public static void SaveSubmissions(string path = "submission.xml")
        {
            try
            {
                using (var writer = new StreamWriter(path))
                {
                    var serializer = new XmlSerializer(typeof(List<SubmittedAssignment>));
                    serializer.Serialize(writer, submissionsList);
                }
                Console.WriteLine("Submissions saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving submissions: {ex.Message}");
            }
        }

        public static bool LoadSubmissions(string path = "submission.xml")
        {
            try
            {
                using (var reader = new StreamReader(path))
                {
                    var serializer = new XmlSerializer(typeof(List<SubmittedAssignment>));
                    submissionsList = (List<SubmittedAssignment>)serializer.Deserialize(reader);
                }
                Console.WriteLine("Submissions loaded successfully.");
                return true;
            }
            catch (FileNotFoundException)
            {
                submissionsList = new List<SubmittedAssignment>();
                Console.WriteLine("No submissions file found. List cleared.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading submissions: {ex.Message}");
                submissionsList = new List<SubmittedAssignment>();
                return false;
            }
        }

        public static List<SubmittedAssignment> SubmissionsList => new List<SubmittedAssignment>(submissionsList);
    }
}
