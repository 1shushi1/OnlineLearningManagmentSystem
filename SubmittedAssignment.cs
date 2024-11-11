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

        public int SubmissionID
        {
            get => _submissionID;
            private set
            {
                if (value <= 0) throw new ArgumentException("Submission ID must be positive.");
                _submissionID = value;
            }
        }

        public DateTime SubmissionDate
        {
            get => _submissionDate;
            private set
            {
                if (value > DateTime.Now) throw new ArgumentException("Submission date cannot be in the future.");
                _submissionDate = value;
            }
        }

        public SubmittedAssignment(int submissionID, DateTime submissionDate)
        {
            SubmissionID = submissionID;
            SubmissionDate = submissionDate;
            submissionsList.Add(this);
        }

        public static void SaveSubmissions(string path = "submissions.xml")
        {
            try
            {
                using (var writer = new StreamWriter(path))
                {
                    var serializer = new XmlSerializer(typeof(List<SubmittedAssignment>));
                    serializer.Serialize(writer, submissionsList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving submissions: {ex.Message}");
            }
        }

        public static bool LoadSubmissions(string path = "submissions.xml")
        {
            try
            {
                using (var reader = new StreamReader(path))
                {
                    var serializer = new XmlSerializer(typeof(List<SubmittedAssignment>));
                    submissionsList = (List<SubmittedAssignment>)serializer.Deserialize(reader);
                }
                return true;
            }
            catch (FileNotFoundException)
            {
                submissionsList.Clear();
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading submissions: {ex.Message}");
                submissionsList.Clear();
                return false;
            }
        }

        public static List<SubmittedAssignment> SubmissionsList => submissionsList;
    }
}
