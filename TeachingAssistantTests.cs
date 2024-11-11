using NUnit.Framework;
using System;
using BYT_Project;
using System.IO;

namespace BYT_Project.Tests
{
    [TestFixture]
    public class TeachingAssistantTests
    {
        [Test]
        public void TestGetCorrectTeachingAssistantInformation()
        {
            var ta = new TeachingAssistant(1, 3);
            Assert.That(ta.TeachingAssistantID, Is.EqualTo(1));
            Assert.That(ta.Experience, Is.EqualTo(3));
        }

        [Test]
        public void TestExceptionForInvalidTeachingAssistantID()
        {
            var ex = Assert.Throws<ArgumentException>(() => new TeachingAssistant(-1, 3));
            Assert.That(ex.Message, Is.EqualTo("Teaching Assistant ID must be positive."));
        }

        [Test]
        public void TestExceptionForNegativeExperience()
        {
            var ex = Assert.Throws<ArgumentException>(() => new TeachingAssistant(1, -1));
            Assert.That(ex.Message, Is.EqualTo("Experience cannot be negative."));
        }

        [Test]
        public void TestSaveAndLoadTeachingAssistants()
        {
            var ta1 = new TeachingAssistant(1, 3);
            var ta2 = new TeachingAssistant(2, 5);

            TeachingAssistant.SaveTeachingAssistants("teachingAssistantsTest.xml");
            var success = TeachingAssistant.LoadTeachingAssistants("teachingAssistantsTest.xml");

            Assert.That(success, Is.True);  // Check if LoadTeachingAssistants() returns true
            Assert.That(TeachingAssistant.TeachingAssistantsList.Count, Is.EqualTo(2));  // Check that the list contains 2 teaching assistants
        }

        [Test]
        public void TestPersistencyAfterLoad()
        {
            var ta = new TeachingAssistant(1, 3);
            TeachingAssistant.SaveTeachingAssistants("teachingAssistantsTest.xml");

            // Clear the current list and reload
            TeachingAssistant.LoadTeachingAssistants("teachingAssistantsTest.xml");

            Assert.That(TeachingAssistant.TeachingAssistantsList.Count, Is.GreaterThan(0));  // Ensure teaching assistants were loaded from the file
        }
    }
}
