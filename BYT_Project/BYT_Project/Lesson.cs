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
        private List<Assignment> _assignments = new List<Assignment>();

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
        public Lesson(int lessonID, string lessonTitle, string videoURL, string lessonDescription)
        {
            LessonID = lessonID;
            LessonTitle = lessonTitle;
            VideoURL = videoURL;
            LessonDescription = lessonDescription;
            lessonsList.Add(this);
        }
        public void AssignToCourse(Course course)
        {
            if (course == null) throw new ArgumentException("Course cannot be null.");
            if (_course != null && _course != course)
                throw new ArgumentException("This lesson is already assigned to another course.");

            _course = course;

            if (!course.Lessons.Contains(this))
            {
                course.AddLesson(this);  
            }
        }

        public void RemoveCourse()
        {
            if (_course == null) throw new ArgumentException("Lesson is not assigned to any course.");

            var tempCourse = _course;
            _course = null;

            if (tempCourse.Lessons.Contains(this))
            {
                tempCourse.RemoveLesson(this); 
            }
        }
        public void UpdateCourse(Course newCourse)
        {
            if (newCourse == null) throw new ArgumentException("New course cannot be null.");

            if (_course != null) 
            {
                _course.RemoveLesson(this);
            }

            _course = newCourse; 
            newCourse.AddLesson(this);
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

            foreach (var assignment in new List<Assignment>(lesson._assignments))
            {
                lesson.RemoveAssignment(assignment);
            }

            lessonsList.Remove(lesson);
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
