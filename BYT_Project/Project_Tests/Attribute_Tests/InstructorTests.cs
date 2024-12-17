using NUnit.Framework;
using System;
using BYT_Project;

namespace BYT_Project.Tests
{
    [TestFixture]
    public class InstructorTests
    {
        [SetUp]
        public void SetUp()
        {
            typeof(Instructor)
                 .GetField("instructorsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                 ?.SetValue(null, new List<Instructor>());

            if (File.Exists("instructor.xml"))
            {
                File.Delete("instructor.xml");
            }
        }

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

            Assert.That(Instructor.InstructorsList.Count, Is.EqualTo(2));

            Instructor.SaveInstructors("instructor.xml");

            typeof(Instructor)
                 .GetField("instructorsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                 ?.SetValue(null, new List<Instructor>());

            var success = Instructor.LoadInstructors("instructor.xml");

            Assert.That(success, Is.True);  
            Assert.That(Instructor.InstructorsList.Count, Is.EqualTo(2));  
        }
    }
}
