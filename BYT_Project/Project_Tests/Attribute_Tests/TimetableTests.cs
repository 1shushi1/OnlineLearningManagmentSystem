using NUnit.Framework;
using System;
using BYT_Project;

namespace BYT_Project.Tests
{
    [TestFixture]
    public class TimetableTests
    {
        [SetUp]
        public void SetUp()
        {
            typeof(Timetable)
                .GetField("timetableList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Timetable>());

            if (File.Exists("timetable.xml"))
            {
                File.Delete("timetable.xml");
            }
        }

        [Test]
        public void TestGetCorrectTimetableInformation()
        {
            var schedule = new List<string> { "Mon 10:00 AM", "Wed 10:00 AM" };
            var timetable = new Timetable(1, DateTime.Now.Date, DateTime.Now.AddMonths(1).Date, schedule);

            Assert.That(timetable.TimetableID, Is.EqualTo(1));
            Assert.That(timetable.StartDate, Is.EqualTo(DateTime.Today));
            Assert.That(timetable.EndDate, Is.EqualTo(DateTime.Today.AddMonths(1)));
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
            var validSchedule = new List<string> { "Mon 10:00 AM" };
           
            var ex = Assert.Throws<ArgumentException>(() => new Timetable(1, DateTime.Now.AddMonths(1), DateTime.Now, validSchedule));

           
            Assert.That(ex.Message, Is.EqualTo("End date cannot be before start date."));
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

            Assert.That(Timetable.TimetableList.Count, Is.EqualTo(1)); 

            Timetable.SaveTimetables("timetable.xml");

            typeof(Timetable)
                .GetField("timetableList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Timetable>());

            var success = Timetable.LoadTimetables("timetable.xml");  

            Assert.That(success, Is.True);
            Assert.That(Timetable.TimetableList.Count, Is.EqualTo(1)); 
        }
    }
}
