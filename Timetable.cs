using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BYT_Project
{
    [Serializable]
    public class Timetable
    {
        private static List<Timetable> timetablesList = new List<Timetable>();
        private int _timetableID;
        private DateTime _startDate;
        private DateTime _endDate;
        private List<string> _schedule;

        public int TimetableID
        {
            get => _timetableID;
            private set
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
                if (value > _endDate) throw new ArgumentException("Start date cannot be after end date.");
                _startDate = value;
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (value < _startDate) throw new ArgumentException("End date cannot be before start date.");
                _endDate = value;
            }
        }

        public List<string> Schedule
        {
            get => _schedule;
            set
            {
                if (value == null || value.Count == 0) throw new ArgumentException("Schedule cannot be empty.");
                _schedule = value;
            }
        }

        public Timetable(int timetableID, DateTime startDate, DateTime endDate, List<string> schedule)
        {
            TimetableID = timetableID;
            StartDate = startDate;
            EndDate = endDate;
            Schedule = schedule;
            timetablesList.Add(this);
        }

        public static void SaveTimetables(string path = "timetables.xml")
        {
            try
            {
                using (var writer = new StreamWriter(path))
                {
                    var serializer = new XmlSerializer(typeof(List<Timetable>));
                    serializer.Serialize(writer, timetablesList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving timetables: {ex.Message}");
            }
        }

        public static bool LoadTimetables(string path = "timetables.xml")
        {
            try
            {
                using (var reader = new StreamReader(path))
                {
                    var serializer = new XmlSerializer(typeof(List<Timetable>));
                    timetablesList = (List<Timetable>)serializer.Deserialize(reader);
                }
                return true;
            }
            catch (FileNotFoundException)
            {
                timetablesList.Clear();
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading timetables: {ex.Message}");
                timetablesList.Clear();
                return false;
            }
        }

        public static List<Timetable> TimetablesList => timetablesList;
    }
}
