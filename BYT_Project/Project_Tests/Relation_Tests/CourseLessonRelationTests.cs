using BYT_Project;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Project_Tests.Relation_Tests
{
    [TestFixture]
    public class CourseLessonRelationTests
    {
        [SetUp]
        public void Setup()
        {
            typeof(Course)
                .GetField("coursesList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Course>());

            typeof(Lesson)
                .GetField("lessonsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Lesson>());
        }

        [Test]
        public void TestAddingLessonToCourse()
        {
            var course = new Course(1, "Math 101", "Basic Math Course", 30);
            var lesson = new Lesson(1, "Lesson 1", "http://video.url", "Lesson 1 Description");

            course.AddLesson(lesson);

            Assert.That(course.Lessons.Contains(lesson), "Lesson not added to course.");
            Assert.That(lesson.Course == course, "Course not set in lesson.");
        }

        [Test]
        public void TestRemovingLessonFromCourse()
        {
            var course = new Course(1, "Math 101", "Basic Math Course", 30);
            var lesson = new Lesson(1, "Lesson 1", "http://video.url", "Lesson 1 Description");

            course.AddLesson(lesson);
            course.RemoveLesson(lesson);

            Assert.That(!course.Lessons.Contains(lesson), "Lesson not removed from course.");
            Assert.That(lesson.Course == null, "Course not removed from lesson.");
        }

        [Test]
        public void TestReverseConnectionIntegrity()
        {
            var course = new Course(1, "Math 101", "Basic Math Course", 30);
            var lesson = new Lesson(1, "Lesson 1", "http://video.url", "Lesson 1 Description");

            course.AddLesson(lesson);
            course.RemoveLesson(lesson);

            Assert.That(lesson.Course == null, "Reverse connection not properly removed.");
        }

        [Test]
        public void TestDeletingCourseDeletesLessons()
        {
            var course = new Course(1, "Math 101", "Basic Math Course", 30);
            var lesson1 = new Lesson(1, "Lesson 1", "http://video.url", "Lesson 1 Description");
            var lesson2 = new Lesson(2, "Lesson 2", "http://video.url", "Lesson 2 Description");

            course.AddLesson(lesson1);
            course.AddLesson(lesson2);

            course.RemoveLesson(lesson1);
            course.RemoveLesson(lesson2);

            Assert.That(course.Lessons.Count, Is.EqualTo(0), "Lessons not deleted when course deleted.");
            Assert.That(lesson1.Course == null, "Lesson 1 not removed from course.");
            Assert.That(lesson2.Course == null, "Lesson 2 not removed from course.");
        }

        [Test]
        public void TestErrorWhenAddingNullLessonToCourse()
        {
            var course = new Course(1, "Math 101", "Basic Math Course", 30);

            var ex = Assert.Throws<ArgumentException>(() => course.AddLesson(null));
            Assert.That(ex.Message, Is.EqualTo("Lesson cannot be null."));
        }

        [Test]
        public void TestErrorWhenAddingDuplicateLessonToCourse()
        {
            var course = new Course(1, "Math 101", "Basic Math Course", 30);
            var lesson = new Lesson(1, "Lesson 1", "http://video.url", "Lesson 1 Description");

            course.AddLesson(lesson);

            var ex = Assert.Throws<ArgumentException>(() => course.AddLesson(lesson));
            Assert.That(ex.Message, Is.EqualTo("Lesson is already added to this course."));
        }

        [Test]
        public void TestErrorWhenRemovingNonExistingLessonFromCourse()
        {
            var course = new Course(1, "Math 101", "Basic Math Course", 30);
            var lesson = new Lesson(1, "Lesson 1", "http://video.url", "Lesson 1 Description");

            var ex = Assert.Throws<ArgumentException>(() => course.RemoveLesson(lesson));
            Assert.That(ex.Message, Is.EqualTo("Lesson is not added to this course."));
        }

        [Test]
        public void TestErrorWhenAssigningLessonToAnotherCourse()
        {
            var course1 = new Course(1, "Math 101", "Basic Math Course", 30);
            var course2 = new Course(2, "Science 101", "Basic Science Course", 25);
            var lesson = new Lesson(1, "Lesson 1", "http://video.url", "Lesson 1 Description");

            course1.AddLesson(lesson);

            var ex = Assert.Throws<ArgumentException>(() => course2.AddLesson(lesson));
            Assert.That(ex.Message, Is.EqualTo("This lesson is already assigned to another course."));
        }

        [Test]
        public void TestUpdatingLessonCourse()
        {
            var course1 = new Course(1, "Math 101", "Basic Math Course", 30);
            var course2 = new Course(2, "Science 101", "Basic Science Course", 25);
            var lesson = new Lesson(1, "Lesson 1", "http://video.url", "Lesson 1 Description");

            course1.AddLesson(lesson);

            lesson.UpdateCourse(course2);

            Assert.That(course1.Lessons.Contains(lesson), Is.False, "Lesson still in old course.");
            Assert.That(course2.Lessons.Contains(lesson), Is.True, "Lesson not added to new course.");
        }

        [Test]
        public void TestAddingLessonToCourseReverseCheck()
        {
            var course = new Course(1, "Math 101", "Basic Math Course", 30);
            var lesson = new Lesson(1, "Lesson 1", "http://video.url", "Lesson 1 Description");

            course.AddLesson(lesson);

            Assert.Multiple(() =>
            {
                Assert.That(lesson.Course, Is.EqualTo(course), "Reverse connection not correctly established.");
                Assert.That(course.Lessons.Contains(lesson), "Course is not associated with the lesson.");
            });
        }
    }
}
