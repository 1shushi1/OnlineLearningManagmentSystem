using System;
using System.Collections.Generic;
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
        private List<Student> _students = new List<Student>(); // one-to-many relation with Student
        private Lesson _lesson; // one-to-one composition relation with Lesson, Lesson composes of Assignment

        public int AssignmentID
        {
            get => _assignmentID;
            set
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
        //  property to expose associated students
        public IReadOnlyList<Student> Students => _students.AsReadOnly();

        // property to expose the associated lesson
        public Lesson Lesson => _lesson;

        public Assignment() { }

        public Assignment(int assignmentID, string title, string description, DateTime dueDate, int maxScore)
        {
            AssignmentID = assignmentID;
            Title = title;
            Description = description;
            DueDate = dueDate;
            MaxScore = maxScore;
            assignmentsList.Add(this);
        }

        public void AddStudent(Student student)
        {
            if (student == null) throw new ArgumentException("Student cannot be null.");
            
            // Check for duplicate relationship
            if (_students.Contains(student)) throw new ArgumentException("Student is already added to this assignment.");
            _students.Add(student);

            // Bidirectional connection: Ensures the student also references this assignment
            if (!student.Assignments.Contains(this))
            {
                student.AddAssignment(this); 
            }
        }

        public void RemoveStudent(Student student)
        {
            if (student == null) throw new ArgumentException("Student cannot be null.");

            // Attempt to remove the user; if not found, throw an exception
            if (!_students.Remove(student)) throw new ArgumentException("Student is not added to this assignment.");


            // Bidirectional connection: Ensures the student removes this assignment
            if (student.Assignments.Contains(this))
            {
                student.RemoveAssignment(this);
            }
        }

        public void UpdateStudent(Student oldStudent, Student newStudent)
        {
            if (oldStudent == null || newStudent == null)
                throw new ArgumentException("Both old and new students must be provided.");

            RemoveStudent(oldStudent);
            AddStudent(newStudent);
        }

        public void AssignToLesson(Lesson lesson)
        {
            if (lesson == null) throw new ArgumentException("Lesson cannot be null.");
            if (_lesson == lesson) return; // Prevent redundant assignment

            // Remove from the previous lesson if necessary
            if (_lesson != null && _lesson != lesson)
            {
                _lesson.RemoveAssignment(this); 
            }

            _lesson = lesson;

            // Add to the new lesson's assignments if not already added
            if (!lesson.Assignments.Contains(this))
            {
                lesson.AddAssignment(this); // Reverse connection
            }
        }


        public void RemoveLesson()
        {
            if (_lesson == null) throw new ArgumentException("Assignment is not assigned to any lesson.");

            var tempLesson = _lesson;
            _lesson = null;

            // Remove from the lesson's assignments list
            if (tempLesson.Assignments.Contains(this))
            {
                tempLesson.RemoveAssignment(this); // Ensure reverse disconnection
            }
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
                assignmentsList = new List<Assignment>();
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading assignments: {ex.Message}");
                assignmentsList = new List<Assignment>();
                return false;
            }
        }

        public static List<Assignment> AssignmentsList => new List<Assignment>(assignmentsList);
    }
}
