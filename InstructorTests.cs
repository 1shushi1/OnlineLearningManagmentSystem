using NUnit.Framework;
using System;
using BYT_Project;
using System.IO;

namespace BYT_Project.Tests
{
    [TestFixture]
    public class InstructorTests
    {
        [Test]
        public void TestGetCorrectInstructorInformation()
        {
            var instructor = new Instructor(1, "Mathematics", "MWF 10:00-12:00");
            Assert.That(instructor.InstructorID, Is.EqualTo(1));
            Assert.That(instructor.Expertise, Is.EqualTo("Mathematics"));
            Assert.That(instructor.OfficeHours, Is.EqualTo("MWF 10:00-12:00"));
        }

        [Test]
        public void TestExceptionForInvalidInstructorID()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Instructor(-1, "Mathematics"));
            Assert.That(ex.Message, Is.EqualTo("Instructor ID must be positive."));
        }

        [Test]
        public void TestExceptionForEmptyExpertise()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Instructor(1, ""));
            Assert.That(ex.Message, Is.EqualTo("Expertise cannot be empty."));
        }

        [Test]
        public void TestSaveAndLoadInstructors()
        {
            var instructor1 = new Instructor(1, "Mathematics", "MWF 10:00-12:00");
            var instructor2 = new Instructor(2, "Computer Science");

            Instructor.SaveInstructors("instructorsTest.xml");
            var success = Instructor.LoadInstructors("instructorsTest.xml");

            Assert.That(success, Is.True);  // Check if LoadInstructors() returns true
            Assert.That(Instructor.InstructorsList.Count, Is.EqualTo(2));  // Check that the list contains 2 instructors
        }

        [Test]
        public void TestPersistencyAfterLoad()
        {
            var instructor = new Instructor(1, "Mathematics", "MWF 10:00-12:00");
            Instructor.SaveInstructors("instructorsTest.xml");

            // Clear the current list and reload
            Instructor.LoadInstructors("instructorsTest.xml");

            Assert.That(Instructor.InstructorsList.Count, Is.GreaterThan(0));  // Ensure instructors were loaded from the file
        }
    }
}
