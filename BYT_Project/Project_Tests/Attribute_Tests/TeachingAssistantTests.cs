using NUnit.Framework;
using System;
using BYT_Project;

namespace BYT_Project.Tests
{
    [TestFixture]
    public class TeachingAssistantTests
    {
        [SetUp]
        public void SetUp()
        {
            typeof(TeachingAssistant)
                .GetField("teachingAssistantsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<TeachingAssistant>());

            if (File.Exists("teachingAssistant.xml"))
            {
                File.Delete("teachingAssistant.xml");
            }
        }

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

            Assert.That(success, Is.True);  
            Assert.That(TeachingAssistant.TeachingAssistantsList.Count, Is.EqualTo(2));  
        }

        [Test]
        public void TestPersistencyAfterLoad()
        {
            var ta = new TeachingAssistant(1, 3);

            Assert.That(TeachingAssistant.TeachingAssistantsList.Count, Is.EqualTo(1));

            TeachingAssistant.SaveTeachingAssistants("teachingAssistant.xml");

            typeof(TeachingAssistant)
                .GetField("teachingAssistantsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<TeachingAssistant>());

            var success = TeachingAssistant.LoadTeachingAssistants("teachingAssistant.xml");

            Assert.That(success, Is.True);

            Assert.That(TeachingAssistant.TeachingAssistantsList.Count, Is.EqualTo(1));
        }
    }
}
