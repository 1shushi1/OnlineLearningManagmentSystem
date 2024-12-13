using BYT_Project;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Project_Tests.Relation_Tests
{
    [TestFixture]
    public class InstructorCourseRelationTests
    {
        [SetUp]
        public void Setup()
        {
            // Reset static fields to ensure clean tests
            typeof(Instructor)
                .GetField("instructorsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Instructor>());

            typeof(Course)
                .GetField("coursesList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Course>());
        }

        [Test]
        public void TestAddingCourseToInstructor()
        {
            var instructor = new Instructor(1, "Mathematics");
            var course = new Course(1, "Math 101", "Basic Math Course", 30);

            instructor.AddCourse(course);

            Assert.That(instructor.Courses.Contains(course), "Course not added to instructor.");
            Assert.That(course.Instructor == instructor, "Instructor not set in course.");
        }

        [Test]
        public void TestRemovingCourseFromInstructor()
        {
            var instructor = new Instructor(1, "Mathematics");
            var course = new Course(1, "Math 101", "Basic Math Course", 30);

            instructor.AddCourse(course);
            instructor.RemoveCourse(course);

            Assert.That(!instructor.Courses.Contains(course), "Course not removed from instructor.");
            Assert.That(course.Instructor == null, "Instructor not removed from course.");
        }

        [Test]
        public void TestReverseConnectionIntegrity()
        {
            var instructor = new Instructor(1, "Mathematics");
            var course = new Course(1, "Math 101", "Basic Math Course", 30);

            instructor.AddCourse(course);
            instructor.RemoveCourse(course);

            Assert.That(course.Instructor == null, "Reverse connection not properly removed.");
        }

        [Test]
        public void TestErrorWhenAddingNullCourseToInstructor()
        {
            var instructor = new Instructor(1, "Mathematics");

            var ex = Assert.Throws<ArgumentNullException>(() => instructor.AddCourse(null));
            Assert.That(ex.ParamName, Is.EqualTo("course"));
        }

        [Test]
        public void TestErrorWhenAddingDuplicateCourseToInstructor()
        {
            var instructor = new Instructor(1, "Mathematics");
            var course = new Course(1, "Math 101", "Basic Math Course", 30);

            instructor.AddCourse(course);

            var ex = Assert.Throws<ArgumentException>(() => instructor.AddCourse(course));
            Assert.That(ex.Message, Is.EqualTo("Course is already added to this instructor."));
        }

        [Test]
        public void TestErrorWhenRemovingNonExistingCourseFromInstructor()
        {
            var instructor = new Instructor(1, "Mathematics");
            var course = new Course(1, "Math 101", "Basic Math Course", 30);

            var ex = Assert.Throws<ArgumentException>(() => instructor.RemoveCourse(course));
            Assert.That(ex.Message, Is.EqualTo("Course is not added to this instructor."));
        }

        [Test]
        public void TestUpdatingCourseInstructor()
        {
            var instructor1 = new Instructor(1, "Mathematics");
            var instructor2 = new Instructor(2, "Physics");
            var course = new Course(1, "Math 101", "Basic Math Course", 30);

            instructor1.AddCourse(course);
            course.SetInstructor(instructor2);

            Assert.That(instructor1.Courses.Contains(course), Is.False, "Course still in old instructor.");
            Assert.That(instructor2.Courses.Contains(course), Is.True, "Course not added to new instructor.");
            Assert.That(course.Instructor == instructor2, "Instructor not updated correctly in course.");
        }

        [Test]
        public void TestAddingCourseToInstructorReverseCheck()
        {
            var instructor = new Instructor(1, "Mathematics");
            var course = new Course(1, "Math 101", "Basic Math Course", 30);

            instructor.AddCourse(course);

            Assert.Multiple(() =>
            {
                Assert.That(course.Instructor, Is.EqualTo(instructor), "Reverse connection not correctly established.");
                Assert.That(instructor.Courses.Contains(course), "Instructor is not associated with the course.");
            });
        }
    }
}
