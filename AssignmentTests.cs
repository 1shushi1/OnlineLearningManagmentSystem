using NUnit.Framework;
using System;
using BYT_Project;

namespace BYT_Project.Tests
{
    [TestFixture]
    public class AssignmentTests
    {
        [Test]
        public void TestGetCorrectAssignmentInformation()
        {
            var assignment = new Assignment(1, "Homework 1", "Complete the exercises", DateTime.Now.AddDays(7), 100);

            Assert.That(assignment.AssignmentID, Is.EqualTo(1));
            Assert.That(assignment.Title, Is.EqualTo("Homework 1"));
            Assert.That(assignment.Description, Is.EqualTo("Complete the exercises"));
            Assert.That(assignment.DueDate, Is.EqualTo(DateTime.Now.AddDays(7)));
            Assert.That(assignment.MaxScore, Is.EqualTo(100));
        }

        [Test]
        public void TestExceptionForInvalidAssignmentID()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Assignment(-1, "Homework 1", "Complete the exercises", DateTime.Now.AddDays(7), 100));
            Assert.That(ex.Message, Is.EqualTo("Assignment ID must be positive."));
        }

        [Test]
        public void TestExceptionForEmptyAssignmentTitle()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Assignment(1, "", "Complete the exercises", DateTime.Now.AddDays(7), 100));
            Assert.That(ex.Message, Is.EqualTo("Assignment title cannot be empty."));
        }

        [Test]
        public void TestExceptionForDueDateInPast()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Assignment(1, "Homework 1", "Complete the exercises", DateTime.Now.AddDays(-1), 100));
            Assert.That(ex.Message, Is.EqualTo("Due date must be in the future."));
        }

        [Test]
        public void TestSaveAndLoadAssignments()
        {
            var assignment = new Assignment(1, "Homework 1", "Complete the exercises", DateTime.Now.AddDays(7), 100);

            Assignment.SaveAssignments();
            var success = Assignment.LoadAssignments();

            Assert.That(success, Is.True);
            Assert.That(Assignment.AssignmentsList.Count, Is.EqualTo(1)); // Ensure assignment is saved and loaded
        }
    }
}
