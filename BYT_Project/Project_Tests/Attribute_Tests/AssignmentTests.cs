using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using BYT_Project;

namespace BYT_Project.Tests
{
    [TestFixture]
    public class AssignmentTests
    {
        [SetUp]
        public void SetUp()
        {
            typeof(Assignment)
                .GetField("assignmentsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Assignment>());

            if (File.Exists("assignment.xml"))
            {
                File.Delete("assignment.xml");
            }
        }

        [Test]
        public void TestGetCorrectAssignmentInformation()
        {
            var dueDate = DateTime.Now.AddDays(7);
            var assignment = new Assignment(1, "Homework 1", "Complete the exercises", dueDate, 100);

            Assert.That(assignment.AssignmentID, Is.EqualTo(1));
            Assert.That(assignment.Title, Is.EqualTo("Homework 1"));
            Assert.That(assignment.Description, Is.EqualTo("Complete the exercises"));
            Assert.That(assignment.DueDate, Is.EqualTo(dueDate).Within(TimeSpan.FromMilliseconds(10)));
            Assert.That(assignment.MaxScore, Is.EqualTo(100));
        }

        [Test]
        public void TestExceptionForInvalidAssignmentID()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Assignment(-1, "Homework 1", "Complete the exercises", DateTime.Now.AddDays(7), 100));
            Assert.That(ex.Message, Is.EqualTo("Assignment ID must be positive."));
        }

        [Test]
        public void TestSaveAndLoadAssignments()
        {
            var dueDate = DateTime.Now.AddDays(7);
            var assignment = new Assignment(1, "Homework 1", "Complete the exercises", dueDate, 100);

            Assert.That(Assignment.AssignmentsList.Count, Is.EqualTo(1));

            Assignment.SaveAssignments("assignment.xml");

            typeof(Assignment)
                .GetField("assignmentsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Assignment>());

            var success = Assignment.LoadAssignments("assignment.xml");

            Assert.That(success, Is.True);
            Assert.That(Assignment.AssignmentsList.Count, Is.EqualTo(1));
        }
    }
}
