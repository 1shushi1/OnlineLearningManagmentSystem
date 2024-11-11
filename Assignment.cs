using System;
using System.IO;
using System.Xml.Serialization;

namespace BYT_Project
{
    [Serializable]
    public class Assignment
    {
        private static List<Assignment> assignmentsList = new List<Assignment>();
        private int _assignmentID;
        private string _title;
        private string _description;
        private DateTime _dueDate;
        private int _maxScore;

        public int AssignmentID
        {
            get => _assignmentID;
            private set
            {
                if (value <= 0) throw new ArgumentException("Assignment ID must be positive.");
                _assignmentID = value;
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

        public DateTime DueDate
        {
            get => _dueDate;
            set
            {
                if (value < DateTime.Now) throw new ArgumentException("Due date cannot be in the past.");
                _dueDate = value;
            }
        }

        public int MaxScore
        {
            get => _maxScore;
            set
            {
                if (value <= 0) throw new ArgumentException("MaxScore must be positive.");
                _maxScore = value;
            }
        }

        public Assignment(int assignmentID, string title, string description, DateTime dueDate, int maxScore)
        {
            AssignmentID = assignmentID;
            Title = title;
            Description = description;
            DueDate = dueDate;
            MaxScore = maxScore;
            assignmentsList.Add(this);
        }

        public static void SaveAssignments(string path = "assignments.xml")
        {
            try
            {
                using (var writer = new StreamWriter(path))
                {
                    var serializer = new XmlSerializer(typeof(List<Assignment>));
                    serializer.Serialize(writer, assignmentsList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving assignments: {ex.Message}");
            }
        }

        public static bool LoadAssignments(string path = "assignments.xml")
        {
            try
            {
                using (var reader = new StreamReader(path))
                {
                    var serializer = new XmlSerializer(typeof(List<Assignment>));
                    assignmentsList = (List<Assignment>)serializer.Deserialize(reader);
                }
                return true;
            }
            catch (FileNotFoundException)
            {
                assignmentsList.Clear();
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading assignments: {ex.Message}");
                assignmentsList.Clear();
                return false;
            }
        }

        // Public static property to expose the assignmentsList for testing purposes
        public static List<Assignment> AssignmentsList => assignmentsList;
    }
}
