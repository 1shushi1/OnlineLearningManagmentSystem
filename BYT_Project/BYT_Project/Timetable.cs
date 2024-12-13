using System;
using System.Xml.Serialization;

namespace BYT_Project
{
    [Serializable]
    public class Timetable
    {
        private static List<Timetable> timetableList = new List<Timetable>();
        private int _timetableID;
        private DateTime _startDate;
        private DateTime _endDate;
        private List<string> _schedule = new List<string>();
        private List<Course> _courses = new List<Course>(); 
        public IReadOnlyList<Course> Courses => _courses.AsReadOnly();

        public int TimetableID
        {
            get => _timetableID;
            set
            {
                if (value <= 0) throw new ArgumentException("Timetable ID must be positive.");
                _timetableID = value;
            }
        }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
         
                if (_endDate != default(DateTime) && value > _endDate)
                {
                    throw new ArgumentException("Start date cannot be after end date.");
                }
                _startDate = value;
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                
                if (_startDate != default(DateTime) && value < _startDate)
                {
                    throw new ArgumentException("End date cannot be before start date.");
                }
                _endDate = value;
            }
        }

        [XmlArray("Schedule")]
        [XmlArrayItem("Entry")]
        public List<string> Schedule
        {
            get => _schedule;
            set
            {
                if (value == null || value.Count == 0) throw new ArgumentException("Schedule cannot be empty.");
                _schedule = value;
            }
        }

        public Timetable() { }
        public Timetable(int timetableID, DateTime startDate, DateTime endDate, List<string> schedule)
        {
         
            TimetableID = timetableID;
            StartDate = startDate;
            EndDate = endDate;
            Schedule = schedule;
            timetableList.Add(this);
        }


        public static void SaveTimetables(string path = "timetable.xml")
        {
            try
            {
                using (var writer = new StreamWriter(path))
                {
                    var serializer = new XmlSerializer(typeof(List<Timetable>));
                    serializer.Serialize(writer, timetableList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving timetables: {ex.Message}");
            }
        }


        public static bool LoadTimetables(string path = "timetable.xml")
        {
            try
            {
                using (var reader = new StreamReader(path))
                {
                    var serializer = new XmlSerializer(typeof(List<Timetable>));
                    timetableList = (List<Timetable>)serializer.Deserialize(reader);
                }
                return true;
            }
            catch (FileNotFoundException)
            {
                timetableList = new List<Timetable>();
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading timetables: {ex.Message}");
                timetableList = new List<Timetable>();
                return false;
            }

        }

        public void AddCourse(Course course)
        {
            if (course == null) throw new ArgumentNullException(nameof(course));
            if (_courses.Contains(course))
            {
                throw new ArgumentException("Course is already added to this timetable.");
            }

            _courses.Add(course);

            if (course.Timetable != this)
            {
                course.SetTimetable(this);
            }
        }


        public void RemoveCourse(Course course)
        {
            if (course == null) throw new ArgumentNullException(nameof(course));
            if (!_courses.Remove(course))
            {
                throw new ArgumentException("Course is not associated with this timetable.");
            }

            if (course.Timetable == this)
            {
                course.RemoveTimetable();
            }
        }


        public static List<Timetable> TimetableList => new List<Timetable>(timetableList);
    }
}
