using System;
using System.Xml.Serialization;

namespace BYT_Project
{
    [Serializable]
    public class Lesson
    {
        private static List<Lesson> lessonsList = new List<Lesson>();
        private int _lessonID;
        private string _lessonTitle;
        private string _videoURL;
        private string _lessonDescription;
        private Course _course;
        private List<Assignment> _assignments = new List<Assignment>(); // zero-to-many relation with Assignment, Lesson composes of Assignment
        private List<Quiz> quizzes = new List<Quiz>(); // zero-to-many relation with Quiz, Lesson composes of Quiz


        public IReadOnlyList<Quiz> Quizzes => quizzes.AsReadOnly();

        public int LessonID
        {
            get => _lessonID;
            set
            {
                if (value <= 0) throw new ArgumentException("Lesson ID must be positive.");
                _lessonID = value;
            }
        }

        public string LessonTitle
        {
            get => _lessonTitle;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Lesson title cannot be empty.");
                _lessonTitle = value;
            }
        }

        public string VideoURL
        {
            get => _videoURL;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Video URL cannot be empty.");
                _videoURL = value;
            }
        }

        public string LessonDescription
        {
            get => _lessonDescription;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Lesson description cannot be empty.");
                _lessonDescription = value;
            }
        }

        public Course Course => _course;
        public IReadOnlyList<Assignment> Assignments => _assignments.AsReadOnly();

        public Lesson() { }
        public Lesson(int lessonID, string lessonTitle, string videoURL, string lessonDescription, Course course)
        {
            LessonID = lessonID;
            LessonTitle = lessonTitle;
            VideoURL = videoURL;
            LessonDescription = lessonDescription;
            AssignToCourse(course); // Automatically establishes composition
            lessonsList.Add(this);
        }
        public void AssignToCourse(Course course)
        {
            if (course == null) throw new ArgumentException("Course cannot be null.");

            // Prevents reassigning a lesson to a different course without explicitly removing the current one
            if (_course != null && _course != course)
                throw new ArgumentException("This lesson is already assigned to another course.");

            _course = course; // sets the course reference for the Lesson

            if (!course.Lessons.Contains(this))
            {
                course.AddLesson(this);   // reverse connection
            }
        }

        public void RemoveCourse()
        {
            // Ensures the lesson is currently assigned to a course
            if (_course == null) throw new ArgumentException("Lesson is not assigned to any course.");

            // Temporarily stores the current course and clears the lesson's reference
            var tempCourse = _course;
            _course = null;

            if (tempCourse.Lessons.Contains(this))
            {
                tempCourse.RemoveLesson(this);  // reverse disconnection
            }
        }
        public void UpdateCourse(Course newCourse)
        {
            if (newCourse == null) throw new ArgumentException("New course cannot be null.");

            // Removes the current association with the existing course
            if (_course != null) 
            {
                _course.RemoveLesson(this);
            }

            // sets the new course reference for Lesson
            _course = newCourse; 
            newCourse.AddLesson(this); // reverse connection with new reference
        }

        public void AddAssignment(Assignment assignment)
        {
            if (assignment == null) throw new ArgumentException("Assignment cannot be null.");
            if (_assignments.Contains(assignment)) throw new ArgumentException("Assignment is already added to this lesson.");

            // Check for ownership conflict
            if (assignment.Lesson != null && assignment.Lesson != this)
                throw new ArgumentException("Assignment is already assigned to another lesson.");

            _assignments.Add(assignment);

            // Set reverse connection if not already set
            if (assignment.Lesson != this)
            {
                assignment.AssignToLesson(this); // Ensure reverse connection
            }
        }


        public void RemoveAssignment(Assignment assignment)
        {
            if (assignment == null) throw new ArgumentException("Assignment cannot be null.");

            // Ensures the assignment is in the lesson's list before attempting to remove it
            if (!_assignments.Remove(assignment)) throw new ArgumentException("Assignment is not added to this lesson.");

            // Remove reverse connection
            if (assignment.Lesson == this)
            {
                assignment.RemoveLesson(); // Ensure reverse disconnection
            }
        }

        public static void DeleteLesson(Lesson lesson)
        {
            if (lesson == null) throw new ArgumentException("Lesson cannot be null.");

            // Delete associated assignments
            foreach (var assignment in new List<Assignment>(lesson.Assignments))
            {
                assignment.RemoveLesson();
                Assignment.AssignmentsList.Remove(assignment); // Remove globally
            }

            // Delete associated quizzes
            foreach (var quiz in new List<Quiz>(lesson.Quizzes))
            {
                quiz.RemoveLesson();
                Quiz.QuizzesList.Remove(quiz); // Remove globally
            }

            lessonsList.Remove(lesson); // Remove the lesson itself
        }


        public void AddQuiz(Quiz quiz)
        {
            if (quiz == null) throw new ArgumentNullException(nameof(quiz));

            // Prevents adding the same quiz multiple times to the lesson
            if (quizzes.Contains(quiz)) throw new ArgumentException("Quiz is already added to this lesson.");

            // Ensures the quiz is not already linked to a different lesson
            if (quiz.Lesson != null && quiz.Lesson != this)
                throw new ArgumentException("Quiz is already associated with another lesson.");

            quizzes.Add(quiz);
            quiz.AssignToLesson(this);
        }

        public void RemoveQuiz(Quiz quiz)
        {
            if (quiz == null) throw new ArgumentNullException(nameof(quiz));

            // Ensures the quiz exists in the lesson's list before attempting to remove it
            if (!quizzes.Remove(quiz)) throw new ArgumentException("Quiz is not associated with this lesson.");

            quiz.RemoveLesson();
        }


        public static void SaveLessons(string path = "lesson.xml")
        {
            try
            {
                using (var writer = new StreamWriter(path))
                {
                    var serializer = new XmlSerializer(typeof(List<Lesson>));
                    serializer.Serialize(writer, lessonsList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving lessons: {ex.Message}");
            }
        }

        public static bool LoadLessons(string path = "lesson.xml")
        {
            try
            {
                using (var reader = new StreamReader(path))
                {
                    var serializer = new XmlSerializer(typeof(List<Lesson>));
                    lessonsList = (List<Lesson>)serializer.Deserialize(reader);
                }
                return true;
            }
            catch (FileNotFoundException)
            {
                lessonsList = new List<Lesson>();
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading lessons: {ex.Message}");
                lessonsList = new List<Lesson>();
                return false;
            }
        }
        public static List<Lesson> LessonsList => new List<Lesson>(lessonsList);
    }
}
