using BYT_Project;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Project_Tests.Relation_Tests
{
    [TestFixture]
    public class LessonAssignmentRelationTests
    {
        [SetUp]
        public void Setup()
        {
            typeof(Lesson)
                .GetField("lessonsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Lesson>());

            typeof(Assignment)
                .GetField("assignmentsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Assignment>());
        }

        [Test]
        public void TestAddingAssignmentToLesson()
        {
            var lesson = new Lesson(1, "Lesson 1", "http://example.com", "Introduction");
            var assignment = new Assignment(1, "Math Homework", "Solve equations", DateTime.Now.AddDays(7), 100);

            lesson.AddAssignment(assignment);

            Assert.That(lesson.Assignments.Contains(assignment), "Assignment not added to lesson.");
            Assert.That(assignment.Lesson == lesson, "Lesson not assigned to assignment.");
        }

        [Test]
        public void TestRemovingAssignmentFromLesson()
        {
            var lesson = new Lesson(1, "Lesson 1", "http://example.com", "Introduction");
            var assignment = new Assignment(1, "Math Homework", "Solve equations", DateTime.Now.AddDays(7), 100);

            lesson.AddAssignment(assignment);
            lesson.RemoveAssignment(assignment);

            Assert.That(!lesson.Assignments.Contains(assignment), "Assignment not removed from lesson.");
            Assert.That(assignment.Lesson == null, "Lesson not removed from assignment.");
        }

        [Test]
        public void TestReverseConnectionIntegrity()
        {
            var lesson = new Lesson(1, "Lesson 1", "http://example.com", "Introduction");
            var assignment = new Assignment(1, "Math Homework", "Solve equations", DateTime.Now.AddDays(7), 100);

            // Add the assignment to the lesson
            lesson.AddAssignment(assignment);

            // Remove the assignment from the lesson
            lesson.RemoveAssignment(assignment);

            // Assert reverse connection integrity
            Assert.Multiple(() =>
            {
                Assert.That(!lesson.Assignments.Contains(assignment), "Assignment still connected to lesson.");
                Assert.That(assignment.Lesson == null, "Assignment still has a reference to the lesson.");
            });
        }


        [Test]
        public void TestErrorHandlingForAddingDuplicateAssignmentToLesson()
        {
            var lesson = new Lesson(1, "Lesson 1", "http://example.com", "Introduction");
            var assignment = new Assignment(1, "Math Homework", "Solve equations", DateTime.Now.AddDays(7), 100);

            lesson.AddAssignment(assignment);

            var ex = Assert.Throws<ArgumentException>(() => lesson.AddAssignment(assignment));
            Assert.That(ex.Message, Is.EqualTo("Assignment is already added to this lesson."));
        }

        [Test]
        public void TestErrorHandlingForAddingNullAssignment()
        {
            var lesson = new Lesson(1, "Lesson 1", "http://example.com", "Introduction");

            var ex = Assert.Throws<ArgumentException>(() => lesson.AddAssignment(null));
            Assert.That(ex.Message, Is.EqualTo("Assignment cannot be null."));
        }

        [Test]
        public void TestErrorHandlingForRemovingNonExistingAssignment()
        {
            var lesson = new Lesson(1, "Lesson 1", "http://example.com", "Introduction");
            var assignment = new Assignment(1, "Math Homework", "Solve equations", DateTime.Now.AddDays(7), 100);

            var ex = Assert.Throws<ArgumentException>(() => lesson.RemoveAssignment(assignment));
            Assert.That(ex.Message, Is.EqualTo("Assignment is not added to this lesson."));
        }

        [Test]
        public void TestErrorHandlingForAddingAssignmentAlreadyAssignedToAnotherLesson()
        {
            var lesson1 = new Lesson(1, "Lesson 1", "http://example.com", "Introduction");
            var lesson2 = new Lesson(2, "Lesson 2", "http://example.com", "Advanced");
            var assignment = new Assignment(1, "Math Homework", "Solve equations", DateTime.Now.AddDays(7), 100);

            lesson1.AddAssignment(assignment);

            var ex = Assert.Throws<ArgumentException>(() => lesson2.AddAssignment(assignment));
            Assert.That(ex.Message, Is.EqualTo("Assignment is already assigned to another lesson."));
        }

        [Test]
        public void TestCompositionDeletion()
        {
            var lesson = new Lesson(1, "Lesson 1", "http://example.com", "Introduction");
            var assignment1 = new Assignment(1, "Math Homework", "Solve equations", DateTime.Now.AddDays(7), 100);
            var assignment2 = new Assignment(2, "Science Project", "Build a model", DateTime.Now.AddDays(7), 100);

            lesson.AddAssignment(assignment1);
            lesson.AddAssignment(assignment2);

            Lesson.DeleteLesson(lesson);

            Assert.That(assignment1.Lesson == null, "Lesson not removed from assignment1 upon lesson deletion.");
            Assert.That(assignment2.Lesson == null, "Lesson not removed from assignment2 upon lesson deletion.");
        }

        [Test]
        public void TestAssignmentCannotExistWithoutLesson()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Assignment(1, "Math Homework", "Solve equations", DateTime.Now.AddDays(7), 100).AssignToLesson(null));
            Assert.That(ex.Message, Is.EqualTo("Lesson cannot be null."));
        }
    }
}
