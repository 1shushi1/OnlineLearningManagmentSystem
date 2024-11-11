using NUnit.Framework;
using System;
using BYT_Project;

namespace BYT_Project.Tests
{
    [TestFixture]
    public class TimetableTests
    {
        [Test]
        public void TestGetCorrectTimetableInformation()
        {
            var schedule = new List<string> { "Mon 10:00 AM", "Wed 10:00 AM" };
            var timetable = new Timetable(1, DateTime.Now, DateTime.Now.AddMonths(1), schedule);

            Assert.That(timetable.TimetableID, Is.EqualTo(1));
            Assert.That(timetable.StartDate, Is.EqualTo(DateTime.Today).Within(TimeSpan.FromSeconds(1)));
            Assert.That(timetable.EndDate, Is.EqualTo(DateTime.Today.AddMonths(1)).Within(TimeSpan.FromSeconds(1)));
            Assert.That(timetable.Schedule, Is.EqualTo(schedule));
        }

        [Test]
        public void TestExceptionForInvalidTimetableID()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Timetable(-1, DateTime.Now, DateTime.Now.AddMonths(1), new List<string>()));
            Assert.That(ex.Message, Is.EqualTo("Timetable ID must be positive."));
        }

        [Test]
        public void TestExceptionForInvalidDateRange()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Timetable(1, DateTime.Now.AddMonths(1), DateTime.Now, new List<string>()));
            Assert.That(ex.Message, Is.EqualTo("Start date cannot be after end date."));
        }

        [Test]
        public void TestExceptionForEmptySchedule()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Timetable(1, DateTime.Now, DateTime.Now.AddMonths(1), new List<string>()));
            Assert.That(ex.Message, Is.EqualTo("Schedule cannot be empty."));
        }

        [Test]
        public void TestSaveAndLoadTimetables()
        {
            var schedule = new List<string> { "Mon 10:00 AM", "Wed 10:00 AM" };
            var timetable = new Timetable(1, DateTime.Now, DateTime.Now.AddMonths(1), schedule);

            Timetable.SaveTimetables();
            var success = Timetable.LoadTimetables();  // Load the timetables

            Assert.That(success, Is.True);
            Assert.That(Timetable.TimetablesList.Count, Is.EqualTo(1)); // Check that the timetable was saved and loaded
        }
    }
}
