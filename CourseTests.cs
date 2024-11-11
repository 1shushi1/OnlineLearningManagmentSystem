using NUnit.Framework;
using System;
using BYT_Project;

namespace BYT_Project.Tests
{
    [TestFixture]
    public class CourseTests
    {
        [Test]
        public void TestGetCorrectCourseInformation()
        {
            var course = new Course(1, "C# Programming", "Learn C# programming", 100);

            Assert.That(course.CourseID, Is.EqualTo(1));
            Assert.That(course.Title, Is.EqualTo("C# Programming"));
            Assert.That(course.Description, Is.EqualTo("Learn C# programming"));
            Assert.That(course.MaxEnrollment, Is.EqualTo(100));
        }

        [Test]
        public void TestExceptionForInvalidCourseID()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Course(-1, "C# Programming", "Learn C# programming", 100));
            Assert.That(ex.Message, Is.EqualTo("Course ID must be positive."));
        }

        [Test]
        public void TestExceptionForInvalidMaxEnrollment()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Course(1, "C# Programming", "Learn C# programming", -1));
            Assert.That(ex.Message, Is.EqualTo("Max enrollment must be greater than 0."));
        }

        [Test]
        public void TestSaveAndLoadCourses()
        {
            var course = new Course(1, "C# Programming", "Learn C# programming", 100);

            Course.SaveCourses();
            var success = Course.LoadCourses();

            Assert.That(success, Is.True);
            Assert.That(Course.CoursesList.Count, Is.EqualTo(1)); // Ensure course is saved and loaded
        }
    }
}
