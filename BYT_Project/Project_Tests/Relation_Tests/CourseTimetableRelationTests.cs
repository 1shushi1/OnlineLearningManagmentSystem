using BYT_Project;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Project_Tests.Relation_Tests
{
    [TestFixture]
    public class CourseTimetableRelationTests
    {
        [SetUp]
        public void Setup()
        {
            typeof(Course)
                .GetField("coursesList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Course>());

            typeof(Timetable)
                .GetField("timetableList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Timetable>());
        }

        [Test]
        public void TestAddingTimetableToCourse()
        {
            var course = new Course(1, "Math 101", "Basic Math Course", 30);
            var timetable = new Timetable(1, DateTime.Now, DateTime.Now.AddDays(30), new List<string> { "Monday 9-11", "Wednesday 9-11" });

            course.SetTimetable(timetable);

            Assert.That(course.Timetable, Is.EqualTo(timetable), "Timetable not set in course.");
            Assert.That(timetable.Courses.Contains(course), "Course not added to timetable.");
        }

        [Test]
        public void TestRemovingTimetableFromCourse()
        {
            var course = new Course(1, "Math 101", "Basic Math Course", 30);
            var timetable = new Timetable(1, DateTime.Now, DateTime.Now.AddDays(30), new List<string> { "Monday 9-11", "Wednesday 9-11" });

            course.SetTimetable(timetable);
            course.RemoveTimetable();

            Assert.That(course.Timetable, Is.Null, "Timetable not removed from course.");
            Assert.That(!timetable.Courses.Contains(course), "Course not removed from timetable.");
        }

        [Test]
        public void TestReverseConnectionIntegrity()
        {
            var course = new Course(1, "Math 101", "Basic Math Course", 30);
            var timetable = new Timetable(1, DateTime.Now, DateTime.Now.AddDays(30), new List<string> { "Monday 9-11", "Wednesday 9-11" });

            course.SetTimetable(timetable);
            course.RemoveTimetable();

            Assert.That(timetable.Courses.Contains(course), Is.False, "Reverse connection not properly removed.");
        }

        [Test]
        public void TestErrorWhenAddingDuplicateCourseToTimetable()
        {
            var timetable = new Timetable(1, DateTime.Now, DateTime.Now.AddDays(30), new List<string> { "Monday 9-11", "Wednesday 9-11" });
            var course = new Course(1, "Math 101", "Basic Math Course", 30);

            timetable.AddCourse(course);

            var ex = Assert.Throws<ArgumentException>(() => timetable.AddCourse(course));
            Assert.That(ex.Message, Is.EqualTo("Course is already added to this timetable."));
        }

        [Test]
        public void TestErrorWhenRemovingNonExistingCourseFromTimetable()
        {
            var timetable = new Timetable(1, DateTime.Now, DateTime.Now.AddDays(30), new List<string> { "Monday 9-11", "Wednesday 9-11" });
            var course = new Course(1, "Math 101", "Basic Math Course", 30);

            var ex = Assert.Throws<ArgumentException>(() => timetable.RemoveCourse(course));
            Assert.That(ex.Message, Is.EqualTo("Course is not associated with this timetable."));
        }

        [Test]
        public void TestUpdatingTimetableForCourse()
        {
            var course = new Course(1, "Math 101", "Basic Math Course", 30);
            var timetable1 = new Timetable(1, DateTime.Now, DateTime.Now.AddDays(30), new List<string> { "Monday 9-11" });
            var timetable2 = new Timetable(2, DateTime.Now.AddDays(1), DateTime.Now.AddDays(31), new List<string> { "Tuesday 10-12" });

            course.SetTimetable(timetable1);
            course.SetTimetable(timetable2);

            Assert.That(course.Timetable, Is.EqualTo(timetable2), "Timetable not updated in course.");
            Assert.That(timetable1.Courses.Contains(course), Is.False, "Course not removed from old timetable.");
            Assert.That(timetable2.Courses.Contains(course), Is.True, "Course not added to new timetable.");
        }

        [Test]
        public void TestAddingCourseToTimetableReverseCheck()
        {
            var course = new Course(1, "Math 101", "Basic Math Course", 30);
            var timetable = new Timetable(1, DateTime.Now, DateTime.Now.AddDays(30), new List<string> { "Monday 9-11" });

            timetable.AddCourse(course);

            Assert.Multiple(() =>
            {
                Assert.That(course.Timetable, Is.EqualTo(timetable), "Reverse connection not correctly established.");
                Assert.That(timetable.Courses.Contains(course), "Timetable is not associated with the course.");
            });
        }
    }
}
